using System;

namespace SCNDISC.Server.Domain.Aggregates.Partners
{
    public class Rating: Aggregate
    {
        public string DeviceId { get; set; }
        public string PartnerId { get; set; }
        public int Mark { get; set; }
        public DateTime? Modified { get; set; }
    }
}
