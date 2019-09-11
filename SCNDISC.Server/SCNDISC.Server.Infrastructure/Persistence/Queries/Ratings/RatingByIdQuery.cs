using System.Threading.Tasks;
using MongoDB.Driver;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Queries.PartnerRating;
using SCNDISC.Server.Infrastructure.Persistence.Providers;

namespace SCNDISC.Server.Infrastructure.Persistence.Queries.Ratings
{
    public class RatingByIdQuery: MongoCommandQueryBase<Rating>, IRatingByIdQuery
    {
        public RatingByIdQuery(IMongoCollectionProvider collectionProvider) : base(collectionProvider) { }

        public async Task<Rating> RunAsync(string id)
        {
            var filter = new FilterDefinitionBuilder<Rating>().Eq(r => r.Id, id);
            var collection = Collection.Aggregate().Match(filter);

            return await collection.SingleOrDefaultAsync();
        }
    }
}
