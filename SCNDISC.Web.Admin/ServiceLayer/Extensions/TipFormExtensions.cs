using MongoDB.Driver.GeoJsonObjectModel;
using SCNDISC.Server.Domain.Aggregates.Partners;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace SCNDISC.Web.Admin.ServiceLayer.Extensions
{
    public static class TipFormExtensions
    {
        private const char coordSeparator = ',';
        private const string phoneName = "Phone";

        public static bool IsPartner(this TipForm tipForm)
        {
            return tipForm.Id == tipForm.PartnerId;
        }

        public static Branch ToBranch(this TipForm tipForm)
        {
            var branch = EnitiyExtensionsMapper.MapPublicProperties<TipForm, Branch>(tipForm);
	        MapWebAddresses(tipForm, branch);
            MapPointToGeoJson(tipForm, branch);
            MapPhones(tipForm, branch);
            MapDiscount(tipForm, branch);
            branch.CategoryIds = tipForm.Categories;
            return EnitiyExtensionsMapper.MapFlatToLocalizableProperties(tipForm, branch);
        }

	    private static void MapWebAddresses(TipForm tipForm, Branch branch)
	    {
		    branch.WebAddresses = new List<WebAddress>();

			if (tipForm.WebAddresses == null || tipForm.WebAddresses.Count == 0)
		    {
				return;
		    }

			var addresses = new List<WebAddress>();
		    foreach (var webAddress in tipForm.WebAddresses)
		    {
			    var address = EnitiyExtensionsMapper.MapPublicProperties<WebAddressItem, WebAddress>(webAddress);
			    addresses.Add(address);
			}

		    branch.WebAddresses = addresses;
	    }

	    public static TipForm ToTipForm(this Branch branch)
        {
            var tipForm = EnitiyExtensionsMapper.MapPublicProperties<Branch, TipForm>(branch);

			MapWebAddresses(branch, tipForm);
			MapGeoJsonToPoint(branch, tipForm);
            MapPhones(branch, tipForm);
            MapDiscount(branch, tipForm);
            tipForm.Categories = branch.CategoryIds?.ToList() ?? new List<string>();
            return EnitiyExtensionsMapper.MapLocalizableToFlatProperties(branch, tipForm);
        }

	    private static void MapWebAddresses(Branch branch, TipForm tipForm)
	    {
		    tipForm.WebAddresses = new List<WebAddressItem>();

		    if (branch.WebAddresses == null || !branch.WebAddresses.Any())
		    {
			    return;
		    }

		    var addresses = new List<WebAddressItem>();
		    foreach (var webAddress in branch.WebAddresses)
		    {
			    var address = EnitiyExtensionsMapper.MapPublicProperties<WebAddress, WebAddressItem>(webAddress);
			    addresses.Add(address);
		    }

		    var index = 0;
		    addresses.ForEach(x=> x.Index = ++index);

			tipForm.WebAddresses = addresses;
		}

	    private static void MapDiscount(TipForm tipForm, Branch branch)
        {
            if (!string.IsNullOrEmpty(tipForm.Discount))
            {
	            branch.Discounts = new List<Discount>
	            {
		            new Discount
		            {
			            Name = new List<LocalizableText>
			            {
				            new LocalizableText {Lan = "RU", LocText = tipForm.Discount},
				            new LocalizableText {Lan = "EN", LocText = tipForm.Discount}
			            },
						DiscountType = tipForm.DiscountType
		            }
	            };
            }
        }
        
        private static void MapDiscount(Branch branch, TipForm tipForm)
        {
            if (branch.Discounts!= null && branch.Discounts.FirstOrDefault() != null)
            {
                var discount = branch.Discounts.First();
                if (discount.Name != null && discount.Name.Any())
                {
                    tipForm.Discount = discount.Name.First().LocText;
                }

	            tipForm.DiscountType = discount.DiscountType;
            }
        }

        private static void MapPhones(Branch branch, TipForm tipForm)
        {
            if (branch.Phones != null)
            {
                var i = 1;
                foreach (var phone in branch.Phones)
                {
                    var targetProperty = tipForm.GetType().GetTypeInfo().GetProperty(string.Format("{0}{1}", phoneName, i));
                    if (targetProperty != null)
                    {
                        targetProperty.SetValue(tipForm, phone.Number);
                    }
                    i++;
                }
            }
        }

        private static void MapPhones(TipForm tipForm, Branch branch)
        {
            var phones = new List<Phone>();
            int i = 1;
            while (true)
            {
                var sourceProperty = tipForm.GetType().GetTypeInfo().GetProperty(string.Format("{0}{1}", phoneName, i));
                if (sourceProperty != null)
                {
                    var value = (string)sourceProperty.GetValue(tipForm);
                    if (!string.IsNullOrEmpty(value))
                    {
                        phones.Add(new Phone() {Number = value});
                    }
                }
                else
                {
                    branch.Phones = phones;
                    break;
                }
                i++;
            }
        }

        public static void MapGeoJsonToPoint(Branch branch, TipForm tipForm)
        {
            tipForm.Point = string.Format("{0}{1}{2}", branch.Location.Coordinates.Latitude, coordSeparator, branch.Location.Coordinates.Longitude);
        }

        public static void MapPointToGeoJson(TipForm tipForm, Branch branch)
        {
            if (!string.IsNullOrEmpty(tipForm.Point))
            {
                var stringCoords = tipForm.Point.Split(coordSeparator);
                var latitude = double.Parse(stringCoords[0], CultureInfo.InvariantCulture);
                var longitude = double.Parse(stringCoords[1], CultureInfo.InvariantCulture);
                branch.Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(longitude, latitude));
            }
        }
    }
}