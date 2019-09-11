using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using SCNDISC.Server.Domain.Aggregates;
using SCNDISC.Server.Domain.Aggregates.Categories;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Infrastructure.Persistence.Mappings;
using SCNDISC.Server.Infrastructure.Persistence.Providers;
using SCNDISC.Server.Infrastructure.Persistence.Queries;
using SCNDISC.Server.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SCNDISC.Server.Application.Tests
{
    [TestClass]
    public abstract class BaseApplicationServiceTests
    {
        protected readonly IMongoCollectionProvider _collectionProvider = new MongoCollectionProvider(new SettingsInfService(), new MongoMappingsService());
        protected static readonly Random _random = new Random();

        protected TQ CreateCommandQuery<TQ, TA>() where TQ: MongoCommandQueryBase<TA> where TA : Aggregate
        {
            var ctor = (typeof(TQ)).GetConstructor(new[] {typeof (IMongoCollectionProvider)});
            if (ctor != null)
            {
                return (TQ) ctor.Invoke((new object[] {_collectionProvider}));
            }
            return null;
        }

        [TestInitialize]
        public void Initialize()
        {
            _collectionProvider.GetCollection<Branch>().DeleteManyAsync(b => true).GetAwaiter().GetResult();
        }

        protected virtual async Task<Branch> CreatePartner(int lowEntityLimit = 2, int highEntityLimit = 3)
        {
            var id = ObjectId.GenerateNewId().ToString();

            const int count = 8;
            var categories = await CreateCategoriesAsync(_collectionProvider.GetCollection<Category>(), count);

            return new Branch
            {
                Id = id,
                Modified = DateTime.UtcNow,
                Name = CreateLocalizedTextProperty(),
                Description = CreateLocalizedTextProperty(),
                CategoryIds = categories.Select(i => i.Id).Take(_random.Next(lowEntityLimit, highEntityLimit)).ToList(),
                Discounts = CreateEntities<Discount>(_random.Next(lowEntityLimit, highEntityLimit)),
                Icon = Guid.NewGuid().ToString(),
                Image = Guid.NewGuid().ToString(),
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(
                    new GeoJson2DGeographicCoordinates(_random.NextDouble(), _random.NextDouble())),
                Timetable = Guid.NewGuid().ToString(),
                Url = Guid.NewGuid().ToString(),
                Phones = CreatePhones(_random.Next(lowEntityLimit, highEntityLimit)),
                PartnerId = id,
                Address = CreateLocalizedTextProperty()
            };
        }

        protected Category CreateCategory()
        {
            var id = ObjectId.GenerateNewId().ToString();
            return new Category
            {
                Id = id,
                Modified = DateTime.UtcNow,
                Name = CreateLocalizedTextProperty(),
                Color = $"#{_random.Next(0x1000000):x6}"
            };
        }

        protected Rating CreateRating(int lowRatingMark = 0, int highRatingMark = 5)
        {
            return new Rating
            {
                Modified = DateTime.UtcNow,
                DeviceId = Guid.NewGuid().ToString(),
                PartnerId = ObjectId.GenerateNewId().ToString(),
                Mark = _random.Next(lowRatingMark, highRatingMark)
            };
        }

        protected string GetNewId()
        {
            return Guid.NewGuid().ToString();
        }
        protected async Task<IEnumerable<Category>> CreateCategoriesAsync(IMongoCollection<Category> collection, int count)
        {
            var categories = new List<Category>();
            for (var i = 0; i < count; i++)
            {
                categories.Add(CreateCategory());
            }
            await collection.InsertManyAsync(categories);
            return categories;
        }

        protected IEnumerable<Rating> CreateRatingElements(int count)
        {
            var ratings = new List<Rating>();
            for (var i = 0; i < count; i++)
            {
                ratings.Add(CreateRating());
            }
            return ratings;
        }

        private IEnumerable<Phone> CreatePhones(int i)
        {
            var result = new List<Phone>(i);
            for (var j = 0; j < i; j++)
            {
                result.Add(new Phone {Number = Guid.NewGuid().ToString()});
            }
            return result;
        }

        protected Branch CreateBranch(string partnerId, int lowEntityLimit = 2, int highEntityLimit = 3)
        {
            return new Branch
            {
                Name = CreateLocalizedTextProperty(),
				Modified = DateTime.UtcNow,
                Discounts = CreateEntities<Discount>(_random.Next(lowEntityLimit, highEntityLimit)),
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(_random.NextDouble(), _random.NextDouble())),
                Timetable = Guid.NewGuid().ToString(),
                Phones = CreatePhones(_random.Next(lowEntityLimit, highEntityLimit)),
                PartnerId = partnerId,
                Address = CreateLocalizedTextProperty()
            };
        }

        private IEnumerable<T> CreateEntities<T>(int i) where T : Entity, new()
        {
            var result = new List<T>(i);
            for (var j = 0; j < i; j++)
            {
                result.Add(CreateEntity<T>());
            }
            return result;
        }

        protected virtual T CreateEntity<T>() where T : Entity, new()
        {
            return new T
            {
                Name = CreateLocalizedTextProperty()
            };
        }

        protected virtual IEnumerable<LocalizableText> CreateLocalizedTextProperty()
        {
            return new List<LocalizableText>
            {
                CreateLocalizedText("RU"),
                CreateLocalizedText("EN")
            };
        }

        protected virtual LocalizableText CreateLocalizedText(string languageCode)
        {
            return new LocalizableText
            {
                Lan = languageCode,
                LocText = Guid.NewGuid().ToString()
            };
        }

        protected void AssertEquality(Branch comparand1, Branch comparand2)
        {
            Assert.AreEqual(comparand1.Id, comparand2.Id);
            Assert.AreEqual(comparand1.PartnerId, comparand2.PartnerId);
            Assert.AreEqual(comparand1.Timetable, comparand2.Timetable);
            Assert.AreEqual(comparand1.Url, comparand2.Url);
            Assert.IsTrue(AreLocalizableTextPropertiesEqual(comparand1.Name, comparand2.Name));
            Assert.IsTrue(AreLocalizableTextPropertiesEqual(comparand1.Description, comparand2.Description));
            Assert.AreEqual(comparand1.Location.Coordinates.Latitude, comparand2.Location.Coordinates.Latitude);
            Assert.AreEqual(comparand1.Location.Coordinates.Longitude, comparand2.Location.Coordinates.Longitude);
            Assert.IsTrue(AreEntityCollectionsEqual(comparand1.Discounts, comparand2.Discounts));
            Assert.IsTrue(AreStringCollectionsEqual(comparand1.CategoryIds, comparand2.CategoryIds));
        }

        protected bool AreLocalizableTextPropertiesEqual(IEnumerable<LocalizableText> comparand1, IEnumerable<LocalizableText> comparand2)
        {
            if (AreNotNull(comparand1, comparand2))
            {
                return comparand1.All(localizableText => comparand2.Any(c => c.Lan == localizableText.Lan && c.LocText == localizableText.LocText));
            }
            return AreNull(comparand1, comparand2);
        }

        protected bool AreStringCollectionsEqual(IEnumerable<string> comparand1, IEnumerable<string> comparand2)
        {
            if (AreNotNull(comparand1, comparand2))
            {
                return comparand1.All(c1 => comparand2.Any(c2 => c1 == c2));
            }
            return AreNull(comparand1, comparand2);
        }

        protected bool AreEntityCollectionsEqual(IEnumerable<Entity> comparand1, IEnumerable<Entity> comparand2)
        {
            if (AreNotNull(comparand1, comparand2))
            {
                return comparand1.All(c1 => comparand2.Any(c2 => AreEntitiesEqual(c1, c2)));
            }
            return AreNull(comparand1, comparand2);
        }

        private bool AreNull(object comparand1, object comparand2)
        {
            return comparand1 == null && comparand2 == null;
        }

        private bool AreNotNull(object comparand1, object comparand2)
        {
            return comparand1 != null && comparand2 != null;
        }

        protected bool AreEntitiesEqual(Entity comparand1, Entity comparand2)
        {
            if (AreNotNull(comparand1, comparand2))
            {
                return AreLocalizableTextPropertiesEqual(comparand1.Name, comparand2.Name);
            }
            return AreNull(comparand1, comparand2);
        }

        protected async Task<IEnumerable<Branch>> CreateHierachyAsync(IMongoCollection<Branch> collection, int numParents, int lowBranchLimit, int highBranchLimit, int lowEntityLimit, int highEntityLimit)
        {
            var partners = new List<Branch>();
            for (var i = 0; i < numParents; i++)
            {
                var partner = await CreatePartner(lowEntityLimit, highEntityLimit);
                partners.Add(partner);
            }
            await collection.InsertManyAsync(partners);

            var branches = new List<Branch>();
            foreach (var partner in partners)
            {
                var numBranches = _random.Next(lowBranchLimit, highBranchLimit);
                for (var i = 0; i < numBranches; i++)
                {
                    branches.Add(CreateBranch(partner.Id, lowEntityLimit, highEntityLimit));
                }
            }
            if (branches.Count > 0)
            {
                await collection.InsertManyAsync(branches);
                partners.AddRange(branches);
            }
            
            return partners;
        }
    }
}
