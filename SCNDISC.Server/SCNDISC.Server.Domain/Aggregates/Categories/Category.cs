using System;

namespace SCNDISC.Server.Domain.Aggregates.Categories
{
    public class Category : Aggregate
    {
        public string Color { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? Modified { get; set; }
    }
}
