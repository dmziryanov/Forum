using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace RmsAuto.Common.Net
{
    public class TimedWebClient : WebClient
    {
        public int? Timeout { get; set; }

        protected override WebRequest GetWebRequest(Uri address)
        {   
            WebRequest request = base.GetWebRequest(address);   
 
            if (Timeout.HasValue && request.GetType() == typeof(HttpWebRequest))   
            {   
                ((HttpWebRequest)request).Timeout = Timeout.Value;   
            }
   
            return request;   
        }   
    }
}
