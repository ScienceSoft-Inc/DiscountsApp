using System;

namespace SCNDISC.Server.Domain.Aggregates.Partners
{
    public class GalleryImage : Aggregate
    {
        public string Image { get; set; }
        public string PartnerId { get; set; }
        public string FileName { get; set; }
        public DateTime? Created { get; set; }
    }
}
