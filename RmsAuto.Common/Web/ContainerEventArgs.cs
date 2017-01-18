using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RmsAuto.Common.Web
{
    public class ContainerEventArgs<T> : EventArgs where T : class
    {
        public T Value { get; protected set; }

        public ContainerEventArgs(T value)
        {
            this.Value = value;
        }
    }
}
