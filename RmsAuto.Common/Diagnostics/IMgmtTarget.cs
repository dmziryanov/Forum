using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Common.Diagnostics
{
    interface IMgmtTarget
    {
        void Purge();
        CacheStatistics GetStatistics();
    }
}
