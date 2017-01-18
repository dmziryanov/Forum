using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Common.Web.State;
using StateItem = RmsAuto.Common.Web.State.StateItem;

namespace RmsAuto.Common.Web
{
	public class StateTracker : Control
	{
		private GridView _gridView;
		private DetailsView _detailView;
		private List<DropDownList> _filters;
		private DropDownList _pageSizer;
		private ActionMode _actionMode;
		private string _stateName;

		[Browsable(true), DefaultValue("GridView1")]
		public string GridViewID { get; set; }

		[Browsable(true), DefaultValue("FilterRepeater")]
		public string FilterRepeaterID { get; set; }

		[Browsable(true), DefaultValue("DetailsView1")]
		public string DetailsViewID { get; set; }

		private string RecentName
		{
			get { return Convert.ToString(HttpContext.Current.Session["stRecentName"]); }
			set { HttpContext.Current.Session["stRecentName"] = value; }
		}

		private Dictionary<string, StateContainer> Items
		{
			get { return (Dictionary<string, StateContainer>)HttpContext.Current.Session["StateTracker"]; }
			set { HttpContext.Current.Session["StateTracker"] = value; }
		}

		protected override void OnLoad(EventArgs e)
		{

			if (!string.IsNullOrEmpty(DetailsViewID) && !string.IsNullOrEmpty(GridViewID))
				throw new InvalidOperationException("Too many controls defined");
			if (string.IsNullOrEmpty(DetailsViewID) && string.IsNullOrEmpty(GridViewID))
				throw new InvalidOperationException("GridView or DetailsView ID is missing");

			if (Items == null)
				Items = new Dictionary<string, StateContainer>();

			_actionMode = string.IsNullOrEmpty(DetailsViewID) ? (string.IsNullOrEmpty(GridViewID) ? ActionMode.None : ActionMode.List) : ActionMode.Edit;

			

            switch (_actionMode)
			{
				case ActionMode.Edit:
					_detailView = (DetailsView)Parent.FindControl(DetailsViewID);
					if (_detailView != null)
					{
						_stateName = GetTableName(_detailView);
						if (!Items.ContainsKey(_stateName))
							Items.Add(_stateName, new StateContainer());
					}
					break;
				case ActionMode.List:
					_gridView = (GridView)Parent.FindControl(GridViewID);

					if (_gridView != null)
					{
						_stateName = GetTableName(_gridView);
						if (!Items.ContainsKey(_stateName))
							Items.Add(_stateName, new StateContainer());

						_gridView.PageIndexChanged += GridViewPagerIndexChanged;
						_gridView.Sorted += GridViewSorted;
						if (_gridView.BottomPagerRow != null && _gridView.BottomPagerRow.Cells[0].FindControl("GridViewPager") != null)
						{
							_pageSizer =
								(DropDownList)
								_gridView.BottomPagerRow.Cells[0].FindControl("GridViewPager").TemplateControl.FindControl(
									"DropDownListPageSize");

							if (_pageSizer != null)
								_pageSizer.SelectedIndexChanged += PageSizeChanged;
						}
						if (!string.IsNullOrEmpty(FilterRepeaterID))
						{
							FilterRepeater filterRepeater = (FilterRepeater)FindControl(Parent.Controls, FilterRepeaterID);
							if (filterRepeater != null)
							{
								List<UserControl> filterControls = GetFilterControls(filterRepeater);
								_filters = new List<DropDownList>();
								foreach (UserControl ctl in filterControls)
								{
									DropDownList listbox = ctl.FindControl("DropDownList1") as DropDownList;
									if (listbox != null)
									{
										_filters.Add(listbox);
										listbox.SelectedIndexChanged += FilterSelectedIndexChanged;
									}
								}
								if (_filters.Count > 0)
								{
									Button btnClear = new Button();
									btnClear.Text = "Снять все фильтры";
									btnClear.Click += ClearFilters;
									btnClear.CssClass = "dropdown";
									Controls.Add(btnClear);
								}
							}
						}
					}

					break;
			}
			if (!string.IsNullOrEmpty(RecentName) && _stateName != RecentName)
				Items[RecentName].Reset();

			RecentName = _stateName;
			base.OnLoad(e);
		}

		private string GetTableName(DataBoundControl control)
		{
			var dataSourceCtl = FindControl(Page.Controls, control.DataSourceID) as IDynamicDataSource;
			if (dataSourceCtl == null)
				throw new InvalidOperationException("DataSource  control not found or it doesn't implement IDynamicDataSource");

			var table = dataSourceCtl.GetTable();
			if (table == null)
				throw new InvalidOperationException("MetaTable not found");
			return table.Name;
		}

		protected override void OnPreRender(EventArgs e)
		{
			

			if (!Page.IsPostBack)
			{
				switch (_actionMode)
				{
					case ActionMode.List:
						ApplyFilters();
						_gridView.Sort(Items[_stateName].SortExpression.Value.ToString(), (SortDirection)Items[_stateName].SortDirection.Value);
						_gridView.PageSize = Convert.ToInt32(Items[_stateName].PageSize.Value);
						_gridView.PageIndex = Convert.ToInt32(Items[_stateName].PageIndex.Value);
						break;
					case ActionMode.Edit:
						break;
				}
			}
			
			base.OnPreRender(e);
		}

		private void ApplyFilters()
		{
			if (_filters != null)
				foreach (DropDownList item in _filters)
				{
					if (Items[_stateName].Filters.ContainsKey(item.ClientID))
						item.SelectedValue = Items[_stateName].Filters[item.ClientID].Value.ToString();
				}
		}

		private Control FindControl(ControlCollection controls, string id)
		{
			foreach (Control ctl in controls)
			{
				if (ctl.ID == id)
					return ctl;

				var ret = FindControl(ctl.Controls, id);
				if (ret != null)
					return ret;
			}
			return null;
		}

		public List<UserControl> GetFilterControls(FilterRepeater filterRepeater)
		{
			var filters = new List<UserControl>();

			foreach (RepeaterItem item in filterRepeater.Items)
			{
				var filter = item.Controls.OfType<UserControl>().FirstOrDefault();
				filters.Add(filter);
			}

			return filters;
		}

		#region Event handlers

		private void ClearFilters(object sender, EventArgs e)
		{
			foreach (DropDownList item in _filters)
			{
				item.SelectedIndex = 0;
				if (Items[_stateName].Filters.ContainsKey(item.ClientID))
					Items[_stateName].Filters[item.ClientID].Value = "";

			}
		}

		private void FilterSelectedIndexChanged(object sender, EventArgs e)
		{
			DropDownList list = (DropDownList)sender;
			if (Items[_stateName].Filters.ContainsKey(list.ClientID))
				Items[_stateName].Filters[list.ClientID].Value = list.SelectedValue;
			else
				Items[_stateName].Filters[list.ClientID] = new StateItem(list.SelectedValue, "", false);
		}

		private void PageSizeChanged(object sender, EventArgs e)
		{
			Items[_stateName].PageSize.Value = _pageSizer.SelectedValue;
			Items[_stateName].PageIndex.Value = 0;
			_gridView.PageIndex = 0;
		}

		private void GridViewSorted(object sender, EventArgs e)
		{
			if (Page.IsPostBack)
			{
				Items[_stateName].SortExpression.Value = _gridView.SortExpression;
				Items[_stateName].SortDirection.Value = _gridView.SortDirection;
				Items[_stateName].PageIndex.Value = 0;
			}
		}

		private void GridViewPagerIndexChanged(object sender, EventArgs e)
		{
			Items[_stateName].PageIndex.Value = _gridView.PageIndex;
		}
		#endregion


	}
}
