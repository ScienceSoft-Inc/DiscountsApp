using SCNDISC.Server.Core.Models.Notifications;
using System;

namespace SCNDISC.Server.Core.Mapper
{
    public static class NotificationMapper
    {
        public static PushNotificationAndroidRequest ToAndroidNotification(this PushNotificationModel notifyModel, string topicLanguage, string clickAction)
        {
            const string androidTopic = "platform_android";
            var now = DateTime.UtcNow;
            var id = now.Second + now.Minute * 100 + now.Hour * 10000 + now.Day * 1000000 + now.Month * 100000000;

            return new PushNotificationAndroidRequest
            {
                Condition = $"'{androidTopic}' in topics && '{topicLanguage}' in topics",
                Data = new PushNotifyData
                {
                    Id = id,
                    Title = notifyModel.Title,
                    Body = notifyModel.Text,
                    DocumentId = notifyModel.DocumentId,
                    ClickAction = clickAction
                }
            };
        }

        public static PushNotificationIosRequest ToIosNotification(this PushNotificationModel notifyModel, string topicLanguage, string clickAction)
        {
            const string iosTopic = "platform_ios";
            return new PushNotificationIosRequest
            {
                Condition = $"'{iosTopic}' in topics && '{topicLanguage}' in topics",
                Data = new PushNotifyData
                {
                    DocumentId = notifyModel.DocumentId,
                },
                Notification = new IosNotification
                {
                    Title = notifyModel.Title,
                    Body = notifyModel.Text,
                    ClickAction = clickAction,
                    // other fields initialized by default in the model
                }
            };
        }

        public static Domain.Aggregates.Notification ToNotification(this PushNotificationModel model, string language)
        {
            return new Domain.Aggregates.Notification
            {
                Title = model.Title,
                Text = model.Text,
                DocumentId = model.DocumentId,
                Created = DateTime.UtcNow,
                Language = language,
                IsSentToAllDevices = model.IsSendToAllDevices
            };
        }
    }
}
