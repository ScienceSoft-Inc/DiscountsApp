using ScnDiscounts.Models.Database.Tables;

namespace ScnDiscounts.Models.Data
{
    public class PersonalRatingData
    {
        public string DocumentId { get; set; }

        public string DeviceId { get; set; }

        public string PartnerId { get; set; }

        public int Mark { get; set; }

        public PersonalRatingData()
        {
        }

        public PersonalRatingData(PersonalRating personalRating)
            : this()
        {
            DocumentId = personalRating.DocumentId;
            DeviceId = personalRating.DeviceId;
            PartnerId = personalRating.PartnerId;
            Mark = personalRating.Mark;
        }
    }
}
