using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RmsAuto.Common.Web.UI
{
    public class Comment : Literal
    {
        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write("<!--");
            base.Render(writer);
            writer.Write("-->");
        }
    }
}
