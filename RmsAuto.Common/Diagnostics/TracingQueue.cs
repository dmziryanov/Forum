using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Common.Diagnostics
{
    class TracingQueue
    {
        private int _size;
        private Queue<TraceItem> _queue;
        private readonly object _sync = new object();
        
        public TracingQueue(int size)
        {
            if (size <= 0)
                throw new ArgumentException("Size of tracing queue must be greater than zero", "size");
            _size = size;
            _queue = new Queue<TraceItem>();
        }

        public void AddTraceData(string action, string requestXml, string responseXml, bool errorOccured)
        {
            if (string.IsNullOrEmpty(action))
                throw new ArgumentException("Action cannot be empty", "action");
            lock (_sync)
            {
                if (_queue.Count == _size)
                    _queue.Dequeue();
                _queue.Enqueue(
                    new TraceItem()
                    {
                        ItemId = Guid.NewGuid(),
                        TraceTime = DateTime.Now,
                        Action = action,
                        RequestXml = requestXml,
                        ResponseXml = responseXml,
                        ErrorOccured = errorOccured
                    });
            }
        }

        public TraceItem[] GetItems()
        {
            lock (_sync)
                return _queue.ToArray();
        }

        public void Clear()
        {
            lock (_sync)
                _queue.Clear();
        }
    }
}
