using System;
using System.Web.UI;
using SCNDISC.Web.Admin.Controls;

namespace SCNDISC.Web.Admin
{
    public partial class LanguageBar : UserControl
    {
        public Language Language
        {
            get { return (Language) Enum.Parse(typeof (Language), uxLang.SelectedValue, true); }
        }

        public event LanguageChanged Changed;

        protected void OnLanguageChanged(object sender, EventArgs e)
        {
            var language = uxLang.SelectedValue;
            uxLang.CssClass = "x-lang-bar " + "x-lang-bar-" + language;
            if (Changed != null)
            {
                Changed(this, new LanguageChangedArgs {Language = Language});
            }
        }

        public string SelectedValue
        {
            get { return uxLang.SelectedValue; }
            set
            {
                uxLang.SelectedValue = value;
                OnLanguageChanged(this, new EventArgs());
            }
        }
    }
}