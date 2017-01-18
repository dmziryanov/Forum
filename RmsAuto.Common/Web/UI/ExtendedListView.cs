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
    public class ExtendedListView : ListView
    {
        bool _isLayoutTemplateDataBound = false;

        protected override void CreateLayoutTemplate()
        {
            base.CreateLayoutTemplate();

             if (this.Controls.Count == 1 && !_isLayoutTemplateDataBound)  
             {  
                 this.Controls[0].DataBind();  
                 _isLayoutTemplateDataBound = true;  
             }  
          }
    }
}
