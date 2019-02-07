using Foundation;
using MessageUI;
using ScnDiscounts.DependencyInterface;
using ScnDiscounts.iOS.DependencyInterface;
using System;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(PhoneService))]

namespace ScnDiscounts.iOS.DependencyInterface
{
    public class PhoneService : IPhoneService
    {
        private static double PixelsToDp(double value)
        {
            return value / UIScreen.MainScreen.Scale;
        }

        public Thickness SafeAreaInsets
        {
            get
            {
                Thickness result;

                if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                {
                    var insets = UIApplication.SharedApplication.KeyWindow.SafeAreaInsets;

                    result = new Thickness(
                        PixelsToDp(insets.Left),
                        PixelsToDp(insets.Top),
                        PixelsToDp(insets.Right),
                        PixelsToDp(insets.Bottom));
                }
                else
                {
                    var statusBarHeight = UIApplication.SharedApplication.StatusBarFrame.Height;
                    result = new Thickness(0, statusBarHeight, 0, 0);
                }

                return result;
            }
        }

        public bool HasSafeAreaSupport => UIDevice.CurrentDevice.CheckSystemVersion(11, 0);

        public void DialNumber(string number)
        {
            var url = new NSUrl($"tel:{number}");
            UIApplication.SharedApplication.OpenUrl(url);
        }

        public void SendEmail(string toEmail, string subject = null, string text = null)
        {
            if (MFMailComposeViewController.CanSendMail)
            {
                var mailController = new MFMailComposeViewController();

                if (!string.IsNullOrEmpty(toEmail))
                    mailController.SetToRecipients(toEmail.Split(new[] {',', ';', ' '},
                        StringSplitOptions.RemoveEmptyEntries));

                if (!string.IsNullOrEmpty(subject))
                    mailController.SetSubject(subject);

                if (!string.IsNullOrEmpty(text))
                    mailController.SetMessageBody(text, false);

                mailController.Finished += (sender, args) =>
                {
                    var controller = (MFMailComposeViewController) sender;
                    controller.InvokeOnMainThread(() => controller.DismissViewController(true, null));
                };

                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(mailController, true,
                    null);
            }
        }

        public void OpenGpsSettings()
        {
            var url = new NSUrl(UIApplication.OpenSettingsUrlString);
            UIApplication.SharedApplication.OpenUrl(url);
        }
    }
}