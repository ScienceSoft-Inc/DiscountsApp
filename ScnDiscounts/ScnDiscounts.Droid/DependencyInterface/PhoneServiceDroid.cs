using Android.Content;
using Android.Net;
using ScnDiscounts.DependencyInterface;
using ScnDiscounts.Droid.DependencyInterface;
using Xamarin.Forms;

[assembly: Dependency(typeof(PhoneServiceDroid))]

namespace ScnDiscounts.Droid.DependencyInterface
{
    class PhoneServiceDroid : IPhoneService
    {
        public void DialNumber(string number, string name = "")
        {
            var intent = new Intent(Intent.ActionCall, Uri.Parse(
                             "tel:" + System.Uri.EscapeDataString(number)));
            Forms.Context.StartActivity(intent);
        }
    }
}