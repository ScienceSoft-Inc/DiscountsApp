using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace SCNDISC.Server.Core.Models.Notifications
{
    [JsonObject]
    public class PushNotificationModel
    {
        [JsonProperty(PropertyName = "title")]
        [Required]
        public string Title { get; set; }

        [Required]
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "documentId")]
        public string DocumentId { get; set; }
        [JsonProperty(PropertyName = "isSendToAllDevices")]
        public bool IsSendToAllDevices { get; set; }

        [JsonProperty(PropertyName = "created")]
        public DateTime Created { get; set; }
    }
}
