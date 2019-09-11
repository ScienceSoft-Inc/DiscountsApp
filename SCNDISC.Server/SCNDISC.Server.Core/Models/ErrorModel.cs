using Newtonsoft.Json;
namespace SCNDISC.Server.Core.Models
{
    [JsonObject]
    public class ErrorModel
    {
        [JsonProperty(PropertyName = "Error")]
        public string Error { get; set; }
    }
}
