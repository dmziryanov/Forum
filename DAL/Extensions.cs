using System;
using System.ComponentModel;
using System.Reflection;

namespace DAL
{
    public static class Extensions
    {
        public static string GetDescription(this Enum Value)
        {
            Type type = Value.GetType();
            string name = Enum.GetName(type, Value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr == null)
                    {
                        return name;
                    }
                    else
                    {
                        return attr.Description;
                    }
                }
            }
            return null;

        }
    }
}