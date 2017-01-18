using System.Collections;
using System.Linq;
using RmsAuto.Common.Misc;

namespace System.Web.UI.WebControls
{
    public static class ControlsExt
    {
        public static void FillItemsFromEnumTexts<T>(this DropDownList current, bool onlyNoPostBack, T[] exceptValues )
        {
            if ((!current.Page.IsPostBack && onlyNoPostBack) || !onlyNoPostBack)
            {
                /*
                EnumHelpers.GetSortedPairs<T>().Each(p => current.Items.Add(new ListItem()
                {
                    Value = Convert.ToInt64(p.Key).ToString(),
                    Text = p.Value
                }));
                 */
                if (!typeof(T).IsEnum)
                {
                    throw new ArgumentException("<T> must be of Enum type");
                }
                foreach (var value in Enum.GetValues(typeof(T)))
                {
					if( exceptValues == null || Array.IndexOf( exceptValues, value ) < 0 )
					{
						current.Items.Add( new ListItem( ( (Enum)value ).ToTextOrName(), value.ToString() ) );
					}
                }
            }
        }

        public static void FillItemsFromEnumTexts<T>(this DropDownList current)
        {
            current.FillItemsFromEnumTexts<T>(false,null);
        }

		public static void FillItemsFromEnumTexts<T>( this DropDownList current, T[] exceptValues )
		{
			current.FillItemsFromEnumTexts<T>( false, exceptValues );
		}

        public static bool SetSelected(this DropDownList current, object val)
        {
            //не работает :(
            //current.SelectedValue = val.ToString();
            var sVal = val.ToString();
            bool wasFound = false;
            foreach (ListItem item in current.Items)
            {
                if (item.Value == sVal)
                {
                    current.Items.Each<ListItem>(l => l.Selected = false);
                    item.Selected = true;
                    wasFound = true;
                    break;
                }
            }

            return wasFound;
        }

        public static bool IsDataBound(this RepeaterItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            return item.ItemType == ListItemType.Item ||
                item.ItemType == ListItemType.AlternatingItem ||
                item.ItemType == ListItemType.EditItem ||
                item.ItemType == ListItemType.SelectedItem;
        }

        public static void BindEnumeration(this ListControl listControl, Type enumType, Enum selectedValue)
        {
            if (listControl == null)
                throw new ArgumentNullException("listControl");
            if (enumType == null)
                throw new ArgumentNullException("enumType");

            foreach (var value in Enum.GetValues(enumType))
            {
                listControl.Items.Add(
                    new ListItem(((Enum)value).GetDescription(), value.ToString())
                    { Selected = ((Enum)value).Equals(selectedValue) });
            }
        }

        public static T GetSelectedEnumValue<T>(this ListControl listControl)
        {
            if (listControl == null)
                throw new ArgumentNullException("listControl");
            return (T)Enum.Parse(typeof(T), listControl.SelectedValue);
        }
   
        public static Control FindControl(this ControlCollection controls, string id)
        {
            if (controls == null)
                throw new ArgumentNullException("controls");
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Control id cannot be empty", "id");

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

        public static void ShowMessage(this Page page, string message)
        {
            if (page == null)
                throw new ArgumentNullException("page");
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Message to display cannot be empty", "message");

            page.ClientScript.RegisterStartupScript(
                page.GetType(),
                "__messageBox",
                "<script type='text/javascript'>alert('" + message + "');</script>");
        }

        private static void ShowMessage(this System.Web.UI.UserControl ctrl, string message)
        {
            ctrl.Page.ClientScript.RegisterStartupScript(
                ctrl.GetType(),
                "__messageBox",
                "<script type='text/javascript'>alert('" + message + "');</script>");
        }
    }
}
