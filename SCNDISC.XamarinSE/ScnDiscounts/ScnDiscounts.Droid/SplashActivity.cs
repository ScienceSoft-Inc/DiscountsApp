using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;

namespace ScnDiscounts.Droid
{
    [Activity(Label = "Discounts", Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            StartActivity(new Intent(Application.Context, typeof(MainActivity)));

            Finish();
        }
    }
}
