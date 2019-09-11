using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using SCNDISC.Server.Application.Services.PartnerRating;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Infrastructure.Persistence.Commands.PartnerRating;
using SCNDISC.Server.Infrastructure.Persistence.Queries.Ratings;
using System.Linq;
using System.Threading.Tasks;

namespace SCNDISC.Server.Application.Tests.Services.Ratings
{
    [TestClass]
    public class PartnerRatingApplicationServiceTests : BaseApplicationServiceTests
    {
        [TestMethod]
        public async Task ShouldGetRatingList()
        {
            var mongoCollection = _collectionProvider.GetCollection<Rating>();
            await mongoCollection.DeleteManyAsync(new BsonDocument());

            const int count = 8;
            var ratings = CreateRatingElements(count);

            var service = CreateService();

            foreach (var rElement in ratings)
            {
                var savedRatingElement = await service.UpsertRatingAsync(rElement);
                Assert.IsNotNull(savedRatingElement.Id);
            }

            var ratingElemntsInDB = await mongoCollection.Find<Rating>(new BsonDocument()).ToListAsync();
            Assert.AreEqual(count, ratingElemntsInDB.Count);
        }

        [TestMethod]
        public async Task CheckUpsertingRatingAsyncToCreate()
        {
            var mongoCollection = _collectionProvider.GetCollection<Rating>();
            await mongoCollection.DeleteManyAsync(new BsonDocument());

            var count = 3;
            var service = CreateService();

            for (var i = 0; i < count; i++)
            {
                var newRating = CreateRating();
                var result = await service.UpsertRatingAsync(newRating);
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Id);
            }

            var ratingElemntsInDB = await mongoCollection.Find(new BsonDocument()).ToListAsync();
            Assert.AreEqual(count, ratingElemntsInDB.Count);
        }

        [TestMethod]
        public async Task CheckUpsertingRatingAsyncToUpdate()
        {
            var mongoCollection = _collectionProvider.GetCollection<Rating>();
            await mongoCollection.DeleteManyAsync(new BsonDocument());

            var count = 3;
            var service = CreateService();

            for (var i = 0; i < count; i++)
            {
                var newRating = CreateRating();
                var rating = await service.UpsertRatingAsync(newRating);
                var newMark = _random.Next(0, 5);
                rating.Mark = newMark;

                var ratingWithUpdatedMark = await service.UpsertRatingAsync(rating);
                Assert.IsNotNull(ratingWithUpdatedMark);
                Assert.AreEqual(newMark, ratingWithUpdatedMark.Mark);
            }

            var ratingElemntsInDB = await mongoCollection.Find(new BsonDocument()).ToListAsync();
            Assert.AreEqual(count, ratingElemntsInDB.Count);
        }

        [TestMethod]
        public async Task CheckGetPartnerRating()
        {
            var mongoCollection = _collectionProvider.GetCollection<Rating>();
            await mongoCollection.DeleteManyAsync(new BsonDocument());

            var partners = await CreateHierachyAsync(_collectionProvider.GetCollection<Branch>(), 1, 1, 1, 1, 1);
            var partnerWithRatings = partners.FirstOrDefault();

            var count = 3;
            var ratingSum = 0;

            var service = CreateService();

            for (var i = 0; i < count; i++)
            {
                var newRating = CreateRating();
                newRating.PartnerId = partnerWithRatings.PartnerId;
                var rating = await service.UpsertRatingAsync(newRating);
                ratingSum += rating.Mark;
            }

            var checkResult = await service.GetPartnerRatingAsync(partnerWithRatings.PartnerId);
            Assert.AreNotEqual(null, checkResult);
            Assert.AreEqual(count, checkResult.Count());
            Assert.AreEqual(ratingSum, checkResult.Select(r => r.Mark).Sum());
        }

        [TestMethod]
        public async Task CheckGetRatingsByDeviceId()
        {
            var count = 3;
            var mongoCollection = _collectionProvider.GetCollection<Rating>();
            await mongoCollection.DeleteManyAsync(new BsonDocument());
            var partners = (await CreateHierachyAsync(_collectionProvider.GetCollection<Branch>(), count, 0, 0, 1, 1)).ToArray();
            var deviceId = GetNewId();

            var service = CreateService();
            Assert.AreEqual(count, partners?.Count());

            for (var i = 0; i < count; i++)
            {
                var newRating = CreateRating();
                newRating.PartnerId = partners[i].PartnerId;
                newRating.DeviceId = deviceId;

                await service.UpsertRatingAsync(newRating);
            }

            var ratingsBydevice = await service.GetRatingsByDeviceIdAsync(deviceId);
            Assert.AreNotEqual(null, ratingsBydevice);
            Assert.AreEqual(count, ratingsBydevice.Count());
        }

        protected IPartnerRatingService CreateService()
        {
            return new PartnerRatingService(
                CreateCommandQuery<PartnerRatingQuery, Rating>(),
                CreateCommandQuery<RatingByIdQuery, Rating>(),
                CreateCommandQuery<DeviceRatingsQuery, Rating>(),
                CreateCommandQuery<UpsertRatingCommand, Rating>()
                );
        }
    }
}