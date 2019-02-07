using System.Collections.Generic;
using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Queries.Partners;

namespace SCNDISC.Web.Admin.ServiceLayer
{
	public class WebAddressCategoryService : IWebAddressCategoryService
	{
		private readonly IWebAddressCategoryQuery _categoryQuery;

		public WebAddressCategoryService(IWebAddressCategoryQuery categoryQuery)
		{
			_categoryQuery = categoryQuery;
		}

		public IEnumerable<WebAddressCategory> GetAll()
		{
			return Task.Factory.StartNew(() => _categoryQuery.Run())
				.Unwrap()
				.GetAwaiter()
				.GetResult();
		}
	}
}