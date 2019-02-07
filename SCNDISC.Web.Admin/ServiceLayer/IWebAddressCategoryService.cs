using System.Collections.Generic;
using SCNDISC.Server.Domain.Aggregates.Partners;

namespace SCNDISC.Web.Admin.ServiceLayer
{
	public interface IWebAddressCategoryService
	{
		IEnumerable<WebAddressCategory> GetAll();
	}
}