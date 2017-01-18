using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RmsAuto.Common.Web.UI
{
    [ParseChildren(true, "Options")]
    public class SortOptions : Control
    {
        private List<SortOption> _sortOptions;

        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        public List<SortOption> Options
        {
            get
            {
                if (_sortOptions == null)
                    _sortOptions = new List<SortOption>();
                return _sortOptions;
            }
        }

        public string OptionListControlID
        {
            get;
            set;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack && _sortOptions != null && _sortOptions.Count > 0)
            {
                var listControl = Page.Controls.FindControl(OptionListControlID);
                if (listControl == null)
                    throw new Exception("Control not found. ID: " + OptionListControlID);
                if (!(listControl is ListControl))
                    throw new Exception("Control must be of type 'ListControl'. ID: " + OptionListControlID);

                _sortOptions.ForEach(so => 
                    ((ListControl)listControl).Items.Add(
                    new ListItem(so.DisplayText, so.SortExpression)));
            }
            base.OnLoad(e);
        }
    }
}
