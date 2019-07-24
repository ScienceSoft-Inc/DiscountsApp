using SCNDISC.Server.Domain.Aggregates.Partners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCNDISC.Server.Application.Services.Gallery
{
    public interface IGalleryApplicationService
    {
        Task<string> AddGalleryImageToPartnerAsync(GalleryImage galleryImage);
        Task<IEnumerable<string>> GetGalleryImageIdsForPartner(string partnerId);
        Task<bool> RemoveGalleryImage(string id);
        Task<string> GetGalleryImageById(string id);
        Task<byte[]> GetGalleryImage(string id);
    }
}
