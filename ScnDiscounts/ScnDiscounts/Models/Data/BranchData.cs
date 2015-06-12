using System;
using System.Collections.Generic;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models.WebService.MongoDB;

namespace ScnDiscounts.Models.Data
{
    public class BranchData : NotifyPropertyChanged
    {
        public BranchData(DeserializeBranchItem branchItem)
        {
            _id = branchItem.Id;

            Double.TryParse(branchItem.Location.Coordinates.Latitude, out _latitude);
            Double.TryParse(branchItem.Location.Coordinates.Longitude, out _longitude);
            
            _address = new Dictionary<LanguageHelper.LangTypeEnum, string>();
            foreach (var item in branchItem.Address)
                SetAddress(item.Lan, item.LocText);

            _phoneList = new List<string>();
            foreach (var item in branchItem.Phones)
                _phoneList.Add(item.Number);

            //CalculateDistance();
        }

        #region Id
        private string _id = "";
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        #endregion

        #region Latitude
        private double _latitude;
        public double Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }
        #endregion

        #region Longitude
        private double _longitude;
        public double Longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }
        #endregion

        #region Address
        private Dictionary<LanguageHelper.LangTypeEnum, string> _address;
        private void SetAddress(string langCode, string value)
        {
            var lang = LanguageHelper.LangCodeToEnum(langCode);
            _address.Add(lang, value);
        }

        public string Address
        {
            get
            {
                if (_address.ContainsKey(AppParameters.Config.SystemLang))
                    return _address[AppParameters.Config.SystemLang];
                return "empty";
            }
        }
        #endregion

        #region Phones
        private List<string> _phoneList;
        public List<string> PhoneList { get { return _phoneList; } }
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
