using MongoDB.Driver;
using SCNDISC.Server.Domain.Aggregates.Categories;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Commands.Categories;
using SCNDISC.Server.Infrastructure.Persistence.Providers;
using SCNDISC.Server.Infrastructure.Persistence.Queries;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SCNDISC.Server.Infrastructure.Persistence.Commands.Categories
{
    public class DeleteCategoryCommand : MongoCommandQueryBase<Category>, IDeleteCategoryCommand
	{
		public DeleteCategoryCommand(IMongoCollectionProvider provider)
			: base(provider)
		{
        }

		public async Task DeleteCategoryAsync(string categoryId)
		{
		    var filterBranch = new FilterDefinitionBuilder<Branch>().Where(b => b.CategoryIds != null && b.CategoryIds.Contains(categoryId));
		    var updateBranch = Builders<Branch>.
		        Update.
		        Pull(x => x.CategoryIds, categoryId).
		        Set(x => x.Modified, DateTime.UtcNow);

            await Provider.GetCollection<Branch>().UpdateOneAsync(filterBranch, updateBranch);

		    var filter = new FilterDefinitionBuilder<Category>().Where(b => b.Id == categoryId);
		    var update = Builders<Category>.
		        Update.
		        Set(x => x.IsDeleted, true).
		        Set(x => x.Modified, DateTime.UtcNow);

		    await Collection.UpdateManyAsync(filter, update);
        }
	}
}
