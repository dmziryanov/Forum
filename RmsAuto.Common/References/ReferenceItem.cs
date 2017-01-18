using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RmsAuto.Common.References
{
    public class ReferenceItem
    {
        public string Key { get; set; }

        [XmlElement("Value")]
        public string TextValue { get; set; }
    }
}
