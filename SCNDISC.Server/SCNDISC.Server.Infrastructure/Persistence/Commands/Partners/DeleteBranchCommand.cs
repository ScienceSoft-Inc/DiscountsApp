﻿using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Commands.Partners;
using SCNDISC.Server.Infrastructure.Persistence.Providers;
using SCNDISC.Server.Infrastructure.Persistence.Queries;

namespace SCNDISC.Server.Infrastructure.Persistence.Commands.Partners
{
    public class DeleteBranchCommand : MongoCommandQueryBase<Branch>, IDeleteBranchCommand
    {
	    public DeleteBranchCommand(IMongoCollectionProvider provider)
		    : base(provider)
	    {

	    }

        public async Task DeleteBranchAsync(string branchId)
        {
	        var filter = new FilterDefinitionBuilder<Branch>().Where(b => b.Id == branchId);
	        var update = Builders<Branch>.Update.
		        Set(x=>x.IsDeleted, true).
		        Set(x => x.Modified, DateTime.UtcNow);

	        await Collection.FindOneAndUpdateAsync(filter, update);
        }
    }
}
