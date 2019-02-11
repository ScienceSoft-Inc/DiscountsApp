using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models;
using ScnDiscounts.Views;
using ScnDiscounts.Views.Styles;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;
using Application = Xamarin.Forms.Application;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace ScnDiscounts
{
    public class App : Application
    {
        public static MainPage RootPage => (MainPage) ((NavigationPage) Current.MainPage).RootPage;

        public static double ScreenWidth { get; set; }

        public static double ScreenHeight { get; set; }

        public static bool IsMoreThan320Dpi => ScreenWidth > 320;

        public App()
        {
            var mainStyles = new MainStyles();
            mainStyles.Load();

            var labelStyles = new LabelStyles();
            labelStyles.Load();

            On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);

            MainPage = new NavigationPage(new SplashPage())
            {
                BackgroundColor = (Color) Current.Resources[MainStyles.MainBackgroundColor]
            };
        }

        protected override void OnResume()
        {
            base.OnResume();

            AppMobileService.Locaion.StartListening();
        }

        protected override void OnSleep()
        {
            base.OnSleep();

            AppMobileService.Locaion.StopListening();
        }
    }
}
