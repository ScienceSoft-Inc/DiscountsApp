using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Commands.Gallery;
using SCNDISC.Server.Infrastructure.Persistence.Providers;
using SCNDISC.Server.Infrastructure.Persistence.Queries;
using System.Threading.Tasks;
using MongoDB.Driver;
using System;

namespace SCNDISC.Server.Infrastructure.Persistence.Commands.Gallery
{
    public class InsertGalleryImageCommand : MongoCommandQueryBase<GalleryImage>, IInsertGalleryImageCommand
    {
        public InsertGalleryImageCommand(IMongoCollectionProvider provider)
            : base(provider)
        {
        }

        public async Task<GalleryImage> ExecuteAsync(GalleryImage galleryImage)
        {
            await Collection.InsertOneAsync(galleryImage);

            if (!string.IsNullOrEmpty(galleryImage.Id))
            {
                var updateBranch = Builders<Branch>.
                Update.
                Set(x => x.Modified, DateTime.UtcNow);

                var filterBranch = new FilterDefinitionBuilder<Branch>().Where(b => b.Id == galleryImage.PartnerId);
                await Provider.GetCollection<Branch>().UpdateOneAsync(filterBranch, updateBranch);
            }

            return galleryImage;
        }
    }
}
