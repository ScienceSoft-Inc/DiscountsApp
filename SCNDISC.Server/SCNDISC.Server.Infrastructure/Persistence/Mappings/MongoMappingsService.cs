using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using SCNDISC.Server.Domain.Aggregates;
using SCNDISC.Server.Domain.Aggregates.Categories;
using SCNDISC.Server.Domain.Aggregates.Parameters;
using SCNDISC.Server.Domain.Aggregates.Partners;

namespace SCNDISC.Server.Infrastructure.Persistence.Mappings
{
	public class MongoMappingsService : IMongoMappingsService
	{
		public void RegisterClassMaps()
		{
			if (!BsonClassMap.IsClassMapRegistered(typeof(Entity)))
			{
				BsonClassMap.RegisterClassMap<Entity>(cm =>
				{
					cm.SetIsRootClass(true);
					cm.MapProperty(e => e.Name);
					cm.AddKnownType(typeof(Aggregate));
					cm.AddKnownType(typeof(Branch));
					cm.AddKnownType(typeof(Category));
					cm.AddKnownType(typeof(Parameter));
					cm.AddKnownType(typeof(Feedback));
                    cm.AddKnownType(typeof(GalleryImage));
                    cm.AddKnownType(typeof(Rating));
                });
			}

			if (!BsonClassMap.IsClassMapRegistered(typeof(LocalizableText)))
			{
				BsonClassMap.RegisterClassMap<LocalizableText>();
			}

			if (!BsonClassMap.IsClassMapRegistered(typeof(Discount)))
			{
				BsonClassMap.RegisterClassMap<Discount>();
			}

			if (!BsonClassMap.IsClassMapRegistered(typeof(Phone)))
			{
				BsonClassMap.RegisterClassMap<Phone>();
			}

			if (!BsonClassMap.IsClassMapRegistered(typeof(WebAddress)))
			{
				BsonClassMap.RegisterClassMap<WebAddress>();
			}

			if (!BsonClassMap.IsClassMapRegistered(typeof(Feedback)))
			{
				BsonClassMap.RegisterClassMap<Feedback>();
			}

			if (!BsonClassMap.IsClassMapRegistered(typeof(Aggregate)))
			{
				BsonClassMap.RegisterClassMap<Aggregate>(cm =>
				{
					cm.MapIdMember(a => a.Id);
					cm.IdMemberMap.SetIdGenerator(new StringObjectIdGenerator())
						.SetSerializer(new StringSerializer(BsonType.ObjectId));
					cm.AddKnownType(typeof(Branch));
					cm.AddKnownType(typeof(Category));
					cm.AddKnownType(typeof(Parameter));
				});
			}

			if (!BsonClassMap.IsClassMapRegistered(typeof(Branch)))
			{
				BsonClassMap.RegisterClassMap<Branch>(cm =>
				{
					cm.MapProperty(b => b.Description);
					cm.MapProperty(b => b.Address);
					cm.MapProperty(b => b.Discounts);
					cm.MapProperty(b => b.Icon);
					cm.MapProperty(b => b.Image);
					cm.MapProperty(b => b.Location);
					cm.MapProperty(b => b.Url);
					cm.MapMember(b => b.PartnerId).SetSerializer(new StringSerializer(BsonType.ObjectId));
					cm.MapProperty(b => b.Phones);
					cm.MapProperty(b => b.WebAddresses);
					cm.MapProperty(b => b.Timetable);
					cm.MapProperty(b => b.CategoryIds);
					cm.MapProperty(b => b.IsDeleted);
					cm.MapProperty(b => b.Modified);
					cm.MapProperty(b => b.Comment);
				});
			}

			if (!BsonClassMap.IsClassMapRegistered(typeof(Category)))
			{
				BsonClassMap.RegisterClassMap<Category>(cm =>
				{
					cm.MapProperty(b => b.Color);
					cm.MapProperty(b => b.IsDeleted);
					cm.MapProperty(b => b.Modified);
				});
			}

            if (!BsonClassMap.IsClassMapRegistered(typeof(GalleryImage)))
            {
                BsonClassMap.RegisterClassMap<GalleryImage>(cm =>
                {
                    cm.MapProperty(gi => gi.Image);
                    cm.MapProperty(gi => gi.FileName);
                    cm.MapProperty(gi => gi.Created);
                    cm.MapProperty(gi => gi.PartnerId).SetSerializer(new StringSerializer(BsonType.ObjectId));
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Rating)))
            {
                BsonClassMap.RegisterClassMap<Rating>(cm =>
                {
                    cm.MapProperty(r => r.DeviceId);
                    cm.MapProperty(r => r.PartnerId).SetSerializer(new StringSerializer(BsonType.ObjectId));
                    cm.MapProperty(r => r.Mark);
                    cm.MapProperty(r => r.Modified);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Parameter)))
			{
				BsonClassMap.RegisterClassMap<Parameter>(cm =>
				{
					cm.MapProperty(b => b.Key);
					cm.MapProperty(b => b.Value);
				});
			}
		}
	}
}
