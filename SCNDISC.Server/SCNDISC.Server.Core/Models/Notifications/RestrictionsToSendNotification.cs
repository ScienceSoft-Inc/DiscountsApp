using Newtonsoft.Json;

namespace SCNDISC.Server.Core.Models.Notifications
{
    [JsonObject]
    public class RestrictionToSendNotification
    {
        [JsonProperty(PropertyName = "secondsToCountdown")]
        public int SecondsToCountDown { get; set; }

        [JsonProperty(PropertyName = "restrictions")]
        public string []Restrictions { get; set; }
    }
}
