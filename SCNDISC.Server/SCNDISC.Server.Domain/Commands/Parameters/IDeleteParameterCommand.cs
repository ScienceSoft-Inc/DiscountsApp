using System.Threading.Tasks;

namespace SCNDISC.Server.Domain.Commands.Parameters
{
    public interface IDeleteParameterCommand
    {
        Task DeleteParameterAsync(string parameterKey);
    }
}
