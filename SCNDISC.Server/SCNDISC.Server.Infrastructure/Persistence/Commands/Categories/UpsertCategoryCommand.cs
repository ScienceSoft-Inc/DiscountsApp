using MongoDB.Bson;
using SCNDISC.Server.Domain.Aggregates.Categories;
using SCNDISC.Server.Domain.Commands.Categories;
using SCNDISC.Server.Domain.Specifications.Categories;
using SCNDISC.Server.Infrastructure.Persistence.Providers;
using SCNDISC.Server.Infrastructure.Persistence.Queries;
using System;
using System.Threading.Tasks;

namespace SCNDISC.Server.Infrastructure.Persistence.Commands.Categories
{
    public class UpsertCategoryCommand : MongoCommandQueryBase<Category>, IUpsertCategoryCommand
    {
        public UpsertCategoryCommand(IMongoCollectionProvider provider)
            : base(provider)
        {
        }

        public async Task<Category> UpsertCategoryAsync(Category category)
        {
            category.Modified = DateTime.UtcNow;

            if (string.IsNullOrEmpty(category.Id))
                category.Id = ObjectId.GenerateNewId().ToString();

            if (new CategorySpecification().IsSatisfiedBy(category))
                await Upsert(category);

            return category;
        }
    }
}
