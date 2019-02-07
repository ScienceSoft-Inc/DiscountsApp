using System;
using System.Web.UI;
using SCNDISC.Web.Admin.ServiceLayer;

namespace SCNDISC.Web.Admin.Controls
{
    public partial class Contact : UserControl
    {
        public TipForm Value
        {
            set
            {
                if (value == null)
                {
                    Visible = false;
                }
                else
                {
                    Visible = true;
                    uxFormAddress_RU.Text = value.Address_RU;
                    uxFormAddress_EN.Text = value.Address_EN;
                    uxFormPhone1.Text = value.Phone1;
                    uxFormPhone2.Text = value.Phone2;
                    uxFormPoint.Text = value.Point;
                }
            }
            get
            {
                return new TipForm
                {
                    Phone1 = uxFormPhone1.Text,
                    Phone2 = uxFormPhone2.Text,
                    Address_RU = uxFormAddress_RU.Text,
                    Address_EN = uxFormAddress_EN.Text,
                    Point = uxFormPoint.Text
                };
            }
        }

        public void Clear()
        {
            uxFormAddress_RU.Text = String.Empty;
            uxFormAddress_EN.Text = String.Empty;
            uxFormPhone1.Text = String.Empty;
            uxFormPhone2.Text = String.Empty;
            uxFormPoint.Text = String.Empty;
        }

        public void SwitchLanguage(Language language)
        {
            uxFormAddress_RU.Visible = false;
            uxFormAddress_EN.Visible = false;

            if (language == Language.Ru)
            {
                uxFormAddress_RU.Visible = true;
            }

            if (language == Language.En)
            {
                uxFormAddress_EN.Visible = true;
            }
        }
    }
}