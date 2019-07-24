using FFImageLoading.Forms.Platform;
using Foundation;
using Google.Maps;
using Plugin.FirebasePushNotification;
using Plugin.FirebasePushNotification.Abstractions;
using ScnDiscounts.DependencyInterface;
using ScnDiscounts.iOS.DependencyInterface;
using ScnDiscounts.iOS.Helpers;
using ScnDiscounts.Views.ContentUI;
using ScnViewGestures.Plugin.Forms.iOS.Renderers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UIKit;
using UserNotifications;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using CarouselViewRenderer = CarouselView.FormsPlugin.iOS.CarouselViewRenderer;

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
            CachedImageRenderer.Init();
            CarouselViewRenderer.Init();
            ViewGesturesRenderer.Init();
            Firebase.Core.App.Configure();

            MapServices.ProvideAPIKey(MapsApiKey);

            App.ScreenWidth = UIScreen.MainScreen.Bounds.Width;
            App.ScreenHeight = UIScreen.MainScreen.Bounds.Height;

            DependencyService.Register<IPhoneService, PhoneService>();
            DependencyService.Register<IImageService, ImageService>();

            LoadApplication(new App());

            InitPushNotifications(options);

            return base.FinishedLaunching(app, options);
        }

        private static void InitPushNotifications(NSDictionary options)
        {
            FirebasePushNotificationManager.CurrentNotificationPresentationOption =
                UNNotificationPresentationOptions.Alert;

            var isRegistrationNeeded = string.IsNullOrEmpty(CrossFirebasePushNotification.Current.Token);
            var notificationContentUI = new NotificationContentUI();
            FirebasePushNotificationManager.Initialize(options, new[]
            {
                new NotificationUserCategory("message_en", new List<NotificationUserAction>
                {
                    new NotificationUserAction("details", notificationContentUI.TxtDetails.EnUsValue,
                        NotificationActionType.Foreground),
                    new NotificationUserAction("cancel", notificationContentUI.TxtMarkAsRead.EnUsValue,
                        NotificationActionType.Destructive)
                }),
                new NotificationUserCategory("message_ru", new List<NotificationUserAction>
                {
                    new NotificationUserAction("details", notificationContentUI.TxtDetails.RuRuValue,
                        NotificationActionType.Foreground),
                    new NotificationUserAction("cancel", notificationContentUI.TxtMarkAsRead.RuRuValue,
                        NotificationActionType.Destructive)
                })
            }, isRegistrationNeeded);

            CrossFirebasePushNotification.Current.OnNotificationOpened += App.OnNotificationOpened;
            CrossFirebasePushNotification.Current.OnNotificationAction += App.OnNotificationOpened;
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);
            Debug.WriteLine(error);
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            if (application.ApplicationState == UIApplicationState.Inactive)
            {
                var data = PushNotificationHandler.GetParameters(userInfo);
                App.OnNotificationOpened(null, new FirebasePushNotificationResponseEventArgs(data));
                completionHandler(UIBackgroundFetchResult.NewData);
                return;
            }

            FirebasePushNotificationManager.DidReceiveMessage(userInfo);
            completionHandler(UIBackgroundFetchResult.NewData);
        }
    }
}
