using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using RmsAuto.Common.Web.UI;
using System.Web.SessionState;

namespace RmsAuto.Common.Web.HttpHandlers
{
    public class CaptureImageHandler : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
			context.Response.Cache.SetCacheability( HttpCacheability.NoCache );
            if (context.Request.QueryString.AllKeys.Count() > 0)
            {
                CaptureImage.RefreshCapture();
            }
            else
            {
                CaptureImage.InitCaptureText();
            }
            using (var ci = new CaptureImage(CaptureImage.CaptureString, 231, 50))
            {
                context.Response.Clear();
                context.Response.ContentType = "image/jpeg";
                ci.Image.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }
    }
}
