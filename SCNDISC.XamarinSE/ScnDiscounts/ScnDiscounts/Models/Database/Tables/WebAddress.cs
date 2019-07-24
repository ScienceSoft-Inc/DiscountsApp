using SQLite;

namespace ScnDiscounts.Models.Database.Tables
{
    public class WebAddress
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }
        [Indexed]
        public int DiscountId { get; set; }
        public int Category { get; set; }
        public string Url { get; set; }
    }
}
