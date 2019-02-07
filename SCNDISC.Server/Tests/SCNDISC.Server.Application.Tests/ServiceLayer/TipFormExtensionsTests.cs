using Microsoft.VisualStudio.TestTools.UnitTesting;
using SCNDISC.Web.Admin.ServiceLayer.Extensions;
using System.Threading.Tasks;

namespace SCNDISC.Server.Application.Tests.ServiceLayer
{
    [TestClass]
    public class TipFormExtensionsTests : BaseApplicationServiceTests
    {
        [TestMethod]
        public async Task ShouldConvertBranchToTipForm()
        {
            var branch = await CreatePartner(3, 3);
            var tipForm = branch.ToTipForm();
        }

        [TestMethod]
        public async Task ShouldConvertTipFormToBranch()
        {
            var branch = await CreatePartner(3, 3);
            var tipForm = branch.ToTipForm();
            var newBranch = tipForm.ToBranch();
        }

    }
}
