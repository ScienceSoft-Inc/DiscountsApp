using System.Web.Http;
//using System.Web.Http.Controllers;

using Microsoft.AspNetCore.Mvc;

namespace SCNDISC.Server.Core.Infrastructure.Http.Results
{
	public static class HttpControllerExtensions
	{
        public static ActionResult File(this ControllerBase controller, byte[] buffer)
        {
            var result = new FileResult(buffer);
            return result;
        }
    }
}