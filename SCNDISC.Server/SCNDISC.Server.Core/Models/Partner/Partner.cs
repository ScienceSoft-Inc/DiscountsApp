using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SCNDISC.Server.Core.Models.Rating;

namespace SCNDISC.Server.Core.Models.Partner
{
    public class PartnerModel
    {
        [JsonProperty(PropertyName = "webAddresses")]
        public IEnumerable<WebAddressModel> WebAddresses { get; set; }

        [JsonProperty(PropertyName = "contacts")]
        public IEnumerable<ContactModel> Contacts { get; set; }

        [JsonProperty(PropertyName = "gallery")]
        public IEnumerable<string> Gallery { get; set; }

        [JsonProperty(PropertyName = "categories")]
        public IEnumerable<CategoryModel> Categories { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name_Ru")]
        public string Name_Ru { get; set; }

        [JsonProperty(PropertyName = "name_En")]
        public string Name_En { get; set; }

        [JsonProperty(PropertyName = "description_Ru")]
        public string Description_Ru { get; set; }

        [JsonProperty(PropertyName = "description_En")]
        public string Description_En { get; set; }

        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }

        [JsonProperty(PropertyName = "image")]
        public string Image { get; set; }

        [JsonProperty(PropertyName = "logo")]
        public string Logo { get; set; }

        [JsonProperty(PropertyName = "discount")]
        public double Discount { get; set; }

        [JsonProperty(PropertyName = "selectDiscount")]
        public string SelectDiscount { get; set; }
        [JsonProperty(PropertyName = "rating")]
        public PartnerRatingStatisticsModel Rating { get; set; }
    }

    public class DiscountModel
    {
        [JsonProperty(PropertyName = "categories")]
        public IEnumerable<CategoryModel> Categories { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name_Ru")]
        public string Name_Ru { get; set; }

        [JsonProperty(PropertyName = "name_En")]
        public string Name_En { get; set; }

        [JsonProperty(PropertyName = "description_Ru")]
        public string Description_Ru { get; set; }

        [JsonProperty(PropertyName = "description_En")]
        public string Description_En { get; set; }
    }
    public class WebAddressModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string URL { get; set; }

        [JsonProperty(PropertyName = "socialNetwork")]
        public string SocialNetwork { get; set; }

    }

    public class ContactModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "phoneNumber1")]
        public string PhoneNumber1 { get; set; }

        [JsonProperty(PropertyName = "phoneNumber2")]
        public string PhoneNumber2 { get; set; }

        [JsonProperty(PropertyName = "address_Ru")]
        public string Address_Ru { get; set; }

        [JsonProperty(PropertyName = "address_En")]
        public string Address_En { get; set; }

        [JsonProperty(PropertyName = "coordinates")]
        public string Coordinates { get; set; }
    }

    public class CategoryModel
    {
        [JsonProperty(PropertyName = "name_Ru")]
        public string Name_Ru { get; set; }
        
        [JsonProperty(PropertyName = "name_En")]
        public string Name_En { get; set; }

        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }

    public static class Languages
    {
        public const string Ru = "RU";
        public const string En = "EN";
    }

    public class GalleryImageModel
    {
        [JsonProperty(PropertyName = "partnerId")]
        public string PartnerId { get; set; }

        [JsonProperty(PropertyName = "image")]
        public string Image { get; set; }
    }
}