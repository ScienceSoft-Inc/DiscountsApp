using SQLite.Net.Attributes;

namespace ScnDiscounts.Models.Database.Tables
{
    public class DiscountsStrings
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }
        [Indexed]
        public byte Appointment { get; set; }
        [Indexed]
        public int OwnerId { get; set; }
        [Indexed]
        public int LangStringId { get; set; }
    }
}
