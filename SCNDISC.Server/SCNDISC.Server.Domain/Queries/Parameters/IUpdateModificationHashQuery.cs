using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using SCNDISC.Server.Domain.Aggregates.Parameters;

namespace SCNDISC.Server.Domain.Queries.Parameters
{
    public interface IUpdateModificationHashQuery
    {
        Task<Parameter> Run();
    }
}
