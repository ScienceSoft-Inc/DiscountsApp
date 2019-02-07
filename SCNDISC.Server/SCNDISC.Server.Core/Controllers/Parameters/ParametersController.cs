using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SCNDISC.Server.Application.Services.Parameters;
using SCNDISC.Server.Domain.Aggregates.Parameters;

namespace SCNDISC.Server.Core.Controllers.Parameters
{
    public class ParametersController : ControllerBase
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
