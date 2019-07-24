using Plugin.FirebasePushNotification;
using ScnDiscounts.DependencyInterface;
using ScnDiscounts.Models;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ScnDiscounts.Helpers
{
    public class PushNotificationHelper
    {
        protected static readonly IPushNotificationService Instance = DependencyService.Get<IPushNotificationService>();

        public static bool IsEnabled => AppParameters.Config.IsPushEnabled;

        public static async void EnablePushNotifications(bool isEnabled)
        {
            if (isEnabled)
            {
                await CrossFirebasePushNotification.Current.RegisterForPushNotifications();

                SubscribeToAll();
                SubscribeToPlatform();
                SubscribeToLang();
            }
            else
                CrossFirebasePushNotification.Current.UnregisterForPushNotifications();
        }

        public static void SubscribeToAll()
        {
            const string topicAll = "all";

            Instance.SubscribeToTopic(topicAll);
        }

        public static void SubscribeToPlatform()
        {
            const string topicPlatformPrefix = "platform_";
            var topicPlatform = $"{topicPlatformPrefix}{Device.RuntimePlatform.ToLower()}";

            Instance.SubscribeToTopic(topicPlatform);
        }

        public static void SubscribeToLang()
        {
            const string topicLangPrefix = "lang_";
            var systemLang = AppParameters.Config.SystemLang;
            var topicLang = $"{topicLangPrefix}{systemLang.LangEnumToCode().ToLower()}";

            Enum.GetValues(typeof(LanguageHelper.LangTypeEnum))
                .Cast<LanguageHelper.LangTypeEnum>()
                .Where(i => i != systemLang)
                .Select(i => $"{topicLangPrefix}{i.LangEnumToCode().ToLower()}")
                .ForEach(i => Instance.UnsubscribeFromTopic(i));

            Instance.SubscribeToTopic(topicLang);
        }
    }
}
