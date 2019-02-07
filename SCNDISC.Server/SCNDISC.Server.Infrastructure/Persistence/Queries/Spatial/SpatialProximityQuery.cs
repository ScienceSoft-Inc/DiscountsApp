using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Queries.Spatial;
using SCNDISC.Server.Infrastructure.Persistence.Providers;

namespace SCNDISC.Server.Infrastructure.Persistence.Queries.Spatial
{
    public class SpatialProximityQuery : MongoCommandQueryBase<Branch>, ISpatialProximityQuery
    {
        public SpatialProximityQuery(IMongoCollectionProvider provider) : base(provider) { }

        public async Task<IEnumerable<Branch>> Run(double longitude, double latitude, int proximity)
        {
            var point = GeoJson.Point(GeoJson.Geographic(longitude, latitude));
            var locationFilter =  Builders<Branch>.Filter.Near(b => b.Location, point, proximity);
	        var notDeletedFilter = Builders<Branch>.Filter.Where(x => !x.IsDeleted);

	        var filter = Builders<Branch>.Filter.And(locationFilter, notDeletedFilter);

	        return await Collection.Find(filter).Project(b => new Branch
	        {
		        Id = b.Id,
		        Name = b.Name,
		        Location = b.Location,
		        PartnerId = b.PartnerId,
		        Discounts = b.Discounts,
		        Url = b.Url,
		        Address = b.Address
			}).ToListAsync();
        }
    }
}
