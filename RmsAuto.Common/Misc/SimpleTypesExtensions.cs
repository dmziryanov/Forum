using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Common.Misc
{
    public static class SimpleTypesExtensions
    {
        public static string To_ddMMyyyy_HHmmss_String(this DateTime value)
        {
            return value.ToString("dd.MM.yyyy HH:mm:ss");
        }
    }
}
