using ScnDiscounts.Models.Database.Tables;

namespace ScnDiscounts.Models.Data
{
    public class GalleryImageData
    {
        public string FileName { get; set; }

        public GalleryImageData(GalleryImage galleryImage)
        {
            FileName = galleryImage.FileName;
        }
    }
}
