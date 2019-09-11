using ScnDiscounts.Models.Database.Tables;

namespace ScnDiscounts.Models.Data
{
    public class DiscountRatingData
    {
        public string PartnerId { get; set; }

        public int RatingCount { get; set; }

        public int RatingSum { get; set; }

        public DiscountRatingData(DiscountRating discountRating)
        {
            PartnerId = discountRating.PartnerId;
            RatingCount = discountRating.RatingCount;
            RatingSum = discountRating.RatingSum;
        }
    }
}
