using MongoDB.Driver.GeoJsonObjectModel;
using SCNDISC.Server.Core.Models.Partner;
using SCNDISC.Server.Core.Models.Rating;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Infrastructure.Imaging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SCNDISC.Server.Core.Mapper
{
    public static class PartnerMapper
    {
        private const char CoordSeparator = ',';

        public static PartnerModel Map(List<Branch> branches, CategoryModel[] categories, List<string> galleryImagesIds, PartnerRatingStatisticsModel rating)
        {
	        var partner = new PartnerModel
            {
                Contacts = new List<ContactModel>()
            };

	        if (branches == null || branches.Count == 0)
                return partner;

            if (categories == null)
                categories = new CategoryModel[0];

            var contacts = new List<ContactModel>();
	        partner = FromBranchToPartnerModel(branches.First(b => b.Id == b.PartnerId), categories);
            foreach (var branch in branches)
            {
                var coordinates = MapGeoJsonToString(branch);
                var contact = ContactMapper.MapToContactModels(branch.Address, branch.Phones, coordinates, branch.Id);
                contacts.Add(contact);
            }

            partner.Contacts = contacts;
            partner.Gallery = galleryImagesIds;
            partner.Rating = rating;

            return partner;
        }

        private static PartnerModel FromBranchToPartnerModel(Branch branch, CategoryModel[] categories)
        {
            return new PartnerModel
            {
                Name_Ru = GetValue(branch.Name, Languages.Ru),
                Name_En = GetValue(branch.Name, Languages.En),
                Description_Ru = GetValue(branch.Description, Languages.Ru),
                Description_En = GetValue(branch.Description, Languages.En),
                Comment = branch.Comment,
                Image = branch.Image,
                Logo = branch.Icon,
                Id = branch.PartnerId,
                Discount = GetDiscount(branch.Discounts),
                SelectDiscount = branch.Discounts?.FirstOrDefault()?.DiscountType,
                Categories = branch.CategoryIds.Select(id => categories.FirstOrDefault(x => x.Id == id))
                    .Where(i => i != null).ToList(),
                WebAddresses = WebAddressMapper.MapToWebAddressModels(branch.WebAddresses)
            };
        }

	    private static double GetDiscount(IEnumerable<Discount> discounts)
	    {
		    return Convert.ToDouble(discounts?.FirstOrDefault()?.Name.FirstOrDefault()?.LocText ?? "0");
	    }

	    public static string GetValue(IEnumerable<LocalizableText> texts, string lang)
	    {
		    return texts?.FirstOrDefault(x => x.Lan == lang)?.LocText;
	    }

        private static string MapGeoJsonToString(Branch branch)
        {
            return branch.Location.Coordinates.Latitude.ToString(CultureInfo.InvariantCulture) + CoordSeparator +
                   branch.Location.Coordinates.Longitude.ToString(CultureInfo.InvariantCulture);
        }

        public static List<LocalizableText> SetValue(string valueRu, string valueEn)
        {
            var valueList = new List<LocalizableText>();
            var resultRu = new LocalizableText
            {
                Lan = Languages.Ru,
                LocText = valueRu ?? string.Empty
            };
            var resultEn = new LocalizableText
            {
                Lan = Languages.En,
                LocText = valueEn ?? string.Empty
            };
            valueList.Add(resultRu);
            valueList.Add(resultEn);
            return valueList;
        }

        private static Branch FromPartnerModelToBranch(PartnerModel partner, int numberOfContact)
        {
            var branch = new Branch
            {
                PartnerId = partner.Id,
                Comment = partner.Comment,
                Image = partner.Image,
                Icon = partner.Logo,
                WebAddresses = WebAddressMapper.MapToWebAddresses(partner.WebAddresses),
                Name = SetValue(partner.Name_Ru, partner.Name_En),
                Description = SetValue(partner.Description_Ru, partner.Description_En),
                CategoryIds = partner.Categories.Select(i => i.Id),
                Discounts = new List<Discount>
                {
                    new Discount
                    {
                        DiscountType = partner.SelectDiscount,
                        Name = SetValue(partner.Discount.ToString(CultureInfo.InvariantCulture),
                            partner.Discount.ToString(CultureInfo.InvariantCulture))
                    }
                },
                Address = ContactMapper.MapFromContactModelToAddress(partner.Contacts.ElementAt(numberOfContact)),
                Phones = ContactMapper.MapFromContactModelToPhone(partner.Contacts.ElementAt(numberOfContact)),
                Id = partner.Contacts.ElementAt(numberOfContact).Id
            };

            if (!string.IsNullOrEmpty(partner.Contacts.ElementAt(numberOfContact).Coordinates))
            {
                var stringCoords = partner.Contacts.ElementAt(numberOfContact).Coordinates.Split(CoordSeparator);
                var latitude = double.Parse(stringCoords[0], CultureInfo.InvariantCulture);
                var longitude = double.Parse(stringCoords[1], CultureInfo.InvariantCulture);
                branch.Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(longitude, latitude));
            }

            return branch;
        }

        public static IEnumerable<Branch> MapToBranches(PartnerModel partner, IImageConverter imageConverter)
        {
            if (partner == null)
                return new List<Branch>();
            var branches = new List<Branch>();
            ProcessPartnerMainImages(partner, imageConverter);
            for (var i = 0; i< partner.Contacts.Count(); i++)
            {
                var branch = FromPartnerModelToBranch(partner, i);
                branches.Add(branch);
            }
            return branches;
        }

        public static GalleryImage MapToGalleryImage(GalleryImageModel galleryImageModel, IImageConverter imageConverter)
        {
            if (galleryImageModel == null)
                return new GalleryImage();

            var galleryImage = new GalleryImage()
            {
                // image reprocessing
                Image = string.IsNullOrEmpty(galleryImageModel.Image) 
                ? galleryImageModel.Image 
                : Convert.ToBase64String(imageConverter.Convert(Convert.FromBase64String(galleryImageModel.Image), ImageOptions.Image)),
                PartnerId = galleryImageModel.PartnerId,
                Created = DateTime.UtcNow
            };

            return galleryImage;
        }

        public static Rating MapToPartnerRating(this PartnerRatingModel partnerRatingModel)
        {
            return partnerRatingModel != null
                ? new Rating
                {
                    Id = partnerRatingModel.Id,
                    DeviceId = partnerRatingModel.DeviceId,
                    Modified = DateTime.UtcNow,
                    PartnerId = partnerRatingModel.PartnerId,
                    Mark = partnerRatingModel.Mark
                }
                : new Rating();
        }

        public static PartnerRatingModel MapToPartnerRatingModel(this Rating rating)
        {
            return rating != null
                ? new PartnerRatingModel
                {
                    Id = rating.Id,
                    DeviceId = rating.DeviceId,
                    Modified = rating.Modified,
                    PartnerId = rating.PartnerId,
                    Mark = rating.Mark
                }
                : null;
        }

        public static PartnerRatingStatisticsModel MapToPartnerStatisticsModel(this IEnumerable<Rating> ratingData)
        {
            var items = ratingData.ToList();

            return new PartnerRatingStatisticsModel
            {
                RatingCount = items.Count,
                RatingSum = items.Sum(r => r.Mark)
            };
        }

        public static IEnumerable<PartnerRatingModel> MapToPartnerRatingModels(this IEnumerable<Rating> ratingData)
        {
            return ratingData.Select(i => new PartnerRatingModel
            {
                Id = i.Id,
                DeviceId = i.DeviceId,
                PartnerId = i.PartnerId,
                Mark = i.Mark,
                Modified = i.Modified
            });
        }

        private static void ProcessPartnerMainImages(PartnerModel partner, IImageConverter imageConverter)
        {
            partner.Image = string.IsNullOrEmpty(partner.Image)
                ? partner.Image 
                : Convert.ToBase64String(imageConverter.Convert(Convert.FromBase64String(partner.Image), ImageOptions.Image));
            partner.Logo = string.IsNullOrEmpty(partner.Logo) 
                ? partner.Logo 
                : Convert.ToBase64String(imageConverter.Convert(Convert.FromBase64String(partner.Logo), ImageOptions.Icon));
        }
    }
}