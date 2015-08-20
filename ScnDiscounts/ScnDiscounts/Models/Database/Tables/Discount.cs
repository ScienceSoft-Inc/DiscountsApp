using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScnDiscounts.Models.Database.Tables
{
    public class Discount
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string DocumentId { get; set; }
        public string PercentValue { get; set; }
        public string UrlAddress { get; set; }
        public string LogoFileName { get; set; }
        public string ImageFileName { get; set; }

        public bool IsFullDescription { get; set; }
    }
}
