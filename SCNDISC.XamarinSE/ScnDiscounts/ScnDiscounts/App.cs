using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.FirebasePushNotification.Abstractions;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models;
using ScnDiscounts.Models.Data;
using ScnDiscounts.Views;
using ScnDiscounts.Views.ContentUI;
using ScnDiscounts.Views.Styles;
using ScnPage.Plugin.Forms;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;
using Application = Xamarin.Forms.Application;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace ScnDiscounts
{
    public class App : Application
    {
        private const string AppCenterSecretKey = "android=xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx;" +
                                                  "ios=zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz";

        public static MainPage RootPage => ((NavigationPage) Current.MainPage).RootPage as MainPage;

        public static double ScreenWidth { get; set; }

        public static double ScreenHeight { get; set; }

        public static bool IsMoreThan320Dpi => ScreenWidth > 320;

        public static PushNotificationData PushNotificationData { get; set; }

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

        protected override void OnStart()
        {
            base.OnStart();

            if (!Functions.IsDebug)
                AppCenter.Start(AppCenterSecretKey, typeof(Analytics), typeof(Crashes));
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

        public static async void OnNotificationOpened(object source, FirebasePushNotificationResponseEventArgs e)
        {
            if (e.Identifier != "cancel")
            {
                string title = null;
                string text = null;

                if (e.Data.ContainsKey("title") && e.Data.ContainsKey("body"))
                {
                    title = e.Data["title"]?.ToString();
                    text = e.Data["body"]?.ToString();
                }

                if (e.Data.ContainsKey("aps.alert.title") && e.Data.ContainsKey("aps.alert.body"))
                {
                    title = e.Data["aps.alert.title"]?.ToString();
                    text = e.Data["aps.alert.body"]?.ToString();
                }

                if (!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(text))
                {
                    e.Data.TryGetValue("documentId", out var id);
                    var documentId = id?.ToString();

                    PushNotificationData = new PushNotificationData
                    {
                        Title = title,
                        Text = text,
                        DocumentId = documentId
                    };

                    if (RootPage != null)
                        await ProcessPushNotification();
                }
            }
        }

        public static async Task ProcessPushNotification()
        {
            var data = PushNotificationData;
            if (data != null)
            {
                PushNotificationData = null;

                if (Current.MainPage.Navigation.NavigationStack.LastOrDefault() is BaseContentPage currentPage)
                {
                    if (!string.IsNullOrEmpty(data.DocumentId))
                        await currentPage.OpenDetailPage(data.DocumentId, true);

                    var rootContentUI = new RootContentUI();
                    await currentPage.DisplayAlert(data.Title, data.Text, rootContentUI.TxtOk);
                }
            }
        }
    }
}
