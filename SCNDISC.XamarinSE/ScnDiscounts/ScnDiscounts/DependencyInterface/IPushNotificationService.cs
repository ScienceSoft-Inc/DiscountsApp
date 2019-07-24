namespace ScnDiscounts.DependencyInterface
{
    public interface IPushNotificationService
    {
        void SubscribeToTopic(string topic);
        void UnsubscribeFromTopic(string topic);
    }
}
