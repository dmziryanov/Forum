using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Common.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public class CrudAttribute : Attribute
    {
        private CrudActions _allowedActions;

        public CrudAttribute(CrudActions allowedActions)
        {
            _allowedActions = allowedActions; 
        }

        public CrudActions AllowedActions
        {
            get { return _allowedActions; }
        }
    }
}
