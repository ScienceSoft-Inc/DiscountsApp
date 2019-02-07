using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Queries.Partners;

namespace SCNDISC.Server.Infrastructure.Persistence.Queries.Partners
{
	public class WebAddressCategoryQuery : IWebAddressCategoryQuery
	{
		public Task<IEnumerable<WebAddressCategory>> Run()
		{
			var items = new List<WebAddressCategory>
			{
				new WebAddressCategory{Id = 0, Name = "HTTP Url"},
				new WebAddressCategory{Id = 1, Name = "VK"},
				new WebAddressCategory{Id = 2, Name = "Facebook"},
				new WebAddressCategory{Id = 3, Name = "Instagram"},
				new WebAddressCategory{Id = 4, Name = "Youtube"},
				new WebAddressCategory{Id = 5, Name = "Google+"},
				new WebAddressCategory{Id = 6, Name = "Odnoklassniki"},
				new WebAddressCategory{Id = 7, Name = "Twitter"},
			};

			return Task.FromResult(items.AsEnumerable());
		}
	}
}