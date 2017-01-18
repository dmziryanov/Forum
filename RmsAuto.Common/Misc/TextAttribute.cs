using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Common.Misc
{
    [AttributeUsage(AttributeTargets.All)]
    public class TextAttribute : Attribute
    {
        private string _text;

        public TextAttribute(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("text cannot be empty", "text");
            _text = text;
        }

        public string Text
        {
            get { return _text; }
        }
    }
}
