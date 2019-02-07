using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates.Parameters;

namespace SCNDISC.Server.Application.Services.Parameters
{
    public interface IParameterCommandApplicationService
    {
        Task<Parameter> UpsertParameterAsync(Parameter parameter);
        Task DeleteParameterAsync(string parameterKey);
    }
}