using SCNDISC.Server.Infrastructure.Persistence.Queries;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Commands.Ratings;
using SCNDISC.Server.Infrastructure.Persistence.Providers;
using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using SCNDISC.Server.Domain.Specifications.Ratings;

namespace SCNDISC.Server.Infrastructure.Persistence.Commands.PartnerRating
{
    public class UpsertRatingCommand : MongoCommandQueryBase<Rating>, IUpsertRatingCommand
    {
        public UpsertRatingCommand(IMongoCollectionProvider provider)
            : base(provider)
        {
        }

        public async Task<Rating> UpsertRatingAsync(Rating rating)
        {
            Rating result = null;
            rating.Modified = DateTime.UtcNow;

            if (string.IsNullOrEmpty(rating.Id))
                rating.Id = ObjectId.GenerateNewId().ToString();

            if (new RatingSpecification().IsSatisfiedBy(rating))
                result = await Upsert(rating);

            return result;
        }
    }
}
