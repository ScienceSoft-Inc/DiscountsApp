using MongoDB.Driver.GeoJsonObjectModel;
using System;
using System.Collections.Generic;

namespace SCNDISC.Server.Domain.Aggregates.Partners
{
    public class Branch : Aggregate
    {
        public IEnumerable<LocalizableText> Description { get; set; }
        public IEnumerable<LocalizableText> Address { get; set; }
        public string PartnerId { get; set; }
        public GeoJsonPoint<GeoJson2DGeographicCoordinates> Location { get; set; }
        public string Timetable { get; set; }
        public IEnumerable<Discount> Discounts { get; set; }
        public IEnumerable<string> CategoryIds { get; set; }
        public IEnumerable<Phone> Phones { get; set; }
        public IEnumerable<WebAddress> WebAddresses { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Image { get; set; }
	    public string Comment { get; set; }

		public bool IsDeleted { get; set; }
		public DateTime? Modified { get; set; }
    }

	public class WebAddress : Aggregate
	{
		public string Url { get; set; }
		public string Category { get; set; }
	}
}
