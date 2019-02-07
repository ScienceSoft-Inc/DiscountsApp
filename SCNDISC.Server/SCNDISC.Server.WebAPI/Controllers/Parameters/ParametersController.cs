using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using SCNDISC.Server.Application.Services.Parameters;
using SCNDISC.Server.Domain.Aggregates.Parameters;

namespace SCNDISC.Server.WebAPI.Controllers.Parameters
{
    public class ParametersController : ApiController
    {
        private readonly IParameterApplicationService _parameterApplicationService;

        public ParametersController(IParameterApplicationService parameterApplicationService)
        {
            _parameterApplicationService = parameterApplicationService;
        }

        [Route("modificationhash/")]
        [HttpGet]
        public async Task<Parameter> GetModificationHashAsync()
        {
            return await _parameterApplicationService.GetModificationHashAsync();
        }
    }
}
