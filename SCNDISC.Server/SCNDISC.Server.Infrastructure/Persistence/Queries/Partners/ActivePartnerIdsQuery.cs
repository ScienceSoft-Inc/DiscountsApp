using MongoDB.Driver;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Queries.Partners;
using SCNDISC.Server.Infrastructure.Persistence.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCNDISC.Server.Infrastructure.Persistence.Queries.Partners
{
    public class ActivePartnerIdsQuery : MongoCommandQueryBase<Branch>, IActivePartnerIdsQuery
    {
        public ActivePartnerIdsQuery(IMongoCollectionProvider provider) : base(provider) { }

        public async Task<IList<string>> RunAsync()
        {
            var filter = new FilterDefinitionBuilder<Branch>().Where(x => !x.IsDeleted);

            var partnerIds = await Collection.
            Aggregate().
            Match(filter).
            Group(x => x.PartnerId, g => new { g.Key }).ToListAsync();

            return partnerIds?.Select(p => p.Key).ToList();
        }
    }
}
