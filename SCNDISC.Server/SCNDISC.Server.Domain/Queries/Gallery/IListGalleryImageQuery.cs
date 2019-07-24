using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCNDISC.Server.Domain.Queries.Gallery
{
    public interface IListGalleryImageQuery
    {
        Task<IEnumerable<string>> RunAsync(string partnerId);
    }
}
