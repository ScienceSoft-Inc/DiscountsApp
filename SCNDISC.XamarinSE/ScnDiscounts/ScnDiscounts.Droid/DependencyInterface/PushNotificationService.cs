using Plugin.FirebasePushNotification;
using ScnDiscounts.DependencyInterface;
using ScnDiscounts.Droid.DependencyInterface;
using Xamarin.Forms;

[assembly: Dependency(typeof(PushNotificationService))]

namespace ScnDiscounts.Droid.DependencyInterface
{
    public class PushNotificationService : IPushNotificationService
    {
        public void SubscribeToTopic(string topic)
        {
            CrossFirebasePushNotification.Current.Subscribe(topic);
        }

        public void UnsubscribeFromTopic(string topic)
        {
            CrossFirebasePushNotification.Current.Unsubscribe(topic);
        }
    }
}