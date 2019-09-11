using System;

namespace ScnDiscounts.Models.WebService.MongoDB
{
    public class DeserializePersonalRating
    {
        public string Id { get; set; }
        public string DeviceId { get; set; }
        public string PartnerId { get; set; }
        public int Mark { get; set; }
        public DateTime? Modified { get; set; }
    }
}
