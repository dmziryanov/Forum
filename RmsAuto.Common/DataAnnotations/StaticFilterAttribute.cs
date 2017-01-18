using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Common.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple=false)]
    public class StaticFilterAttribute : Attribute
    {
        public readonly object Value;

        public StaticFilterAttribute(object value)
        {
            Value = value;
        }
    }
}
