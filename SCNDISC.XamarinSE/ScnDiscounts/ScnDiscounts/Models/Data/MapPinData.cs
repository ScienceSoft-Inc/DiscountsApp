using Plugin.Geolocator.Abstractions;
using ScnDiscounts.Helpers;
using System.Collections.Generic;

namespace ScnDiscounts.Models.Data
{
    public class MapPinData
    {
        public string PartnerId { get; set; }

        public string Id { get; set; }

        public string Discount { get; set; }

        public string DiscountType { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        #region Name

        private readonly Dictionary<LanguageHelper.LangTypeEnum, string> _name =
            new Dictionary<LanguageHelper.LangTypeEnum, string>();

        public void SetName(string langCode, string value)
        {
            var lang = langCode.LangCodeToEnum();
            _name[lang] = value;
        }

        public string Name => _name.ContainsKey(AppParameters.Config.SystemLang)
            ? _name[AppParameters.Config.SystemLang]
            : string.Empty;
        
        #endregion

        #region Category

        public List<CategoryData> CategoryList { get; set; } = new List<CategoryData>();

        public CategoryData PrimaryCategory { get; set; }

        #endregion

        #region Distance

        public double DistanceValue { get; set; }

        public string DistanceString => DistanceValue.ToDistanceString();

        #endregion

        public void CalculateDistance()
        {
            var position = AppMobileService.Locaion.CurrentLocation;

            if (position != null)
            {
                var value = position.CalculateDistance(new Position(Latitude, Longitude),
                    GeolocatorUtils.DistanceUnits.Kilometers);

                DistanceValue = value;
            }
        }
    }
}
