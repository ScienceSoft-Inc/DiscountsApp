using System;
using System.Collections.Generic;
using System.Globalization;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models.WebService.MongoDB;

namespace ScnDiscounts.Models.Data
{
    public class MapPinData : NotifyPropertyChanged
    {
        public MapPinData()
        {
            _categorieList = new List<CategorieData>();
        }

        public MapPinData(DeserializeBranchItem deserializeBranch) : this()
        {
            NumberFormatInfo format = new NumberFormatInfo();
            format.NumberGroupSeparator = ",";
            format.NumberDecimalSeparator = ".";

            foreach (var item in deserializeBranch.Categories)
            {
                var categorie = new CategorieData();
                int typeCode = 0;
                int.TryParse(item.Type, out typeCode);
                categorie.TypeCode = typeCode;
                foreach (var itemName in item.Name)
                    categorie.SetName(itemName.Lan, itemName.LocText);
                _categorieList.Add(categorie);
            }

            if (_categorieList.Count > 0)
                _primaryCategory = _categorieList[0];
            else
                _primaryCategory = new CategorieData { TypeCode = -1 };

            try
            {
                _discount = deserializeBranch.Discounts[0].Name[0].LocText;
            }
            catch
            {
                _discount = "0";
            }

            _latitude = Double.Parse(deserializeBranch.Location.Coordinates.Latitude, format);
            _longitude = Double.Parse(deserializeBranch.Location.Coordinates.Longitude, format);
            _id = deserializeBranch.Id;
            _partnerId = deserializeBranch.PartnerId;

            foreach (var item in deserializeBranch.Name)
                SetName(item.Lan, item.LocText);
        }

        private string _partnerId;
        public string PartnerId
        {
            get { return _partnerId; }
            set { _partnerId = value; }
        }

        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _discount;
        public string Discount
        {
            get { return _discount; }
            set { _discount = value; }
        }

        private double _latitude;
        public double Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }

        private double _longitude;
        public double Longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }

        #region Name
        private Dictionary<LanguageHelper.LangTypeEnum, string> _name = new Dictionary<LanguageHelper.LangTypeEnum, string>(); 
        public void SetName(string langCode, string value)
        {
            var lang = LanguageHelper.LangCodeToEnum(langCode);
            _name.Add(lang, value);
        }

        public string Name
        {
            get
            {
                if (_name.ContainsKey(AppParameters.Config.SystemLang))
                    return _name[AppParameters.Config.SystemLang];
                return "empty";
            }
        }
        #endregion

        #region Category
        private List<CategorieData> _categorieList;
        public List<CategorieData> CategorieList { get { return _categorieList; } }

        private CategorieData _primaryCategory;
        public CategorieData PrimaryCategory
        {
            get { return _primaryCategory; }
            set { _primaryCategory = value; }
        }
        #endregion

        #region Distance
        private string _distance = "0.0";
        public string Distance
        {
            get { return _distance; }
            set
            {
                _distance = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public void CalculateDistance()
        {
            double value = LocationHelper.DistanceFromMeToLocation(AppMobileService.Locaion.CurrentLocation.Latitude, AppMobileService.Locaion.CurrentLocation.Longitude, Latitude, Longitude);
            Distance = value.ToString("0.##");
        }
    }
}
