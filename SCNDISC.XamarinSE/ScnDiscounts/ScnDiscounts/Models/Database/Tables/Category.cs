using SQLite.Net.Attributes;
using System;

namespace ScnDiscounts.Models.Database.Tables
{
    public class Category
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }
        [Indexed]
        public string DocumentId { get; set; }
        public string Color { get; set; }
        public DateTime? Modified { get; set; }
    }
}
