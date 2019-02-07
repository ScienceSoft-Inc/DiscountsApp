using System.Collections.Generic;
using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates.Partners;

namespace SCNDISC.Server.Domain.Queries.Partners
{
	public interface IWebAddressCategoryQuery
	{
		Task<IEnumerable<WebAddressCategory>> Run();
	}
}