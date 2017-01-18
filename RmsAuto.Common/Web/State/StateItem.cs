using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Common.Web.State
{
	public class StateItem
	{
		public object Value
		{ 
			get; set;
		}
		
		public bool IsPersistent
		{
			get; set;
		}

		public object DefaultValue
		{
			get; private set;
		}

		public StateItem(object value, object defaultValue, bool isPersistent)
		{
			Value = value;
			DefaultValue = defaultValue;
			IsPersistent = isPersistent;
		}
	}
}
