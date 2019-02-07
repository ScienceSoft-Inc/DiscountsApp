using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Queries.Spatial;
using SCNDISC.Server.Infrastructure.Persistence.Providers;

namespace SCNDISC.Server.Infrastructure.Persistence.Queries.Spatial
{
    public class SpatialDiscountsQuery : MongoCommandQueryBase<Branch>, ISpatialDiscountsQuery
    {
        public SpatialDiscountsQuery(IMongoCollectionProvider collectionProvider) : base(collectionProvider) { }

        public async Task<IEnumerable<Branch>> Run(DateTime? syncdate = null)
        {
	        IFindFluent<Branch, Branch> filteredCollection;
	        if (syncdate != null)
	        {
		        syncdate = syncdate.Value.Kind == DateTimeKind.Utc ? syncdate : syncdate.Value.ToUniversalTime();
		        filteredCollection = Collection.Find(b => b.Modified > syncdate);
			}
	        else
	        {
		        filteredCollection = Collection.Find(b => true);
	        }

	        return await filteredCollection.Project(b => new Branch
	        {
		        Id = b.Id,
		        Name = b.Name,
		        Location = b.Location,
		        PartnerId = b.PartnerId,
		        Address = b.Address,
		        Phones = b.Phones,
				Modified = b.Modified,
				IsDeleted = b.IsDeleted
	        }).ToListAsync();
        }
    }
}