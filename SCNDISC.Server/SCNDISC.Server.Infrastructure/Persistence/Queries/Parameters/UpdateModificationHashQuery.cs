using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using SCNDISC.Server.Domain.Aggregates.Parameters;
using SCNDISC.Server.Domain.Queries.Parameters;
using SCNDISC.Server.Infrastructure.Persistence.Providers;

namespace SCNDISC.Server.Infrastructure.Persistence.Queries.Parameters
{
    public class UpdateModificationHashQuery : MongoCommandQueryBase<Parameter>, IUpdateModificationHashQuery
    {
        public UpdateModificationHashQuery(IMongoCollectionProvider collectionProvider) : base(collectionProvider) { }
        
        public async Task<Parameter> Run()
        {
            var parameter = await Collection.Find(b => b.Key.ToLower() == ParametersName.ModificationHash.ToLower()).SingleOrDefaultAsync();

            if (parameter == null)
                parameter = new Parameter { Key = ParametersName.ModificationHash, Value = String.Empty };
            parameter.Value = Guid.NewGuid().ToString();

            return await Upsert(parameter);
        }
    }
}
