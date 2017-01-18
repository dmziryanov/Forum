using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RmsAuto.Common.Misc
{
    public static class StringExtensions
    {

        public static bool IsNumeric(this string s)
        {
            if (s == null)
                throw new ArgumentNullException("s");
            return s.ToCharArray().All(c => Char.IsDigit(c));
        }

        public static string IfEmpty(this string s, string value)
        {
            //if (s == null)
                //throw new ArgumentNullException("s");
            return s != null ? s : value;         
            //return s.Length > 0 ? s : value;         
        }

		public static string WithBrackets(this string s)
		{
			//if (s == null)
			//throw new ArgumentNullException("s");
			return s == null ? s : "(" + s + ")";
		}

        public static string Combine(this IEnumerable<string> strings, string delimiter)
        {
            if (strings == null)
                return null; 
            var ret = new StringBuilder();
            foreach (string s in strings)
            {
                if (ret.Length > 0)
                    ret.Append(delimiter);
                if (!string.IsNullOrEmpty(s))
                    ret.Append(s);
            }
            return ret.ToString();
        }
    }
}
