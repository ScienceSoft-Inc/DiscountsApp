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
            //SetName()
        }

        public MapPinData(DeserializeBranchItem deserializeBranch)
        {
            NumberFormatInfo format = new NumberFormatInfo();
            format.NumberGroupSeparator = ",";
            format.NumberDecimalSeparator = ".";

            _categoryType = 0;

            if ((deserializeBranch.Categories != null) && (deserializeBranch.Categories.Count > 0))
            {
                int.TryParse(deserializeBranch.Categories[0].Type, out _categoryType);

                foreach (var item in deserializeBranch.Categories[0].Name)
                    SetCategory(item.Lan, item.LocText);
            }
            
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

        private int _categoryType;
        public int CategoryType
        {
            get { return _categoryType; }
            set { _categoryType = value; }
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
        private void SetName(string langCode, string value)
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
        private Dictionary<LanguageHelper.LangTypeEnum, string> _category = new Dictionary<LanguageHelper.LangTypeEnum, string>();
        private void SetCategory(string langCode, string value)
        {
            var lang = LanguageHelper.LangCodeToEnum(langCode);
            _category.Add(lang, value);
        }

        public string CaregoryName
        {
            get
            {
                if (_category.ContainsKey(AppParameters.Config.SystemLang))
                    return _category[AppParameters.Config.SystemLang];
                return "noname";
            }
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
