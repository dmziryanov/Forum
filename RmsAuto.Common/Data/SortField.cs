using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Common.Data
{
    public class SortField
    {
        public string FieldText { get; protected set; }
        public string FieldName { get; protected set; }
        public bool SortByThisField { get; set; }

        public SortField(string fieldText, string fieldName, bool sort)
        {
            this.FieldText = fieldText;
            this.FieldName = fieldName;
            this.SortByThisField = sort;
        }
    }
}
