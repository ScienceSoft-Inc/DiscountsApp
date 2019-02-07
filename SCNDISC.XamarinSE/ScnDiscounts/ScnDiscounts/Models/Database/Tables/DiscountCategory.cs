using SQLite.Net.Attributes;

namespace ScnDiscounts.Models.Database.Tables
{
    public class DiscountCategory
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }
        [Indexed]
        public int DiscountId { get; set; }
        [Indexed]
        public string CategoryId { get; set; }
    }
}
