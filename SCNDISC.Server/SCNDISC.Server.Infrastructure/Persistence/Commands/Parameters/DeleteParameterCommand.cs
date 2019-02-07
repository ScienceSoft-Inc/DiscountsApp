using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using SCNDISC.Server.Domain.Aggregates.Parameters;
using SCNDISC.Server.Domain.Commands.Parameters;
using SCNDISC.Server.Infrastructure.Persistence.Providers;
using SCNDISC.Server.Infrastructure.Persistence.Queries;

namespace SCNDISC.Server.Infrastructure.Persistence.Commands.Parameters
{
    public class DeleteParameterCommand: MongoCommandQueryBase<Parameter>, IDeleteParameterCommand
    {
        public DeleteParameterCommand(IMongoCollectionProvider provider) : base(provider) { }

        public async Task DeleteParameterAsync(string parameterKey)
        {
            await Collection.DeleteOneAsync(b => String.Compare(b.Key, parameterKey, StringComparison.CurrentCultureIgnoreCase) == 0);
        }
    }
}
