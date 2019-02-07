using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SCNDISC.Server.Application.Services.Partners;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Infrastructure.Persistence.Commands.Partners;
using SCNDISC.Server.Infrastructure.Persistence.Queries.Partners;

namespace SCNDISC.Server.Application.Tests.Services.Partners
{
    [TestClass]
    public class PartnerApplicationServiceTests : BaseApplicationServiceTests
    {
        [TestMethod]
        public async Task ShouldGetPartnerDetailsAsync()
        {
            var branches = await CreateHierachyAsync(_collectionProvider.GetCollection<Branch>(), 3, 3, 3, 2, 2);
            var partner = branches.First(b => b.Id == b.PartnerId);
            var service = CreateService();
            var partnerDetails = await service.GetPartnerDetailsAsync(partner.Id);
            Assert.AreEqual(4, partnerDetails.Count());
            Assert.IsTrue(partnerDetails.All(b => b.PartnerId == partner.Id));
        }

        [TestMethod]
        public async Task ShouldGetBranchToolTipAsync()
        {
            var branches = await CreateHierachyAsync(_collectionProvider.GetCollection<Branch>(), 3, 3, 3, 2, 2);
            var branch = branches.First(b => b.Id != b.PartnerId);
            var partner = branches.First(b => b.Id == branch.PartnerId);
            var service = CreateService();
            var toolTip = await service.GetBranchToolTipAsync(branch.PartnerId, branch.Id);
            Assert.AreEqual(branch.Id, toolTip.Id);
            Assert.AreEqual(branch.PartnerId, toolTip.PartnerId);
            Assert.IsTrue(AreEntityCollectionsEqual(branch.Discounts, toolTip.Discounts));
            Assert.IsTrue(AreLocalizableTextPropertiesEqual(partner.Name, toolTip.Name));
            Assert.IsTrue(AreLocalizableTextPropertiesEqual(branch.Name, toolTip.Description));
        }

        protected IPartnerApplicationService CreateService()
        {
            return new PartnerApplicationService(
	            CreateCommandQuery<PartnerDetailsQuery, Branch>(), 
	            CreateCommandQuery<BranchToolTipQuery, Branch>(),
	            CreateCommandQuery<UpsertBranchCommand, Branch>(),
	            CreateCommandQuery<DeleteBranchCommand, Branch>(),
	            CreateCommandQuery<UpsertPartnerCommand, Branch>(),
	            CreateCommandQuery<DeletePartnerCommand, Branch>()
	            );
        }

        //[TestMethod]
        [Ignore]
        public async Task GenerateDb()
        {
            await CreateHierachyAsync(_collectionProvider.GetCollection<Branch>(), 1, 1, 1, 1, 1);
        }
    }
}
