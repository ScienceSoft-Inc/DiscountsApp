using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCNDISC.Server.Core.Models.Notifications
{
    [JsonObject]
    public class IosNotification
    {
        [JsonProperty(PropertyName = "content_available")]
        public bool ContentAvailable => true;
        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "click_action")]
        public string ClickAction { get; set; }
        [JsonProperty(PropertyName = "mutable_content")]
        public bool MutableContent => true;
        [JsonProperty(PropertyName = "sound")]
        public string Sound => "default";
    }
}
