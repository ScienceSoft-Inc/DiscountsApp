using System;
using System.Linq;
using ScnDiscounts.Control;
using ScnDiscounts.DependencyInterface;
using ScnDiscounts.Views.ContentUI;
using Xamarin.Forms;
using ScnPage.Plugin.Forms;

namespace ScnDiscounts.ViewModels
{
    class AboutViewModel : BaseViewModel
    {
        private AboutContentUI contentUI
        {
            get { return (AboutContentUI)ContentUI; }
        }

        public void txtLink_Click(object sender, EventArgs e)
        {
            LabelExtended label = sender as LabelExtended;
            if (!String.IsNullOrWhiteSpace(label.Text))
                Device.OpenUri(new Uri(label.Text));
        }

        public void txtPhone_Click(object sender, EventArgs e)
        {
            LabelExtended label = sender as LabelExtended;
            string phoneNumber = label.Text.ToLower();

            int index = phoneNumber.IndexOfAny("0123456789".ToCharArray());
            if (index > 0)
                phoneNumber = phoneNumber.Remove(0, index - 1);
            phoneNumber = phoneNumber.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");
            if (!String.IsNullOrWhiteSpace(phoneNumber))
                DependencyService.Get<IPhoneService>().DialNumber(phoneNumber, "ScienceSoft");
        }
    }
}
