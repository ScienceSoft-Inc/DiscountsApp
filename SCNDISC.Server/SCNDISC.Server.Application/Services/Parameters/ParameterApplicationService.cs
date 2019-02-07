using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates.Parameters;
using SCNDISC.Server.Domain.Commands.Parameters;
using SCNDISC.Server.Domain.Queries.Parameters;

namespace SCNDISC.Server.Application.Services.Parameters
{
    public class ParameterApplicationService : IParameterApplicationService
    {
        private readonly IUpdateModificationHashQuery _updateModificationHashQuery;
        private readonly IModificationHashQuery _modificationHashQuery;

        public ParameterApplicationService(
            IUpdateModificationHashQuery modifyHashQuery,
            IModificationHashQuery modificationHashQuery)
        {
            _updateModificationHashQuery = modifyHashQuery;
            _modificationHashQuery = modificationHashQuery;
        }

        public async Task UpdateModificationHash()
        {
            await _updateModificationHashQuery.Run();
        }

        public async Task<Parameter> GetModificationHashAsync()
        {
            return await _modificationHashQuery.GetModificationHashAsync();
        }
    }
}
