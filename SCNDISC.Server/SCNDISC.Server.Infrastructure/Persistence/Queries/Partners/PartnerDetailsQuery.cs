using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Queries.Partners;
using SCNDISC.Server.Infrastructure.Persistence.Providers;

namespace SCNDISC.Server.Infrastructure.Persistence.Queries.Partners
{
    public class PartnerDetailsQuery : MongoCommandQueryBase<Branch>, IPartnerDetailsQuery
    {
        public PartnerDetailsQuery(IMongoCollectionProvider collectionProvider) : base(collectionProvider) { }

        public async Task<IEnumerable<Branch>> Run(string partnerId)
        {
	        return await Collection.
		        Find(b => b.PartnerId == partnerId && !b.IsDeleted).
		        ToListAsync();
        }
    }
}
