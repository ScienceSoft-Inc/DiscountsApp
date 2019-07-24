using Android.Support.V4.App;
using Plugin.FirebasePushNotification;
using ScnDiscounts.Models.WebService;
using System.Collections.Generic;

namespace ScnDiscounts.Droid.Helpers
{
    public class PushNotificationHandler : DefaultPushNotificationHandler
    {
        public override void OnBuildNotification(NotificationCompat.Builder notificationBuilder, IDictionary<string, object> parameters)
        {
            base.OnBuildNotification(notificationBuilder, parameters);

            if (parameters.TryGetValue("documentId", out var documentId))
            {
                var url = ApiService.GetPartnerLogoUrl((string) documentId);
                var logo = BitmapHelper.GetBitmapFromUrl(url);
                if (logo != null)
                    notificationBuilder.SetLargeIcon(logo);
            }
        }
    }
}
