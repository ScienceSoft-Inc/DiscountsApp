using FFImageLoading.Forms.Platform;
using Foundation;
using Google.Maps;
using ScnDiscounts.DependencyInterface;
using ScnDiscounts.iOS.DependencyInterface;
using ScnViewGestures.Plugin.Forms.iOS.Renderers;
using UIKit;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace ScnDiscounts.iOS
{
    [Register(nameof(AppDelegate))]
    public class AppDelegate : FormsApplicationDelegate
    {
        private const string MapsApiKey = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();
            FormsMaps.Init();
            ViewGesturesRenderer.Init();
            CachedImageRenderer.Init();

            MapServices.ProvideAPIKey(MapsApiKey);

            App.ScreenWidth = UIScreen.MainScreen.Bounds.Width;
            App.ScreenHeight = UIScreen.MainScreen.Bounds.Height;

            DependencyService.Register<IPhoneService, PhoneService>();
            DependencyService.Register<IImageService, ImageService>();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
