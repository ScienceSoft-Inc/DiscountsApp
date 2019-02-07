using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using SCNDISC.Server.Application.Services.Discounts;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Infrastructure.Persistence.Queries.Discounts;

namespace SCNDISC.Server.Application.Tests.Services.Discounts
{
    [TestClass]
    public class DiscountApplicationServiceTests : BaseApplicationServiceTests
    {
        [TestMethod]
        public async Task ShouldGetDiscountList()
        {
	        var mongoCollection = _collectionProvider.GetCollection<Branch>();

	        await mongoCollection.DeleteManyAsync(x => true);

			var branches = await CreateHierachyAsync(mongoCollection, 3, 3, 3, 2, 2);
            var service = CreateService();
            var discounts = await service.GetPartnersDiscountListAsync();
            Assert.IsTrue(discounts.All(p => p.Discounts.Count() == (3 + 1) * 2));
            var partners = branches.Where(b => b.Id == b.PartnerId).ToArray();
            Assert.AreEqual(partners.Count(), discounts.Count());
            foreach (var partner in partners)
            {
                Assert.IsNotNull(discounts.SingleOrDefault(b => partner.Id == b.Id));
                Assert.IsNotNull(discounts.SingleOrDefault(b => AreLocalizableTextPropertiesEqual(partner.Name, b.Name) && AreLocalizableTextPropertiesEqual(partner.Description, partner.Description)));
                Assert.IsNotNull(discounts.SingleOrDefault(b => partner.Timetable.Equals(b.Timetable)));
            }
        }

	    [TestMethod]
	    public async Task ShouldGetOnlyModifiedDiscountList()
	    {
		    var mongoCollection = _collectionProvider.GetCollection<Branch>();
		    await mongoCollection.DeleteManyAsync(x => true);

		    await CreateHierachyAsync(_collectionProvider.GetCollection<Branch>(), 3, 3, 3, 2, 2);

			var temp = await CreatePartner();
		    await mongoCollection.InsertOneAsync(temp);

		    var service = CreateService();
		    var discounts = (await service.GetPartnersDiscountListAsync(temp.Modified?.AddMilliseconds(-1))).ToList();

			Assert.IsNotNull(discounts);
		    Assert.AreEqual(1, discounts.Count);
		    Assert.AreEqual(temp.Id, discounts[0].Id);
	    }

		[TestMethod]
	    public async Task ShouldGetDiscountImage()
		{
			var collection = _collectionProvider.GetCollection<Branch>();
			var partner = await CreatePartner();

			var helloWorld = "Hello world";
			var initialBuffer = Encoding.UTF8.GetBytes(helloWorld);
			partner.Image = Convert.ToBase64String(initialBuffer);
			await collection.InsertOneAsync(partner);

			var service = CreateService();
			var image = await service.GetImageByIdAsync(partner.Id);

			Assert.IsNotNull(image);
			Assert.AreEqual(initialBuffer.Length, image.Length);

			Assert.AreEqual(helloWorld, Encoding.UTF8.GetString(image));
		}

	    [TestMethod]
		public async Task ShouldGetEmptyImageIfNotExist()
	    {
		    var id = ObjectId.GenerateNewId().ToString();
			var service = CreateService();
		    var image = await service.GetImageByIdAsync(id);

		    Assert.IsNotNull(image);
			Assert.AreEqual(0, image.Length);
		}

        protected IDiscountApplicationService CreateService()
        {
            return new DiscountApplicationService(
	            CreateCommandQuery<DiscountListQuery, Branch>(),
	            CreateCommandQuery<DiscountImageQuery, Branch>()
	            );
        }
    }
}
