using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using SCNDISC.Server.Application.Services.Partners;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Infrastructure.Imaging;
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
                CreateCommandQuery<ActivePartnerIdsQuery, Branch>(),
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

        // !Important!: to use this method on real DB you had better comment out the line of the code with 
        // deleting all Branches during initialization of BaseApplicationServiceTest in Initialize method,
        // set up desired db to processing in Settings
        // and then execute only the below method to do images reprocessing
        //[TestMethod]
        [Ignore]
        public async Task ImagesProcessing()
        {
            var collection = _collectionProvider.GetCollection<Branch>();
            var filter = new FilterDefinitionBuilder<Branch>().Where(b => b.IsDeleted == false);
            var imageConverter = new ImageConverter();
            var sortDefinition = new SortDefinitionBuilder<Branch>().Ascending(b => b.Modified);
            var counter = 0;

            using (var cursor = await collection.FindAsync(filter, new FindOptions<Branch>() { Sort = sortDefinition }))
            {
                using (var logWriter = System.IO.File.AppendText($"imageProcessing-{DateTime.UtcNow.ToFileTimeUtc()}.txt"))
                {
                    try
                    {
                        logWriter.WriteLine($"Image processing started at {DateTime.UtcNow}:");

                        while (await cursor.MoveNextAsync())
                        {
                            var branches = cursor.Current;
                            foreach (Branch branch in branches)
                            {
                                UpdateDefinition<Branch> updateBranch = null;
                                string processedIcon = null;
                                string processedImage = null;

                                if (branch.Id != branch.PartnerId)
                                {
                                    // branch
                                    if (!string.IsNullOrEmpty(branch.Image) || !string.IsNullOrEmpty(branch.Icon))
                                    {
                                        updateBranch = Builders<Branch>.
                                        Update.
                                        Set(x => x.Icon, null).
                                        Set(x => x.Image, null);
                                        counter++;
                                    }
                                }
                                else if (branch.Id == branch.PartnerId)
                                {
                                    // partner
                                    processedIcon = branch.Icon == null ? null : Convert.ToBase64String(imageConverter.Convert(Convert.FromBase64String(branch.Icon), ImageOptions.Icon));
                                    processedImage = branch.Image == null ? null : Convert.ToBase64String(imageConverter.Convert(Convert.FromBase64String(branch.Image), ImageOptions.Image));

                                    if (branch.Icon?.Length != processedIcon?.Length || branch.Image?.Length != processedImage?.Length)
                                    {
                                        updateBranch = Builders<Branch>.
                                        Update.
                                        Set(x => x.Modified, DateTime.UtcNow).
                                        Set(x => x.Icon, processedIcon).
                                        Set(x => x.Image, processedImage);
                                        counter++;
                                    }
                                }

                                if (updateBranch != null)
                                {
                                    var filterBranch = new FilterDefinitionBuilder<Branch>().Where(b => b.Id == branch.Id);
                                    var resultOfProcessing = await collection.UpdateOneAsync(filterBranch, updateBranch);
                                    Assert.IsTrue(resultOfProcessing.ModifiedCount == 1);

                                    logWriter.WriteLine($"____________________________________________{counter}______________________________________________");
                                    logWriter.WriteLine($"Image processing for BranchId = {branch.Id} with partnerId = {branch.PartnerId}");
                                    logWriter.WriteLine($"Icon and (or) Image were processed for branch with Id = {branch.Id}");
                                    logWriter.WriteLine($"Icon original size: {branch.Icon?.Length}, after processing: {processedIcon?.Length}");
                                    logWriter.WriteLine($"Image original size: {branch.Image?.Length}, after processing: {processedImage?.Length}");
                                    logWriter.WriteLine("____________________________________________________________________________________________________");
                                }
                            }
                        }
                        logWriter.WriteLine($"Image processing finished at {DateTime.UtcNow}");
                    }
                    catch (Exception ex)
                    {
                        logWriter.WriteLine(ex);
                        logWriter.Dispose();
                    }
                }
            }
        }
    }
}
