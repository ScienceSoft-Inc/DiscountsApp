using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using FFImageLoading.Forms.Platform;
using Plugin.FirebasePushNotification;
using Plugin.Permissions;
using ScnDiscounts.DependencyInterface;
using ScnDiscounts.Droid.DependencyInterface;
using ScnViewGestures.Plugin.Forms.Droid.Renderers;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using CarouselViewRenderer = CarouselView.FormsPlugin.Android.CarouselViewRenderer;

namespace ScnDiscounts.Droid
{
    [Activity(Label = "Discounts", Theme = "@style/MyTheme", LaunchMode = LaunchMode.SingleTask,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            FormsMaps.Init(this, bundle);
            CachedImageRenderer.Init(true);
            CarouselViewRenderer.Init();
            ViewGesturesRenderer.Init();

            AndroidBug5497Workaround.AssistActivity(this);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                Window.DecorView.SystemUiVisibility =
                    (StatusBarVisibility) (SystemUiFlags.LayoutStable | SystemUiFlags.LayoutFullscreen);
            }

            App.ScreenWidth = this.FromPixels(Resources.DisplayMetrics.WidthPixels);
            App.ScreenHeight = this.FromPixels(Resources.DisplayMetrics.HeightPixels);

            DependencyService.Register<IPhoneService, PhoneService>();
            DependencyService.Register<IImageService, ImageService>();

            LoadApplication(new App());

            FirebasePushNotificationManager.ProcessIntent(this, Intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            FirebasePushNotificationManager.ProcessIntent(this, intent);
        }
    }
}
