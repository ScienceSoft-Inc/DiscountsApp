using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver.GeoJsonObjectModel;
using SCNDISC.Server.Application.Services.Spatial;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Infrastructure.Persistence.Queries.Spatial;

namespace SCNDISC.Server.Application.Tests.Services.Spatial
{
    [TestClass]
    public class SpatialApplicationServiceTests : BaseApplicationServiceTests
    {
        [TestMethod]
        public async Task ShouldGetSpatialDisountsData()
        {
            await CreateHierachyAsync(_collectionProvider.GetCollection<Branch>(), 3, 3, 3, 2, 2);
            var service = CreateService();
            var spatialData = await service.GetSpatialDiscountsDataAsync();
            Assert.AreEqual(3 + 3 * 3, spatialData.Count());
        }

        [TestMethod]
        public async Task ShouldGetDiscountsInProximity()
        {
            //given
            var centerPoint = new GeoJson2DGeographicCoordinates(-74.00629342, 40.71416726);
            var near50mPoint = new GeoJson2DGeographicCoordinates(-74.00566041, 40.71420792);
            var far100mPoint = new GeoJson2DGeographicCoordinates(-74.0049094, 40.71415913);
            var nearBranch = await CreatePartner(1, 1);
            nearBranch.Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(near50mPoint);
            var farBranch = await CreatePartner(1, 1);
            farBranch.Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(far100mPoint);
            await _collectionProvider.GetCollection<Branch>().InsertManyAsync(new[] {nearBranch, farBranch});

            //when
            var discountsNear75m = await CreateService().GetAllDiscountsInProximityAsync(centerPoint.Longitude, centerPoint.Latitude, 75);

            //then
            var discountNear75m = discountsNear75m.SingleOrDefault();
            Assert.IsNotNull(discountNear75m);
            Assert.AreEqual(nearBranch.Id, discountNear75m.Id);
            Assert.AreEqual(nearBranch.PartnerId, discountNear75m.PartnerId);
            Assert.AreEqual(nearBranch.Location.Coordinates.Longitude, discountNear75m.Location.Coordinates.Longitude);
            Assert.AreEqual(nearBranch.Location.Coordinates.Latitude, discountNear75m.Location.Coordinates.Latitude);
            Assert.IsTrue(AreLocalizableTextPropertiesEqual(nearBranch.Name, discountNear75m.Name));
            Assert.IsTrue(AreEntityCollectionsEqual(nearBranch.Discounts, discountNear75m.Discounts));
        }

        protected ISpatialApplicationService CreateService()
        {
            return new SpatialApplicationService(CreateCommandQuery<SpatialDiscountsQuery, Branch>(), CreateCommandQuery<SpatialProximityQuery, Branch>());
        }
    }
}
