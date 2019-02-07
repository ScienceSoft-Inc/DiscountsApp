using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Commands.Partners;
using SCNDISC.Server.Domain.Specifications.Partners;
using SCNDISC.Server.Infrastructure.Persistence.Providers;
using SCNDISC.Server.Infrastructure.Persistence.Queries;

namespace SCNDISC.Server.Infrastructure.Persistence.Commands.Partners
{
    public class UpsertPartnerCommand : MongoCommandQueryBase<Branch>,  IUpsertPartnerCommand
    {
        public UpsertPartnerCommand(IMongoCollectionProvider provider) : base(provider) { }

        public async Task<Branch> UpsertPartnerAsync(Branch partner)
        {
	        partner.Modified = DateTime.UtcNow;

	        if (partner.WebAddresses != null && partner.WebAddresses.Any())
	        {
		        foreach (var webAddress in partner.WebAddresses)
		        {
			        if (string.IsNullOrEmpty(webAddress.Id))
			        {
				        webAddress.Id = ObjectId.GenerateNewId().ToString();
			        }
		        }
			}

			if (String.IsNullOrEmpty(partner.Id))
            {
                partner.Id = ObjectId.GenerateNewId().ToString();
                partner.PartnerId = partner.Id;
            }
            if (new PartnerSpecification().IsSatisfiedBy(partner))
            {
                await Upsert(partner);
            }
            return partner;
        }
    }
}
