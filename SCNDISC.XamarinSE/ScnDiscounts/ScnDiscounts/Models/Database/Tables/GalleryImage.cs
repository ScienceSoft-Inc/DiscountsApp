using SQLite;

namespace ScnDiscounts.Models.Database.Tables
{
    public class GalleryImage
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }
        [Indexed]
        public int DiscountId { get; set; }
        public string FileName { get; set; }
    }
}
