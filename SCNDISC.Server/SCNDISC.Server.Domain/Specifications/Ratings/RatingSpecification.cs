using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Exceptions.Ratings;

namespace SCNDISC.Server.Domain.Specifications.Ratings
{
    public class RatingSpecification
    {
        public bool IsSatisfiedBy(Rating subject)
        {
            if (subject == null)
            {
                throw new InvalidRatingException("rating must not be null");
            }

            if (subject.PartnerId == null)
            {
                throw new InvalidRatingException("Parameters: partnerId must be specified");
            }

            if (subject.DeviceId == null)
            {
                throw new InvalidRatingException("Parameters: deviceId must be specified");
            }

            if (subject.Mark < 0 || subject.Mark > 5)
            {
                throw new InvalidRatingException("Mark must be in range from 0 to 5");
            }

            return true;
        }
    }
}
