using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Reflection;

namespace RmsAuto.Common.Misc
{
    public static class EnumHelpers
    {
        class Mappings
        {
            public readonly NameValueCollection NameToText = new NameValueCollection();
            public readonly NameValueCollection AltNameToName = new NameValueCollection();
        }

        private static Dictionary<Type, Mappings> _mappings = new Dictionary<Type, Mappings>();
        private static object _syncObj = new object();
                      
        public static string ToTextOrName(this Enum value)
        {
            string s = value.ToString();
            Type enumType = value.GetType();
            NameValueCollection mapping = GetNameToTextMappings(enumType);

            string res = mapping[s];

            if (res != null)
                return res;
            else if (enumType.IsDefined(typeof(FlagsAttribute), false))
            {
                StringBuilder sb = new StringBuilder();
                bool first = true;

                foreach (string sPart in s.Split(new char[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (sPart == "")
                        continue;

                    if (first) first = false;
                    else sb.Append(", ");

                    string namePart = mapping[sPart] ?? sPart;
                    sb.Append(namePart);
                }

                return sb.ToString();
            }
            else return s;
        }
                
        public static T ParseAltName<T>(string altName)
        {
            var t = typeof(T);
            if (!t.IsEnum)
                throw new ArgumentException("<T> must be of Enum type");
            if (string.IsNullOrEmpty(altName))
                throw new ArgumentException("Enumerated constant alternative name cannot be empty", "altName");
            string name = GetAltNameToNameMappings(t)[altName];
            if (name == null)
                throw new Exception("Alternating constant name doesn't exist");
            return (T)Enum.Parse(t, name);
        }

        
        private static NameValueCollection GetNameToTextMappings(Type type)
        {
            ReflectMappings(type);
            return _mappings[type].NameToText;
        }

        private static NameValueCollection GetAltNameToNameMappings(Type type)
        {
            ReflectMappings(type);
            return _mappings[type].AltNameToName;
        }

        private static void ReflectMappings(Type type)
        {
            if (!_mappings.ContainsKey(type))
            {
                lock (_syncObj)
                {
                    if (!_mappings.ContainsKey(type))
                    {
                        var mappings = new Mappings();
                        foreach (FieldInfo field in type.GetFields())
                            if (field.IsLiteral)
                            {
                                var textAttr = (TextAttribute)Attribute.GetCustomAttribute(field, typeof(TextAttribute));
                                if (textAttr != null)
                                    mappings.NameToText.Add(field.Name, textAttr.Text);
                                var altAttr = (AltNameAttribute)Attribute.GetCustomAttribute(field, typeof(AltNameAttribute));
                                if (altAttr != null)
                                    mappings.AltNameToName.Add(altAttr.Name, field.Name);
                            }
                        _mappings.Add(type, mappings);
                    }
                }
            }
        }

        public static string GetDescription(this Enum enumValue)
        {
            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return enumValue.ToString();
        }
    }

    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        private readonly string _resourceKey;
        private readonly ResourceManager _resource;
        public LocalizedDescriptionAttribute(string resourceKey, Type resourceType)
        {
            _resource = new ResourceManager(resourceType);
            _resourceKey = resourceKey;
        }

        public override string Description
        {
            get
            {
                string displayName = _resource.GetString(_resourceKey);

                return string.IsNullOrEmpty(displayName)
                    ? string.Format("[[{0}]]", _resourceKey)
                    : displayName;
            }
        }
    }
}
