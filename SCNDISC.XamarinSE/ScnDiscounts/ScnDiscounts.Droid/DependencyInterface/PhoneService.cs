using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Support.V4.App;
using Plugin.CurrentActivity;
using ScnDiscounts.DependencyInterface;
using ScnDiscounts.Droid.DependencyInterface;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Uri = Android.Net.Uri;

[assembly: Dependency(typeof(PhoneService))]

namespace ScnDiscounts.Droid.DependencyInterface
{
    public class PhoneService : IPhoneService
    {
        public Thickness SafeAreaInsets
        {
            get
            {
                Thickness result = 0;

                var context = CrossCurrentActivity.Current.Activity;

                if (Build.VERSION.SdkInt >= BuildVersionCodes.P)
                {
                    var insets = context.Window.DecorView.RootWindowInsets;
                    if (insets.DisplayCutout != null)
                    {
                        result = new Thickness(
                            context.FromPixels(insets.DisplayCutout.SafeInsetLeft),
                            context.FromPixels(insets.DisplayCutout.SafeInsetTop),
                            context.FromPixels(insets.DisplayCutout.SafeInsetRight),
                            context.FromPixels(insets.DisplayCutout.SafeInsetBottom));
                    }
                    else if (insets.HasStableInsets)
                        result = new Thickness(0, context.FromPixels(insets.StableInsetTop), 0, 0);
                }
                else if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                {
                    var insets = context.Window.DecorView.RootWindowInsets;
                    if (insets.HasStableInsets)
                        result = new Thickness(0, context.FromPixels(insets.StableInsetTop), 0, 0);
                }
                else if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
                {
                    var resourceId = context.Resources.GetIdentifier("status_bar_height", "dimen", "android");
                    if (resourceId > 0)
                    {
                        var statusBarHeight = context.Resources.GetDimensionPixelSize(resourceId);
                        result = new Thickness(0, context.FromPixels(statusBarHeight), 0, 0);
                    }
                }

                return result;
            }
        }

        public bool HasSafeAreaSupport => Build.VERSION.SdkInt >= BuildVersionCodes.P;

        public void DialNumber(string number)
        {
            var uri = Uri.Parse($"tel:{number}");
            var intent = new Intent(Intent.ActionDial, uri);

            var packageManager = CrossCurrentActivity.Current.AppContext.PackageManager;
            if (intent.ResolveActivity(packageManager) != null)
                CrossCurrentActivity.Current.Activity.StartActivity(intent);
        }

        public void SendEmail(string toEmail, string subject = null, string text = null)
        {
            var url = Uri.Parse("mailto:");
            var intent = new Intent(Intent.ActionSendto, url);

            if (!string.IsNullOrEmpty(toEmail))
                intent.PutExtra(Intent.ExtraEmail,
                    toEmail.Split(new[] {',', ';', ' '}, StringSplitOptions.RemoveEmptyEntries));

            if (!string.IsNullOrEmpty(subject))
                intent.PutExtra(Intent.ExtraSubject, subject);

            if (!string.IsNullOrEmpty(text))
                intent.PutExtra(Intent.ExtraText, text);

            var packageManager = CrossCurrentActivity.Current.AppContext.PackageManager;
            if (intent.ResolveActivity(packageManager) != null)
                CrossCurrentActivity.Current.Activity.StartActivity(intent);
        }

        public void OpenGpsSettings()
        {
            var intent = new Intent(Settings.ActionLocationSourceSettings);
            CrossCurrentActivity.Current.Activity.StartActivity(intent);
        }

        public void LaunchApp(string appId, string appUrl, string marketUrl, string webUrl)
        {
            Intent intent;

            try
            {
                var packageManager = CrossCurrentActivity.Current.AppContext.PackageManager;
                intent = packageManager.GetLaunchIntentForPackage(appId) ??
                         new Intent(Intent.ActionView, Uri.Parse(marketUrl));

                intent.AddFlags(ActivityFlags.NewTask);
                CrossCurrentActivity.Current.Activity.StartActivity(intent);
            }
            catch (Exception)
            {
                intent = new Intent(Intent.ActionView, Uri.Parse(webUrl));

                intent.AddFlags(ActivityFlags.NewTask);
                CrossCurrentActivity.Current.Activity.StartActivity(intent);
            }
        }

        public bool CheckNotificationPermission()
        {
            var notificationManager = NotificationManagerCompat.From(CrossCurrentActivity.Current.Activity);
            return notificationManager.AreNotificationsEnabled();
        }
    }
}