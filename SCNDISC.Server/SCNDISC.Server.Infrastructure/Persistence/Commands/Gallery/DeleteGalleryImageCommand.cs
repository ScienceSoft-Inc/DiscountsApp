using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Commands.Gallery;
using SCNDISC.Server.Infrastructure.Persistence.Providers;
using SCNDISC.Server.Infrastructure.Persistence.Queries;
using System.Threading.Tasks;
using MongoDB.Driver;
using System;

namespace SCNDISC.Server.Infrastructure.Persistence.Commands.Gallery
{
    public class DeleteGalleryImageCommand : MongoCommandQueryBase<GalleryImage>, IDeleteGalleryImageCommand
    {
        public DeleteGalleryImageCommand(IMongoCollectionProvider provider)
            : base(provider)
        {
        }

        public async Task<bool> ExecuteAsync(string galleryImageId)
        {
            var deletingItem = await Collection.Find(gi => gi.Id == galleryImageId).FirstOrDefaultAsync();
            var deleteResult = await Collection.DeleteOneAsync(gi => gi.Id == galleryImageId);

            if (deleteResult.IsAcknowledged && deleteResult.DeletedCount == 1)
            {
                if (!string.IsNullOrEmpty(galleryImageId) && deletingItem != null && deletingItem.PartnerId != null)
                {
                    var updateBranch = Builders<Branch>.
                    Update.
                    Set(x => x.Modified, DateTime.UtcNow);

                    var filterBranch = new FilterDefinitionBuilder<Branch>().Where(b => b.Id == deletingItem.PartnerId);
                    await Provider.GetCollection<Branch>().UpdateManyAsync(filterBranch, updateBranch);
                    return true;
                }
            }
                
            return false;
        }
    }
}
