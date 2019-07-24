using MongoDB.Driver;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Queries.Discounts;
using SCNDISC.Server.Infrastructure.Persistence.Providers;
using System.Threading.Tasks;

namespace SCNDISC.Server.Infrastructure.Persistence.Queries.Discounts
{
    public class DiscountImageQuery : MongoCommandQueryBase<Branch>, IDiscountImageQuery
	{
		public DiscountImageQuery(IMongoCollectionProvider provider) 
			: base(provider)
		{
		}

        public async Task<string> GetImageBase64Async(string id)
        {
            var filter = new FilterDefinitionBuilder<Branch>().Eq(x => x.Id, id);

            var image = await Collection.Find(filter).Project(p => p.Image).SingleOrDefaultAsync();
            return image ?? string.Empty;
        }

        public async Task<string> GetLogoBase64Async(string id)
        {
            var filter = new FilterDefinitionBuilder<Branch>().Eq(x => x.Id, id);

            var image = await Collection.Find(filter).Project(p => p.Icon).SingleOrDefaultAsync();
            return image ?? string.Empty;
        }
    }
}