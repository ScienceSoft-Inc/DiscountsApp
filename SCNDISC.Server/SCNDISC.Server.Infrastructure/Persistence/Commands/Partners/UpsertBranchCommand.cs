using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Commands.Partners;
using SCNDISC.Server.Domain.Specifications.Partners;
using SCNDISC.Server.Infrastructure.Persistence.Providers;
using SCNDISC.Server.Infrastructure.Persistence.Queries;

namespace SCNDISC.Server.Infrastructure.Persistence.Commands.Partners
{
    public class UpsertBranchCommand: MongoCommandQueryBase<Branch>,  IUpsertBranchCommand
    {
        public UpsertBranchCommand(IMongoCollectionProvider provider) : base(provider) { }

        public async Task<Branch> UpsertBranchAsync(Branch branch)
        {
	        branch.Modified = DateTime.UtcNow;
            branch.Icon = null;// set null to fields in order to store images and
            branch.Image = null;// icons only per partner document, not in branches

            if (String.IsNullOrEmpty(branch.Id))
            {
                branch.Id = ObjectId.GenerateNewId().ToString();
            }
            if (new BranchSpecification().IsSatisfiedBy(branch))
            {
                await Upsert(branch);
            }
            return branch;
        }
    }
}
