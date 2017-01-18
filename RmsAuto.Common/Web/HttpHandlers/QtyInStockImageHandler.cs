using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using RmsAuto.Common.Web.UI;
using System.Web.SessionState;

namespace RmsAuto.Common.Web.HttpHandlers
{
    public class QtyInStockImageHandler : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
			context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
			string key = context.Request.QueryString["id"];
			var qty = string.IsNullOrEmpty(key) ? "" : context.Session[key].ToString();
			using (var ci = new QtyImage(qty.ToString(), 50, 50))
			{
				context.Response.Clear();
				context.Response.ContentType = "image/jpeg";
				ci.Image.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
			}
        }
    }
}
