using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Common.DataAnnotations
{
    [Flags]
    public enum CrudActions
    {
        None = 0,
        Create = 1,
        Update = 2,
        Delete = 4,
        All = Create | Update | Delete
    }
}
