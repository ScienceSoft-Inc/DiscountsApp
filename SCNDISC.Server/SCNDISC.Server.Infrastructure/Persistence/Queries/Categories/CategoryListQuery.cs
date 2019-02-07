using MongoDB.Driver;
using SCNDISC.Server.Domain.Aggregates.Categories;
using SCNDISC.Server.Domain.Queries.Categories;
using SCNDISC.Server.Infrastructure.Persistence.Providers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCNDISC.Server.Infrastructure.Persistence.Queries.Categories
{
    public class CategoryListQuery : MongoCommandQueryBase<Category>, ICategoryListQuery
	{
		public CategoryListQuery(IMongoCollectionProvider collectionProvider) : base(collectionProvider) { }

		public async Task<IEnumerable<Category>> GetCategoryListAsync(DateTime? syncdate = null)
		{
			var collection = Collection.Aggregate();

			if (syncdate != null)
			{
				syncdate = syncdate.Value.Kind == DateTimeKind.Utc ? syncdate : syncdate.Value.ToUniversalTime();
				var filter = new FilterDefinitionBuilder<Category>().Where(x => x.Modified > syncdate);
				collection = collection.Match(filter);
			}

		    return await collection.ToListAsync();
		}
	}
}
