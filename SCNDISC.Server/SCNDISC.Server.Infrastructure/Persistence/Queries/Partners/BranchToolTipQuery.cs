using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Queries.Partners;
using SCNDISC.Server.Infrastructure.Persistence.Providers;

namespace SCNDISC.Server.Infrastructure.Persistence.Queries.Partners
{
    public class BranchToolTipQuery : MongoCommandQueryBase<Branch>, IBranchToolTipQuery
    {
        public BranchToolTipQuery(IMongoCollectionProvider provider) : base(provider) { }

        public async Task<Branch> Run(string partnerId, string branchId)
        {
            var res = await Collection.Find(b => b.PartnerId == partnerId && !b.IsDeleted).ToListAsync();
            return res.GroupBy(b => b.PartnerId).Select(g => new Branch()
            {
                PartnerId = g.Key,
                Name = g.First(x => x.Id == partnerId).Name,
                Description = g.First(x => x.Id == branchId).Name,
                Discounts = g.First(x => x.Id == branchId).Discounts,
                Address = g.First(x => x.Id == branchId).Address,
                Url = g.First(x => x.Id == partnerId).Url,
                Id = branchId
            }).FirstOrDefault();
        }
    }
}
