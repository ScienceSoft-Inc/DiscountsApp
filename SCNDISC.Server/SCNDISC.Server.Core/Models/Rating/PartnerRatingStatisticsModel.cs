using Newtonsoft.Json;

namespace SCNDISC.Server.Core.Models.Rating
{
    [JsonObject]
    public class PartnerRatingStatisticsModel
    {
        [JsonProperty(PropertyName = "ratingCount")]
        public int? RatingCount { get; set; }
        [JsonProperty(PropertyName = "ratingSum")]
        public int? RatingSum{ get; set; }
    }
}
