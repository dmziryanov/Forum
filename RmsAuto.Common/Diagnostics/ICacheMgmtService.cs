using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Common.Diagnostics
{
    public interface ICacheMgmtService
    {
        CacheStatistics[] GetCacheStatistics();
        void PurgeReferenceCache(string referenceName);
    }
}
