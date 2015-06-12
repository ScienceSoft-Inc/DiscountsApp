using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace ScnDiscounts.Droid
{
    [Activity(Label = "Discounts",
        Icon = "@drawable/ic_logo",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Portrait
        )]
    public class MainActivity : FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            FormsMaps.Init(this, bundle);

            Forms.SetTitleBarVisibility(AndroidTitleBarVisibility.Never);
            LoadApplication(new App());
        }
    }
}

