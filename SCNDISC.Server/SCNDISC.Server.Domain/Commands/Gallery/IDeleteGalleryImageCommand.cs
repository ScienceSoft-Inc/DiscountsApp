using System.Threading.Tasks;

namespace SCNDISC.Server.Domain.Commands.Gallery
{
    public interface IDeleteGalleryImageCommand
    {
        Task<bool> ExecuteAsync(string galleryImageId);
    }
}
