using Newtonsoft.Json;

namespace SCNDISC.Server.Core.Models.Notifications
{
    [JsonObject]
    public class PushNotifyData
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }

        [JsonProperty(PropertyName = "documentId")]
        public string DocumentId { get; set; }

        [JsonProperty(PropertyName = "click_action")]
        public string ClickAction { get; set; }
    }
}
