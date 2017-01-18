using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Text;

namespace RmsAuto.Common.Data
{
    public class ManualTxHelper<T> : IDisposable 
        where T : DataContext, new()
    {
        private T _dataContext;
        private bool _commit;
        private bool _disposed;

        public ManualTxHelper() : this(IsolationLevel.ReadCommitted, true)
        {
        }
        
        public ManualTxHelper(IsolationLevel isolationLevel, bool autoCommit)
        {
            _dataContext = new T();
            _commit = autoCommit;
            _dataContext.Connection.Open();
            _dataContext.Transaction = _dataContext
                .Connection
                .BeginTransaction(isolationLevel);
        }

        public T DataContext
        {
            get 
            {
                if (_disposed)
                    throw new ObjectDisposedException("ManualTxHelper");
                return _dataContext; 
            }
        }

        public void SetCommit()
        {
            if (_disposed)
                throw new ObjectDisposedException("ManualTxHelper");

            _commit = true;
        }

        public void SetRollback()
        {
            if (_disposed)
                throw new ObjectDisposedException("ManualTxHelper");

            _commit = false;
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (_commit)
                    _dataContext.Transaction.Commit();
                else
                    _dataContext.Transaction.Rollback();
                _dataContext.Dispose();

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true /*called by user directly*/);
            GC.SuppressFinalize(this);
        }


    }
}
