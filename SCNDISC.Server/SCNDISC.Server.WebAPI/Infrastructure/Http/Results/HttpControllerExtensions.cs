using System.Web.Http;
using System.Web.Http.Controllers;

namespace SCNDISC.Server.WebAPI.Infrastructure.Http.Results
{
	public static class HttpControllerExtensions
	{
		public static IHttpActionResult File(this IHttpController controller, byte[] buffer)
		{
			var result = new FileResult(buffer);
			return result;
		}
	}
}