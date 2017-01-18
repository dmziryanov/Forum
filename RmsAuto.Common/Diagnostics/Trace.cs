using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace RmsAuto.Common.Diagnostics
{
    //public static class Trace
    //{
    //    private static TracingQueue _traceDst;
    //    private static bool _initialized;
    //    private static readonly object _sync = new object();
    //    private const int _DefaultRequestLimit = 10;
        
    //    public static bool Enabled
    //    {
    //        get 
    //        {
    //            Initialize();
    //            return _traceDst != null;
    //        }
    //    }

    //    internal static void Add(string action, string requestXml, string responseXml, bool errorOccured)
    //    {
    //        Initialize();
    //        EnsureEnabled();
    //        _traceDst.AddTraceData(action, requestXml, responseXml, errorOccured);
    //    }

    //    public static void ClearTracingQueue()
    //    {
    //        Initialize();
    //        EnsureEnabled();
    //        _traceDst.Clear();
    //    }

    //    public static TraceItem[] Items
    //    {
    //        get
    //        {
    //            Initialize();
    //            EnsureEnabled();
    //            return _traceDst.GetItems();
    //        }
    //    }

    //    private static void Initialize()
    //    {
    //        if (!_initialized)
    //            lock (_sync)
    //                if (!_initialized)
    //                {
    //                    if (ServiceProxyConfiguration.Current.TraceEnabled)
    //                        _traceDst = new TracingQueue(
    //                            ServiceProxyConfiguration.Current.TraceRequestLimit > 0 ?
    //                            ServiceProxyConfiguration.Current.TraceRequestLimit :
    //                            _DefaultRequestLimit
    //                            );
    //                    _initialized = true;
    //                }
    //    }
               
    //    private static void EnsureEnabled()
    //    {
    //        if (_traceDst == null)
    //            throw new InvalidOperationException("Tracing is disabled");
    //    }
    //}
}
