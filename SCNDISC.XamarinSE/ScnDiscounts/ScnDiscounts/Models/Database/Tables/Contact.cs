using System;
using SQLite.Net.Attributes;

namespace ScnDiscounts.Models.Database.Tables
{
    public class Contact
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }
        [Indexed]
        public int DiscountId { get; set; }
        [Indexed]
        public string DocumentId { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime? Modified { get; set; }
    }
}
