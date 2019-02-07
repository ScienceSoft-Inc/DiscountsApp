using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using SCNDISC.Server.Application.Services.Admin;
using SCNDISC.Server.Domain.Aggregates.Categories;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Infrastructure.Persistence.Commands.Partners;
using SCNDISC.Server.Infrastructure.Persistence.Queries.Parameters;
using SCNDISC.Server.Domain.Aggregates.Parameters;
using SCNDISC.Server.Infrastructure.Persistence.Commands.Categories;

namespace SCNDISC.Server.Application.Tests.Services.Admin
{
    [TestClass]
    public class AdminCommandApplicaitonServiceTests : BaseApplicationServiceTests
    {
        protected IAdminCommandApplicationService CreateService()
        {
            return new AdminCommandApplicationService(CreateCommandQuery<UpsertPartnerCommand, Branch>(),
                CreateCommandQuery<UpsertBranchCommand, Branch>(),
                CreateCommandQuery<UpsertCategoryCommand, Category>(),
                CreateCommandQuery<DeleteBranchCommand, Branch>(),
                CreateCommandQuery<DeletePartnerCommand, Branch>(),
                CreateCommandQuery<DeleteCategoryCommand, Category>(),
                CreateCommandQuery<UpdateModificationHashQuery, Parameter>());
        }

        [TestMethod]
        public async Task ShouldUpsertPartnerAsync()
        {
            var partner = await CreatePartner();
            var service = CreateService();
            var result = await service.UpsertPartnerAsync(partner);
            var upsertedPartner = await _collectionProvider.GetCollection<Branch>().Find(b => b.Id == result.Id).SingleOrDefaultAsync();
            Assert.IsTrue(upsertedPartner != null);
            AssertEquality(result, upsertedPartner);
        }

        [TestMethod]
        public async Task ShouldUpsertBranchAsync()
        {
            var id = ObjectId.GenerateNewId().ToString();
            var branch = CreateBranch(id);
            var service = CreateService();
            var result = await service.UpsertBranchAsync(branch);
            var upsertedBranch = await _collectionProvider.GetCollection<Branch>().Find(b => b.Id == result.Id).SingleOrDefaultAsync();
            Assert.IsTrue(upsertedBranch != null);
            AssertEquality(result, upsertedBranch);
        }

        [TestMethod]
        public async Task ShouldSoftDeleteBranchAsync()
        {
			var partner1 = await CreatePartner();
	        var service = CreateService();
	        var result1 = await service.UpsertPartnerAsync(partner1);
	        var branch2 = CreateBranch(result1.Id);
	        await service.UpsertBranchAsync(branch2);

	        await service.DeleteBranchAsync(branch2.Id);
            var deletedBranch = await _collectionProvider.GetCollection<Branch>().Find(b => b.Id == branch2.Id).SingleOrDefaultAsync();
            var parnter = await _collectionProvider.GetCollection<Branch>().Find(b => b.Id == result1.Id).SingleOrDefaultAsync();

	        Assert.IsNotNull(deletedBranch);
            Assert.IsTrue(deletedBranch.IsDeleted);

	        Assert.IsNotNull(parnter);
	        Assert.IsFalse(parnter.IsDeleted);
		}

        [TestMethod]
        public async Task ShouldDeletePartnerAsync()
        {
            var partner1 = await CreatePartner();
            var service = CreateService();
            var result1 = await service.UpsertPartnerAsync(partner1);
            var branch2 = CreateBranch(result1.Id);
            var branch3 = CreateBranch(result1.Id);
            var result2 = await service.UpsertBranchAsync(branch2);
            var result3 = await service.UpsertBranchAsync(branch3);
            await service.DeletePartnerAsync(result1.Id);

            var deletedPartner1 = await _collectionProvider.GetCollection<Branch>().Find(b => b.Id == result1.Id).SingleOrDefaultAsync();
            var deletedBranch2 = await _collectionProvider.GetCollection<Branch>().Find(b => b.Id == result2.Id).SingleOrDefaultAsync();
            var deletedBranch3 = await _collectionProvider.GetCollection<Branch>().Find(b => b.Id == result3.Id).SingleOrDefaultAsync();

	        Assert.IsNotNull(deletedPartner1);
            Assert.IsNotNull(deletedBranch2);
            Assert.IsNotNull(deletedBranch3);

	        Assert.IsTrue(deletedPartner1.IsDeleted);
	        Assert.IsTrue(deletedBranch2.IsDeleted);
	        Assert.IsTrue(deletedBranch3.IsDeleted);
		}
    }
}
