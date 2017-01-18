using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RmsAuto.Common.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple=false)]
    public class PositiveAttribute : ValidationAttribute
    {
        private Func<object, object> _convert;
        private IComparable _zero;

        public PositiveAttribute(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
                      
            if (!typeof(IComparable).IsAssignableFrom(type))
                throw new ArgumentException(type.FullName + "doesn't implement 'IComparable'", "type");
            
            TypeConverter converter = TypeDescriptor.GetConverter(type);
            _zero = (IComparable)Convert.ChangeType(0, type);
            _convert = 
                value => ((value != null) && (value.GetType() == type)) ?
                    value : converter.ConvertFrom(value);
        }
        
        public override bool IsValid(object value)
        {
            if (value == null)
                return true;
            if ((value is string) && string.IsNullOrEmpty((string)value))
                return true;
            return _zero.CompareTo(_convert(value)) < 0;
        }

        public override string FormatErrorMessage(string name)
        {
            return "значение поля '" + name + "' должно быть положительным";
        }
    }
}
