using System.Collections.Generic;

namespace ScnDiscounts.Models.Data
{
    public class DiscountDetailData
    {
        public string Title { get; set; }

        public string ImageFileName { get; set; }

        public string Description { get; set; }

        public string Persent { get; set; }

        public string DiscountType { get; set; }

        public List<CategoryData> CategoryList { get; set; }

        public List<WebAddressData> WebAddresses { get; set; }

        public List<DiscountDetailBranchData> BranchList { get; set; }

        public List<GalleryImageData> GalleryImages { get; set; }
    }
}
