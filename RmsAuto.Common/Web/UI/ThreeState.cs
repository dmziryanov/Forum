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
    [DefaultProperty("Checked")]
    [ToolboxData("<{0}:ThreeState runat=server></{0}:NullableBoolSetter>")]
    public class ThreeState : WebControl, INamingContainer, IPostBackDataHandler
    {
        #region props

        const string DefaultNotSetText = "Не выбрано";
        const string DefaultFalseText = "Нет";
        const string DefaultTrueText = "Да";

        string _notSetText = DefaultNotSetText,
               _trueText = DefaultTrueText,
               _falseText = DefaultFalseText;

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(null)]
        [Localizable(true)]
        public bool? Checked
        {
            get
            {
                return (bool?)ViewState["__Checked"];
            }

            set
            {
                ViewState["__Checked"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(DefaultTrueText)]
        [Localizable(true)]
        public string TrueText
        {
            get { return this._trueText; }
            set { this._trueText = value; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(DefaultFalseText)]
        [Localizable(true)]
        public string FalseText
        {
            get { return this._falseText; }
            set { this._falseText = value; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(DefaultNotSetText)]
        [Localizable(true)]
        public string NotSetText
        {
            get { return this._notSetText; }
            set { this._notSetText = value; }
        }

        bool IsCheckedChanged
        {
            get
            {
                return this.ViewState.IsItemDirty("__Checked");
            }
        }

        #endregion

        #region events & overrides

        protected override void RenderContents(HtmlTextWriter output)
        {
            WriteOption(output, String.Empty, this.NotSetText);
            WriteOption(output, "0", this.FalseText);
            WriteOption(output, "1", this.TrueText);
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Select;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            string nameName = HtmlTextWriterAttribute.Name.ToString().ToLower();
            string idName = HtmlTextWriterAttribute.Id.ToString().ToLower();
            string tagName = this.TagKey.ToString().ToLower();
            writer.WriteBeginTag(tagName);

            writer.WriteAttribute(idName, this.ClientID);
            writer.WriteAttribute(nameName, this.UniqueID);
            this.Attributes.Each(a => writer.WriteAttribute(a.Key, a.Value));
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.WriteLine();

            writer.Indent++;

            RenderContents(writer);

            writer.Indent--;
            WriteIndent(writer);

            writer.WriteEndTag(tagName);
        }

        public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            var hasData = postCollection.AllKeys.Any(k => k.Equals(postDataKey, StringComparison.OrdinalIgnoreCase));
            if (hasData)
            {
                var val = postCollection.TryGet<string>(postDataKey, String.Empty);
                if (this.Checked != StringToBool(val))
                {
                    this.Checked = StringToBool(val);
                }
            }
            return hasData;
        }

        public void RaisePostDataChangedEvent()
        {
            if (OnSelectedIndexChanged != null)
            {
                OnSelectedIndexChanged(this, new EventArgs());
            }
        }

        public event EventHandler OnSelectedIndexChanged;

        #endregion

        #region helpers

        void WriteOption(HtmlTextWriter output, string val, string text)
        {
            string tagName = HtmlTextWriterTag.Option.ToString().ToLower();
            string valueName = HtmlTextWriterAttribute.Value.ToString().ToLower();
            string selName = HtmlTextWriterAttribute.Selected.ToString().ToLower();

            WriteIndent(output);

            output.WriteBeginTag(tagName);
            output.WriteAttribute(valueName, val);
            if (this.IsCheckedChanged &&
                StringToBool(val) == this.Checked)
            {
                output.WriteAttribute(selName, selName);
            }
            output.Write(HtmlTextWriter.TagRightChar);

            output.Write(text);
            output.WriteEndTag(tagName);
            output.WriteLine();
        }

        void WriteIndent(HtmlTextWriter output)
        {
            for (int i = 0; i < output.Indent; i++)
            {
               output.Write("\t");
            }
        }

        bool? StringToBool(string val)
        {
            switch (val)
            {
                case "0":
                    {
                        return false;
                    }
                case "1":
                    {
                        return true;
                    }
                case null:
                case "":
                default:
                    {
                        return null;
                    }
            }
        }

        #endregion
    }
}
