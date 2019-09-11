using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Queries.PartnerRating;
using SCNDISC.Server.Infrastructure.Persistence.Providers;

namespace SCNDISC.Server.Infrastructure.Persistence.Queries.Ratings
{
    public class DeviceRatingsQuery : MongoCommandQueryBase<Rating>, IDeviceRatingsQuery
    {
        public DeviceRatingsQuery(IMongoCollectionProvider collectionProvider) : base(collectionProvider) { }

        public async Task<IEnumerable<Rating>> RunAsync(string deviceId)
        {
            var filter = new FilterDefinitionBuilder<Rating>().Where(r => r.DeviceId == deviceId);
            var collection = Collection.Aggregate().Match(filter);

            return await collection.ToListAsync();
        }
    }
}
