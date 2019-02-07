using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using SCNDISC.Server.Domain.Aggregates.Parameters;
using SCNDISC.Server.Domain.Commands.Parameters;
using SCNDISC.Server.Domain.Specifications.Parameters;
using SCNDISC.Server.Infrastructure.Persistence.Providers;
using SCNDISC.Server.Infrastructure.Persistence.Queries;

namespace SCNDISC.Server.Infrastructure.Persistence.Commands.Parameters
{
    public class UpsertParameterCommand: MongoCommandQueryBase<Parameter>, IUpsertParameterCommand
    {
        public UpsertParameterCommand(IMongoCollectionProvider provider) : base(provider) { }

        public async Task<Parameter> UpsertParameterAsync(Parameter parameter)
        {
            if (new ParameterSpecification().IsSatisfiedBy(parameter))
            {
                await Upsert(parameter);
            }

            return parameter;
        }
    }
}
