using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SCNDISC.Server.Application.Services.Admin;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Queries.Partners;
using SCNDISC.Server.Infrastructure.Persistence.Queries.Partners;

namespace SCNDISC.Server.Application.Tests.Services.Admin
{
    [TestClass]
    public class AdminQueryApplicationServiceTests : BaseApplicationServiceTests
    {
        protected IAdminQueryApplicationService CreateService()
        {
            return new AdminQueryApplicationService(CreateCommandQuery<PartnersOnlyQuery, Branch>());
        }

        [TestMethod]
        public async Task ShouldGetPartnersOnlyAsync()
        {
            await CreateHierachyAsync(_collectionProvider.GetCollection<Branch>(), 3, 1, 2, 1, 2);
            var service = CreateService();
            var partners = await service.GetPartnersOnly();
            Assert.AreEqual(3, partners.Count());
            Assert.IsTrue(partners.All(b => b.PartnerId == b.Id));

        }
    }
}
