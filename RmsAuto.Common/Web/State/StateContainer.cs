using System.Collections.Generic;

namespace RmsAuto.Common.Web.State
{
	public class StateContainer
	{
		private Dictionary<string, StateItem> _items;

		public StateItem PageIndex
		{
			get { return _items["PageIndex"]; }
			set { _items["PageIndex"] = value; }
		}

		public StateItem PageSize
		{
			get { return _items["PageSize"]; }
			set { _items["PageSize"] = value; }
		}

		public StateItem SortExpression
		{
			get { return _items["SortExpression"]; }
			set { _items["SortExpression"] = value; }
		}

		public StateItem SortDirection
		{
			get { return _items["SortDirection"]; }
			set { _items["SortDirection"] = value; }
		}

		public Dictionary<string, StateItem> Filters;
		
		public StateContainer()
		{
			_items = new Dictionary<string, StateItem>();
			Filters = new Dictionary<string, StateItem>();
			PageIndex = new StateItem(0, 0, false);
			PageSize = new StateItem(10, 10, true);
			SortExpression = new StateItem("", "", true );
			SortDirection = new StateItem(System.Web.UI.WebControls.SortDirection.Ascending, System.Web.UI.WebControls.SortDirection.Ascending, true);
		}

		public void Reset()
		{
			foreach (KeyValuePair<string, StateItem> item in _items)
			{
				if (!item.Value.IsPersistent)
					item.Value.Value = item.Value.DefaultValue;
			}

			foreach (KeyValuePair<string, StateItem> filter in Filters)
			{
				if (!filter.Value.IsPersistent)
					filter.Value.Value = filter.Value.DefaultValue;
			}
		}
	}

}
