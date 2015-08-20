using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScnDiscounts.Models.Database.Tables
{
    public class Contact
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int DiscountId { get; set; }
        public string DocumentId { get; set; }

        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
