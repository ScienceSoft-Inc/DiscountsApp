using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace SCNDISC.Server.Core.Models.Rating
{
    [JsonObject]
    public class PartnerRatingModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [Required]
        [JsonProperty(PropertyName = "deviceId")]
        public string DeviceId { get; set; }
        [Required]
        [MinLength(24)]
        [MaxLength(24)]
        [JsonProperty(PropertyName = "partnerId")]
        public string PartnerId { get; set; }
        [JsonProperty(PropertyName = "modified")]
        public DateTime? Modified { get; set; }
        [Required]
        [Range(0, 5)]
        [JsonProperty(PropertyName = "mark")]
        public int Mark { get; set; }
    }
}
