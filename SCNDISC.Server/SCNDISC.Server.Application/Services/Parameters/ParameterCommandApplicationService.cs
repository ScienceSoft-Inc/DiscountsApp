using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates.Parameters;
using SCNDISC.Server.Domain.Commands.Parameters;

namespace SCNDISC.Server.Application.Services.Parameters
{
    public class ParameterCommandApplicationService : IParameterCommandApplicationService
    {
        private readonly IUpsertParameterCommand _upsertParameterCommand;
        private readonly IDeleteParameterCommand _deleteParameterCommand;

        public ParameterCommandApplicationService(
            IUpsertParameterCommand upsertParameterCommand, 
            IDeleteParameterCommand deleteParameterCommand)
        {
            _upsertParameterCommand = upsertParameterCommand;
            _deleteParameterCommand = deleteParameterCommand;
        }

         public async Task<Parameter> UpsertParameterAsync(Parameter parameter)
         {
             return await _upsertParameterCommand.UpsertParameterAsync(parameter);
         }

         public async Task DeleteParameterAsync(string parameterKey)
         {
             await _deleteParameterCommand.DeleteParameterAsync(parameterKey);
         }
    }
}
