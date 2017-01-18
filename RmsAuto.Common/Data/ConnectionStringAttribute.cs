using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace RmsAuto.Common.Data
{
    public class ConnectionStringAttribute : Attribute
    {
        public string ConnectionString { get; private set; }

        public ConnectionStringAttribute(string cs)
        {
            this.ConnectionString = cs;
        }
    }
}
