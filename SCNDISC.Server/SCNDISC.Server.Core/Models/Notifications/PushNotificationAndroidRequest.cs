using Newtonsoft.Json;

namespace SCNDISC.Server.Core.Models.Notifications
{
    [JsonObject]
    public class PushNotificationAndroidRequest
    {
        [JsonProperty(PropertyName = "data")]
        public PushNotifyData Data { get; set; }
        [JsonProperty(PropertyName = "condition")]
        public string Condition { get; set; } 
    }
}
