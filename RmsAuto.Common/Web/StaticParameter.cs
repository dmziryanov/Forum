using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RmsAuto.Common.Web
{
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class StaticParameter : Parameter
    {
        public StaticParameter()
        {
        }
        
        public StaticParameter(string name, object value)
            : base(name)
        {
            DataValue = value;
        }
        
        public StaticParameter(string name, TypeCode type, object value)
            : base(name, type)
        {
            DataValue = value;
        }
        
        protected StaticParameter(StaticParameter original)
            : base(original)
        {
            DataValue = original.DataValue;
        }
                
        protected override Parameter Clone()
        {
            return new StaticParameter(this);
        }
        
        public object DataValue
        {
            get
            {
                return ViewState["Value"];
            }
            set
            {
                ViewState["Value"] = value;
            }
        }
        
        public string Value
        {
            get
            {
                object o = DataValue;
                if (o == null || !(o is string))
                    return String.Empty;
                return (string)o;
            }
            set
            {
                DataValue = value;
                OnParameterChanged();
            }
        }
                
        protected override object Evaluate(HttpContext context, Control control)
        {
            if (context.Request == null)
                return null;
            return DataValue;
        }
    }
}
