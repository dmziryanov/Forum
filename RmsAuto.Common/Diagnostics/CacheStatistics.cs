using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Common.Diagnostics
{
    [Serializable]
    public class CacheStatistics
    {
        public string ReferenceName { get; set; }
        public string ReferenceDescription { get; set; }
        public DateTime InitTime { get; set; }
        public int ItemCount { get; set; }
        public int SyncRequestCount { get; set; }
        public DateTime? LastSyncTime { get; set; }
        public DateTime? ExpirationTime { get; set; }
        public DateTime DiagTime { get; set; }
    }
}
