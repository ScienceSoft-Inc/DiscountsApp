using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Core.Models.Partner;
using MongoDB.Driver.GeoJsonObjectModel;

namespace SCNDISC.Server.Core.Mapper
{
    public static class PartnerMapper
    {
        private const char CoordSeparator = ',';

        public static PartnerModel Map(List<Branch> branches, CategoryModel[] categories)
        {
	        var partner = new PartnerModel{Contacts = new List<ContactModel>()};
	        if (branches == null || branches.Count == 0)
	        {
		        return partner;
	        }

	        if (categories == null)
	        {
		        categories = new CategoryModel[0];
			}
            
            var contacts = new List<ContactModel>();
	        partner = FromBranchToPartnerModel(branches.First(), categories);

	        for (var i = 0; i < branches.Count; i++)
	        {
		        var branch = branches[i];

				var coordinates = MapGeoJsonToString(branch);
		        var contact = ContactMapper.MapToContactModels(branch.Address, branch.Phones, coordinates, branch.Id);
		        contacts.Add(contact);
			}

            partner.Contacts = contacts;
            return partner;
        }

        private static PartnerModel FromBranchToPartnerModel(Branch branch, CategoryModel[] categories)
        {
	        var partner = new PartnerModel
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
		        SelectDiscount = branch.Discounts?.FirstOrDefault()?.DiscountType
	        };

	        var categoriesList = new List<CategoryModel>();
            foreach (var id in branch.CategoryIds)
            {
                var category = categories.FirstOrDefault(x => x.Id == id);
	            if (category != null)
	            {
		            categoriesList.Add(category);
	            }

            }
            partner.Categories = categoriesList;
            partner.WebAddresses = WebAddressMapper.MapToWebAddressModels(branch.WebAddresses);
            return partner;
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
            var coordinates = branch.Location.Coordinates.Latitude.ToString(CultureInfo.InvariantCulture) + CoordSeparator +
                              branch.Location.Coordinates.Longitude.ToString(CultureInfo.InvariantCulture);
            return coordinates;
        }

        public static List<LocalizableText> SetValue(string valueRu, string valueEn)
        {
            List<LocalizableText> valueList = new List<LocalizableText>();
            LocalizableText resultRu = new LocalizableText
            {
                Lan = Languages.Ru,
                LocText = valueRu
            };
            if (resultRu.LocText == null)
            {
                resultRu.LocText = String.Empty;
            }
            LocalizableText resultEn = new LocalizableText
            {
                Lan = Languages.En,
                LocText = valueEn
            };
            if (resultEn.LocText == null)
            {
                resultEn.LocText = String.Empty;
            }
            valueList.Add(resultRu);
            valueList.Add(resultEn);
            return valueList;
        }

        private static Branch FromPartnerModelToBranch(PartnerModel partner, int numberOfContact)
        {
            Branch branch = new Branch();
            branch.PartnerId = partner.Id;
            branch.Comment = partner.Comment;
            branch.Image = partner.Image;
            branch.Icon = partner.Logo;
            branch.WebAddresses = WebAddressMapper.MapToWebAddresses(partner.WebAddresses);
            branch.Name = SetValue(partner.Name_Ru, partner.Name_En);
            branch.Description = SetValue(partner.Description_Ru, partner.Description_En);

            List<string> categoryIds = new List<string>();
            foreach (var category in partner.Categories)
            {
                categoryIds.Add(category.Id);
            }

            branch.CategoryIds = categoryIds;

            List<Discount> discounts = new List<Discount>();
            Discount discount = new Discount();
            discount.DiscountType = partner.SelectDiscount;
            var discountNames = SetValue(partner.Discount.ToString(CultureInfo.InvariantCulture), partner.Discount.ToString(CultureInfo.InvariantCulture));
            discount.Name = discountNames;
            discounts.Add(discount);
            branch.Discounts = discounts;

            branch.Address = ContactMapper.MapFromContactModelToAddress(partner.Contacts.ElementAt(numberOfContact));
            branch.Phones = ContactMapper.MapFromContactModelToPhone(partner.Contacts.ElementAt(numberOfContact));
            branch.Id = partner.Contacts.ElementAt(numberOfContact).Id;

            if (!string.IsNullOrEmpty(partner.Contacts.ElementAt(numberOfContact).Coordinates))
            {
                var stringCoords = partner.Contacts.ElementAt(numberOfContact).Coordinates.Split(CoordSeparator);
                var latitude = double.Parse(stringCoords[0], CultureInfo.InvariantCulture);
                var longitude = double.Parse(stringCoords[1], CultureInfo.InvariantCulture);
                branch.Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(longitude, latitude));
            }

            return branch;
        }

        public static IEnumerable<Branch> MapToBranches(PartnerModel partner)
        {
            if (partner == null)
            {
                return new List<Branch>();
            }
            List<Branch> branches = new List<Branch>();
            for (var i = 0; i< partner.Contacts.Count(); i++)
            {
                var branch = FromPartnerModelToBranch(partner, i);
                branches.Add(branch);
            }
            return branches;
        }

    }
}