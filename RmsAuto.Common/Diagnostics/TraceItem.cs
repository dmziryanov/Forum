using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Common.Diagnostics
{
    public class TraceItem
    {
        public Guid ItemId
        {
            get;
            internal set;
        }

        public DateTime TraceTime
        {
            get;
            internal set;
        }

        public string Action
        {
            get;
            internal set;
        }

        public string RequestXml
        {
            get;
            internal set;
        }

        public string ResponseXml
        {
            get;
            internal set;
        }

        public bool ErrorOccured
        {
            get;
            internal set;
        }
    }
}
