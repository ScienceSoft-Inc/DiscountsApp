using SCNDISC.Server.Domain.Queries.Gallery;
using SCNDISC.Server.Infrastructure.Persistence.Providers;
using System.Collections.Generic;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace SCNDISC.Server.Infrastructure.Persistence.Queries.Gallery
{
    public class ListGalleryImageQuery : MongoCommandQueryBase<Domain.Aggregates.Partners.GalleryImage>, IListGalleryImageQuery
    {
        public ListGalleryImageQuery(IMongoCollectionProvider provider)
        : base(provider)
        {
        }

        public async Task<IEnumerable<string>>RunAsync(string partnerId)
        {
            return await Collection
                .Find(gi => gi.PartnerId == partnerId)
                .Project(gi => gi.Id).ToListAsync();
        }
    }

    public class ReadGalleryImageQuery : MongoCommandQueryBase<Domain.Aggregates.Partners.GalleryImage>, IReadGalleryImageQuery
    {
        public ReadGalleryImageQuery(IMongoCollectionProvider provider)
        : base(provider)
        {
        }

        public async Task<string> RunAsync(string id)
        {
            var galleryImage = await Collection
                .Find(gi => gi.Id == id)
                .FirstOrDefaultAsync();

            return galleryImage.Image;
        }
    }
    

}
