using SQLite;

namespace ScnDiscounts.Models.Database.Tables
{
    public class DiscountRating
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }
        [Indexed]
        public string PartnerId { get; set; }
        public int RatingCount { get; set; }
        public int RatingSum { get; set; }
    }
}
