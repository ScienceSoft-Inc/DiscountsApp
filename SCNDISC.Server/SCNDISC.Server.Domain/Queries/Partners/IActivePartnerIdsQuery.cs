
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCNDISC.Server.Domain.Queries.Partners
{
    public interface IActivePartnerIdsQuery
    {
        Task<IList<string>> RunAsync();
    }
}
