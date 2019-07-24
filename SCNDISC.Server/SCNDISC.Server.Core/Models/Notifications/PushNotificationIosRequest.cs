using Newtonsoft.Json;

namespace SCNDISC.Server.Core.Models.Notifications
{
    [JsonObject]
    public class PushNotificationIosRequest
    {
        [JsonProperty(PropertyName = "data")]
        public PushNotifyData Data { get; set; }
        [JsonProperty(PropertyName = "condition")]
        public string Condition { get; set; }
        [JsonProperty(PropertyName = "notification")]
        public IosNotification Notification { get; set; }
    }
}
