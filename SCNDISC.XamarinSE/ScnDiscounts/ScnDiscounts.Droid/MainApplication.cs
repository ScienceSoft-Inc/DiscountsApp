using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Plugin.CurrentActivity;
using Plugin.FirebasePushNotification;
using Plugin.FirebasePushNotification.Abstractions;
using ScnDiscounts.Droid.Helpers;
using ScnDiscounts.Views.ContentUI;
using System;
using System.Collections.Generic;

namespace ScnDiscounts.Droid
{
#if DEBUG
    [Application(Debuggable = true)]
#else
    [Application(Debuggable = false)]
#endif
    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          :base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            CrossCurrentActivity.Current.Init(this);

            InitPushNotifications();
        }

        private void InitPushNotifications()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                FirebasePushNotificationManager.DefaultNotificationChannelId = "FirebasePushNotificationChannel";
                FirebasePushNotificationManager.DefaultNotificationChannelName = "General";
            }

            var notificationColor = ContextCompat.GetColor(this, Resource.Color.notification);
            FirebasePushNotificationManager.Color = new Color(notificationColor);
            FirebasePushNotificationManager.IconResource = Resource.Drawable.ic_notification;

            FirebasePushNotificationManager.Initialize(this, new PushNotificationHandler(), false, true, false);

            var notificationContentUI = new NotificationContentUI();
            FirebasePushNotificationManager.RegisterUserNotificationCategories(new[]
            {
                new NotificationUserCategory("message_en", new List<NotificationUserAction>
                {
                    new NotificationUserAction("details", notificationContentUI.TxtDetails.EnUsValue,
                        NotificationActionType.Foreground),
                    new NotificationUserAction("cancel", notificationContentUI.TxtMarkAsRead.EnUsValue)
                }),
                new NotificationUserCategory("message_ru", new List<NotificationUserAction>
                {
                    new NotificationUserAction("details", notificationContentUI.TxtDetails.RuRuValue,
                        NotificationActionType.Foreground),
                    new NotificationUserAction("cancel", notificationContentUI.TxtMarkAsRead.RuRuValue)
                })
            });

            CrossFirebasePushNotification.Current.OnNotificationOpened += App.OnNotificationOpened;
            CrossFirebasePushNotification.Current.OnNotificationAction += App.OnNotificationOpened;
        }
    }
}