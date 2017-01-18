using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Common.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SortAttribute : Attribute
    {
        public readonly string Expression;
        
        public SortAttribute(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                throw new ArgumentException("Sort expression cannot be empty", "expression");
            Expression = expression;
        }
    }
}
