using System;
using UIKit;
using Foundation;
using Xamarin.Forms;
using ScnDiscounts.DependencyInterface;
using ScnDiscounts.iOS.DependencyInterface;

[assembly: Dependency(typeof(PhoneService))]

namespace ScnDiscounts.iOS.DependencyInterface
{
    public class PhoneService : IPhoneService
    {
        public void DialNumber(string number, string name = "")
        {
            try
            {
                NSUrl url = new NSUrl(string.Format(@"telprompt://{0}", number));
                UIApplication.SharedApplication.OpenUrl(url);

                /*if (!UIApplication.SharedApplication.OpenUrl(new NSUrl("tel:" + number)))
                {
                    var av = new UIAlertView("Not supported",
                        "Scheme 'tel:' is not supported on this device",
                        null,
                        "OK",
                        null);
                    av.Show();
                };*/
            }
            catch (Exception ex)
            { }
        }
    }
}