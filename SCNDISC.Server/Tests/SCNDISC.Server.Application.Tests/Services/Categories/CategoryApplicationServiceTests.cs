using Microsoft.VisualStudio.TestTools.UnitTesting;
using SCNDISC.Server.Application.Services.Categories;
using SCNDISC.Server.Domain.Aggregates.Categories;
using SCNDISC.Server.Infrastructure.Persistence.Queries.Categories;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using SCNDISC.Server.Infrastructure.Persistence.Commands.Categories;

namespace SCNDISC.Server.Application.Tests.Services.Categories
{
    [TestClass]
    public class CategoryApplicationServiceTests : BaseApplicationServiceTests
    {
        [TestMethod]
        public async Task ShouldGetCategoryList()
        {
	        var mongoCollection = _collectionProvider.GetCollection<Category>();
            await mongoCollection.DeleteManyAsync(new BsonDocument());

            const int count = 8;
			await CreateCategoriesAsync(mongoCollection, count);

            var service = CreateService();
            var categories = (await service.GetCategoryListAsync()).ToList();

			Assert.IsNotNull(categories);
		    Assert.AreEqual(count, categories.Count);

            foreach (var category in categories)
            {
                Assert.IsNotNull(categories.SingleOrDefault(b => category.Id == b.Id));
                Assert.IsNotNull(categories.SingleOrDefault(b => AreLocalizableTextPropertiesEqual(category.Name, b.Name)));
                Assert.IsNotNull(categories.SingleOrDefault(b => category.Color == b.Color));
            }
        }

	    [TestMethod]
	    public async Task ShouldGetOnlyModifiedCategoryList()
	    {
		    var mongoCollection = _collectionProvider.GetCollection<Category>();
		    await mongoCollection.DeleteManyAsync(new BsonDocument());

		    await CreateCategoriesAsync(_collectionProvider.GetCollection<Category>(), 8);

			var temp = CreateCategory();
		    await mongoCollection.InsertOneAsync(temp);

		    var service = CreateService();
		    var categories = (await service.GetCategoryListAsync(temp.Modified?.AddMilliseconds(-1))).ToList();

			Assert.IsNotNull(categories);
		    Assert.AreEqual(1, categories.Count);
		    Assert.AreEqual(temp.Id, categories[0].Id);
	    }

        protected ICategoryApplicationService CreateService()
        {
            return new CategoryApplicationService(
                CreateCommandQuery<CategoryListQuery, Category>(),
                CreateCommandQuery<UpsertCategoryCommand, Category>(),
                CreateCommandQuery<DeleteCategoryCommand, Category>()
                );
        }
    }
}
