using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;

namespace System.Xml.Serialization
{
    public static class IXmlSerializableExt
    {
        /// <summary>
        /// Сериализовать сингл-инстанс
        /// </summary>
        /// <param name="ISer">IXmlSerializable инстанс</param>
        /// <param name="rootName">Имя рута/param>
        /// <param name="itemName">Имя айтема</param>
        /// <returns>Xml-строчку</returns>
        public static string SerializeSingle(this IXmlSerializable ISer, string rootName, string itemName)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(sb);
            writer.WriteStartDocument();

            writer.WriteStartElement(rootName);

            if (!String.IsNullOrEmpty(itemName))
            {
                writer.WriteStartElement(itemName);
            }

            ISer.WriteXml(writer);

            if (!String.IsNullOrEmpty(itemName))
            {
                writer.WriteEndElement();
            }

            writer.WriteEndElement();

            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }
    }
}
