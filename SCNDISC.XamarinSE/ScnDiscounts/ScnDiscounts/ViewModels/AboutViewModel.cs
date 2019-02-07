using ScnDiscounts.DependencyInterface;
using ScnDiscounts.Helpers;
using ScnDiscounts.Views.ContentUI;
using ScnPage.Plugin.Forms;
using System;
using Xamarin.Forms;

namespace ScnDiscounts.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private AboutContentUI contentUI => (AboutContentUI) ContentUI;

        public new string Title => contentUI.Title;

        public void TxtLink_Click(object sender, EventArgs e)
        {
            var view = (View) sender;
            view.ClickAnimation(() =>
            {
                var args = e as TappedEventArgs;
                var tag = args?.Parameter?.ToString();
                var link = tag.NormalizeLink();
                if (!string.IsNullOrEmpty(link))
                    Functions.SafeCall(() => Device.OpenUri(new Uri(link)));
            });
        }

        public void TxtPhone_Click(object sender, EventArgs e)
        {
            var view = (View) sender;
            view.ClickAnimation(() =>
            {
                var args = e as TappedEventArgs;
                var tag = args?.Parameter?.ToString();
                var phoneNumber = tag.NormalizePhoneNumber();
                if (!string.IsNullOrEmpty(phoneNumber))
                    DependencyService.Get<IPhoneService>().DialNumber(phoneNumber);
            });
        }

        public void TxtEmail_Click(object sender, EventArgs e)
        {
            var view = (View) sender;
            view.ClickAnimation(() =>
            {
                var args = e as TappedEventArgs;
                var tag = args?.Parameter?.ToString();
                var email = tag.SafeTrim();
                if (!string.IsNullOrEmpty(email))
                    DependencyService.Get<IPhoneService>().SendEmail(email);
            });
        }
    }
}
