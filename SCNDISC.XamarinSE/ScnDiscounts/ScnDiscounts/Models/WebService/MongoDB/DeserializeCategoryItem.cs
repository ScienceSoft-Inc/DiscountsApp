using System;
using System.Collections.Generic;

namespace ScnDiscounts.Models.WebService.MongoDB
{
    public class DeserializeCategoryItem
    {
        public string Id { get; set; }
        public List<DeserializeLocText> Name { get; set; }
        public string Color { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? Modified { get; set; }
    }
}
