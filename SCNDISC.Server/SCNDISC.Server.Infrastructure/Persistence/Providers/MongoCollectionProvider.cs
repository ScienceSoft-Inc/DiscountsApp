using System;
using MongoDB.Driver;
using SCNDISC.Server.Domain.Aggregates;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Infrastructure.Persistence.Mappings;
using SCNDISC.Server.Infrastructure.Settings;
using SCNDISC.Server.Domain.Aggregates.Parameters;

namespace SCNDISC.Server.Infrastructure.Persistence.Providers
{
    //singleton lifestyle
    public class MongoCollectionProvider : IMongoCollectionProvider
    {
	    private readonly ISettingsInfService _settingsService;
        private readonly IMongoClient _client;

        public MongoCollectionProvider(ISettingsInfService settingsService, IMongoMappingsService mongoMappingsService)
        {
	        _settingsService = settingsService;
	        mongoMappingsService.RegisterClassMaps();
            _client = new MongoClient(settingsService.MongoConnectionString);
            var partnerIndexBuilder = new IndexKeysDefinitionBuilder<Branch>().Ascending(b => b.PartnerId);
            var spatialIndexBuilder = new IndexKeysDefinitionBuilder<Branch>().Geo2DSphere(x => x.Location);
            GetCollection<Branch>().Indexes.CreateOneAsync(new CreateIndexModel<Branch>(partnerIndexBuilder)).GetAwaiter().GetResult();
            GetCollection<Branch>().Indexes.CreateOneAsync(new CreateIndexModel<Branch>(spatialIndexBuilder)).GetAwaiter().GetResult();
            var parameterIndexBuilder = new IndexKeysDefinitionBuilder<Parameter>().Ascending(b => b.Key);
            GetCollection<Parameter>().Indexes.CreateOneAsync(new CreateIndexModel<Parameter>(parameterIndexBuilder)).GetAwaiter().GetResult();
            var combinedRatingIndexBuilder = new IndexKeysDefinitionBuilder<Rating>().Ascending(r => r.DeviceId).Ascending(r => r.PartnerId);
            GetCollection<Rating>().Indexes.CreateOneAsync(new CreateIndexModel<Rating>(combinedRatingIndexBuilder, new CreateIndexOptions { Unique = true })).GetAwaiter().GetResult();
        }

        public IMongoCollection<TAggregate> GetCollection<TAggregate>() where TAggregate : Aggregate
        {
            return _client.GetDatabase(_settingsService.DatabaseName).GetCollection<TAggregate>(GetCollectionNameFromType(typeof(TAggregate)));
        }

        protected static string GetCollectionNameFromType(Type aggregateType)
        {
            if (typeof(Aggregate).IsAssignableFrom(aggregateType))
            {
                while (!aggregateType.BaseType.Equals(typeof(Aggregate)))
                {
                    aggregateType = aggregateType.BaseType;
                }
            }
            else
            {
                throw new ArgumentException("Must be Aggregate type", "aggregateType");
            }
            return aggregateType.Name;
        }
    }
}
