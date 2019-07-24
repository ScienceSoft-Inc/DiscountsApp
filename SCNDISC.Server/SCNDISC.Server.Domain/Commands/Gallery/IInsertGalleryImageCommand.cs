using System.Threading.Tasks;

namespace SCNDISC.Server.Domain.Commands.Gallery
{
    public interface IInsertGalleryImageCommand
    {
        Task<Aggregates.Partners.GalleryImage> ExecuteAsync(Aggregates.Partners.GalleryImage galleryImage);
    }
}
