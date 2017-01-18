using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.DynamicData;

using RmsAuto.Common.DataAnnotations;

namespace RmsAuto.Common.Web
{
    public class CrudTracker : Control
    {
        public string EditControlID
        {
            get { return (string)ViewState["__editControlID"]; }
            set { ViewState["__editControlID"] = value; }
        }

        public string CreateControlID
        {
            get { return (string)ViewState["__createControlID"]; }
            set { ViewState["__createControlID"] = value; }
        }

        public string DeleteControlID
        {
            get { return (string)ViewState["__deleteControlID"]; }
            set { ViewState["__deleteControlID"] = value; }
        }

        public string DynamicDataSourceID
        {
            get { return (string)ViewState["__dataSourceID"]; }
            set { ViewState["__dataSourceID"] = value; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            TrackCrudActions();
            base.OnPreRender(e);
        }

        private void TrackCrudActions()
        {
            if (string.IsNullOrEmpty(DynamicDataSourceID))
                throw new InvalidOperationException("DymamicDataSourceID cannot be empty");
            var dataSourceCtl = FindControl(Page.Controls, DynamicDataSourceID) as IDynamicDataSource;
            if (dataSourceCtl == null)
                throw new InvalidOperationException("DataSource  control not found or it doesn't implement IDynamicDataSource");

            var table = dataSourceCtl.GetTable();
            if (table == null)
                throw new InvalidOperationException("MetaTable not found");
            var crudAttr = table.Attributes.OfType<CrudAttribute>().FirstOrDefault();
            if (crudAttr != null && crudAttr.AllowedActions != CrudActions.All)
                HideDisabledCrudControls(Page.Controls, crudAttr.AllowedActions);
        }

        private void HideDisabledCrudControls(
            ControlCollection controls,
            CrudActions allowedActions)
        {
            foreach (Control ctl in controls)
            {
                //Mkuz: fixed a bug when, for example, CreateControlID was null and the Tracker disabled master-page-control, that does not have an Id value
                if (ctl.ID != null &&
                    (ctl.ID == CreateControlID && (allowedActions & CrudActions.Create) == 0) ||
                    (ctl.ID == EditControlID && (allowedActions & CrudActions.Update) == 0) ||
                    (ctl.ID == DeleteControlID && (allowedActions & CrudActions.Delete) == 0))
                {
                    ctl.Visible = false;
                }
                else
                {
                    HideDisabledCrudControls(ctl.Controls, allowedActions);
                }
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
    }
}
