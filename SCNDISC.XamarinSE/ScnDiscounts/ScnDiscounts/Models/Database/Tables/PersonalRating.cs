using SQLite;
using System;

namespace ScnDiscounts.Models.Database.Tables
{
    public class PersonalRating
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }
        [Indexed]
        public string DocumentId { get; set; }
        public string DeviceId { get; set; }
        public string PartnerId { get; set; }
        public int Mark { get; set; }
        public DateTime? Modified { get; set; }
    }
}
