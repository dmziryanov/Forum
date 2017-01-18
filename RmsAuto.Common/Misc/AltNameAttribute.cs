using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Common.Misc
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple=false, Inherited=false)]
    public class AltNameAttribute : Attribute
    {
        private string _name;

        public AltNameAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Alternative type or member name cannot be empty", "name");
            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }
    }
}
