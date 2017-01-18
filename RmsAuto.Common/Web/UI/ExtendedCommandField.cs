using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RmsAuto.Common.Web.UI
{
    public class ExtendedCommandField : CommandField
    {
        [Category("Behavior")]
        [Description("Текст, который выводится, когда жмется кнопка удаления.")]
        public string DeleteConfirmationText
        {
            get { return this.ViewState["DeleteConfirmationText"] as string; }
            set { this.ViewState["DeleteConfirmationText"] = value; }
        }

        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
        {
            base.InitializeCell(cell, cellType, rowState, rowIndex);

            if (!String.IsNullOrEmpty(this.DeleteConfirmationText) && this.ShowDeleteButton)
            {
                foreach (Control control in cell.Controls)
                {
                    IButtonControl button = control as IButtonControl;
                    if (button != null && button.CommandName == "Delete")
                    {
                        ((WebControl)control).Attributes.Add("onclick",
                                                             String.Format("if (!confirm('{0}')) return false;", this.DeleteConfirmationText));
                    }
                }
            }
        }
    }
}
