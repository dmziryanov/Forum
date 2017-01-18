using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Xml;

namespace RmsAuto.Common.Misc
{
    public static class MiscExtensions
    {
        public static IEnumerable<string> ReadLines(this TextReader source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            string line; 
            while ((line = source.ReadLine()) != null)
                yield return line;
        }

        public static string GetNameOnly(this FileInfo file)
        {
            if (file == null)
                throw new ArgumentNullException("file");
            return file.Name.Substring(0, file.Name.IndexOf('.'));
        }

        public static IEnumerable<int> Progression(this int seed, int increment, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return seed;
                seed += increment;
            }
        }

        public static string JsonSerializer<T>(this IHttpHandler h, T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }

		public static string JsonSerializerWithRoot<T>(this IHttpHandler h, T t)
		{
			DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));

			MemoryStream ms = new MemoryStream();
			XmlDictionaryWriter w = JsonReaderWriterFactory.CreateJsonWriter(ms);
			w.WriteStartElement("root");
			w.WriteAttributeString("type", "object");
			ser.WriteObject(w, t);
			w.WriteEndElement();
			w.Flush();
			string jsonString = Encoding.UTF8.GetString(ms.ToArray());
			ms.Close();
			return jsonString;
		} 
    }
}
