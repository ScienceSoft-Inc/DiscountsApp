using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Commands.Gallery;
using SCNDISC.Server.Domain.Queries.Gallery;

namespace SCNDISC.Server.Application.Services.Gallery
{
    public class GalleryApplicationService : IGalleryApplicationService
    {
        private readonly IListGalleryImageQuery _listGalleryImageQuery;
        private readonly IReadGalleryImageQuery _readGalleryImageQuery;
        private readonly IInsertGalleryImageCommand _insertGalleryImageCommand;
        private readonly IDeleteGalleryImageCommand _deleteGalleryImageCommand;

        public GalleryApplicationService(IListGalleryImageQuery listGalleryImageQuery, IInsertGalleryImageCommand insertGalleryImageCommand,
            IDeleteGalleryImageCommand deleteGalleryImageCommand, IReadGalleryImageQuery readGalleryImageQuery)
        {
            _readGalleryImageQuery = readGalleryImageQuery;
            _listGalleryImageQuery = listGalleryImageQuery;
            _insertGalleryImageCommand = insertGalleryImageCommand;
            _deleteGalleryImageCommand = deleteGalleryImageCommand;
        }

        public async Task<string> AddGalleryImageToPartnerAsync(GalleryImage galleryImage)
        {
            return (await _insertGalleryImageCommand.ExecuteAsync(galleryImage))?.Id;
        }

        public async Task<string> GetGalleryImageById(string id)
        {
            return await _readGalleryImageQuery.RunAsync(id);
        }

        public async Task<byte[]> GetGalleryImage(string id)
        {
            var imageBase64 = await _readGalleryImageQuery.RunAsync(id);
            if (string.IsNullOrEmpty(imageBase64))
            {
                return new byte[0];
            }

            return Convert.FromBase64String(imageBase64);
        }

        public async Task<IEnumerable<string>> GetGalleryImageIdsForPartner(string partnerId)
        {
            return await _listGalleryImageQuery.RunAsync(partnerId);
        }

        public async Task<bool> RemoveGalleryImage(string id)
        {
            return await _deleteGalleryImageCommand.ExecuteAsync(id);
        }
    }
}
