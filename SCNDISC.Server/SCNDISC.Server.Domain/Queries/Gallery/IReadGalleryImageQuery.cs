using System.Threading.Tasks;

namespace SCNDISC.Server.Domain.Queries.Gallery
{
    public interface IReadGalleryImageQuery
    {
        Task<string> RunAsync(string id);
    }
}
