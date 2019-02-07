using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using SCNDISC.Server.Domain.Aggregates.Parameters;
using SCNDISC.Server.Domain.Queries.Parameters;
using SCNDISC.Server.Infrastructure.Persistence.Providers;
using System;

namespace SCNDISC.Server.Infrastructure.Persistence.Queries.Parameters
{
    public class ModificationHashQuery : MongoCommandQueryBase<Parameter>, IModificationHashQuery
    {
        public ModificationHashQuery(IMongoCollectionProvider collectionProvider) : base(collectionProvider) { }

        public async Task<Parameter> GetModificationHashAsync()
        {
            var parameter = await Collection.Find(b => b.Key.ToLower() == ParametersName.ModificationHash.ToLower()).SingleOrDefaultAsync();

            if (parameter == null)
                parameter = new Parameter { Key = ParametersName.ModificationHash, Value = String.Empty };

            return parameter;
        }
    }
}
