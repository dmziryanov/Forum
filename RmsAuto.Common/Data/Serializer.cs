using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization.Formatters.Binary;

namespace RmsAuto.Common.Data
{
    public static class Serializer
    {


        public static T DeepCopy<T>(T obj)
        {
            object result = null;

            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                result = (T)formatter.Deserialize(ms);
                ms.Close();
            }

            return (T)result;
        }
        
        public static T ShallowClone<T>(T t, T n)
        {
            throw new NotImplementedException();
            
        }
        
        public static T JsonClone<T>(T t, T n)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            ms.Position = 0;
            n = (T)ser.ReadObject(ms);
            ms.Close();
            return n;
        }

        /// <summary>Clones an object.</summary>
        /// <typeparam name="T">The type of object to be cloned.</typeparam>
        /// <param name="source">The object to be cloned.</param>
        /// <returns>A clone of the specified object.</returns>
        public static T Clone<T>(T source)
        {
            return Deserialize<T>(Serialize(source));
        }

        /// <summary>Serializes an object as an XML string.</summary>
        /// <param name="value">A System.Object representing the object to be serialized.</param>
        /// <returns>A System.String representing an XML representation of the specified object.</returns>
        public static string Serialize(object value)
        {
            if (value.GetType() == typeof(string))
            {
                return value.ToString();
            }

            StringWriter stringWriter = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(stringWriter))
            {
                DataContractSerializer serializer = new DataContractSerializer(value.GetType());
                serializer.WriteObject(writer, value);
            }

            return stringWriter.ToString();
        }

        /// <summary>Creates an object from an XML representation of the object.</summary>
        /// <typeparam name="T">The type of object to be created.</typeparam>
        /// <param name="serializedValue">A System.String representing an XML representation of an object.</param>
        /// <returns>A new object.</returns>
        public static T Deserialize<T>(string serializedValue)
        {
            Type type = typeof(T);
            using (StringReader stringReader = new StringReader(serializedValue))
            {
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    DataContractSerializer serializer = new DataContractSerializer(type);
                    object deserializedValue = serializer.ReadObject(reader);
                    return (T)deserializedValue;
                }
            }
        }
    }
}
