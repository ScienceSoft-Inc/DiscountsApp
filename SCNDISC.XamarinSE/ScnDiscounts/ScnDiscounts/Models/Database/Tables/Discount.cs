using SQLite;
using System;

namespace ScnDiscounts.Models.Database.Tables
{
    public class Discount
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }
        [Indexed]
        public string DocumentId { get; set; }
        public string PercentValue { get; set; }
        public int? DiscountType { get; set; }
        public string LogoFileName { get; set; }
        public string ImageFileName { get; set; }
        public DateTime? Modified { get; set; }
    }
}
