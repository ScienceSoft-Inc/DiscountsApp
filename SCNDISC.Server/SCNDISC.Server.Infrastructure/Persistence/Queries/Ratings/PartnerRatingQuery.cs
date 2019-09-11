using MongoDB.Driver;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Queries.PartnerRating;
using SCNDISC.Server.Infrastructure.Persistence.Providers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCNDISC.Server.Infrastructure.Persistence.Queries.Ratings
{
    public class PartnerRatingQuery : MongoCommandQueryBase<Rating>, IPartnerRatingQuery
    {
        public PartnerRatingQuery(IMongoCollectionProvider collectionProvider) : base(collectionProvider) { }

        public async Task<IEnumerable<Rating>> RunAsync(string partnerId)
        {
            var filter = new FilterDefinitionBuilder<Rating>().Where(r => r.PartnerId == partnerId);
            var collection = Collection.Aggregate().Match(filter);

            return await collection.ToListAsync();
        }
    }
}
