using Firebase.CloudMessaging;
using ScnDiscounts.DependencyInterface;
using ScnDiscounts.iOS.DependencyInterface;
using Xamarin.Forms;

[assembly: Dependency(typeof(PushNotificationService))]

namespace ScnDiscounts.iOS.DependencyInterface
{
    public class PushNotificationService : IPushNotificationService
    {
        public void SubscribeToTopic(string topic)
        {
            Messaging.SharedInstance.Subscribe(topic);
        }

        public void UnsubscribeFromTopic(string topic)
        {
            Messaging.SharedInstance.Unsubscribe(topic);
        }
    }
}