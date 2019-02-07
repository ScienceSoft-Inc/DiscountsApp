using System.Collections.Generic;
using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates.Parameters;

namespace SCNDISC.Server.Application.Services.Parameters
{
    public interface IParameterApplicationService
    {
        Task UpdateModificationHash();
        Task<Parameter> GetModificationHashAsync();
    }
}
