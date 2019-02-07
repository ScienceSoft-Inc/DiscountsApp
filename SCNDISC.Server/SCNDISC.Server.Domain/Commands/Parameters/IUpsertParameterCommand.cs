using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates.Parameters;

namespace SCNDISC.Server.Domain.Commands.Parameters
{
    public interface IUpsertParameterCommand
    {
        Task<Parameter> UpsertParameterAsync(Parameter parameter);
    }
}
