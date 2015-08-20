using ScnDiscounts.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScnDiscounts.Models.Data
{
    public class DiscountDetailBranchData : NotifyPropertyChanged
    {
        public DiscountDetailBranchData()
        {
            _phoneList = new List<string>();
        }

        #region DocumentId
        private string _documentId = "";
        public string DocumentId
        {
            get { return _documentId; }
            set { _documentId = value; }
        }
        #endregion

        #region Address
        private string _address = "";
        public string Address
        {
            get { return _address; }
            set { _address = value; }
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


        #region Phones
        private List<string> _phoneList;
        public List<string> PhoneList { get { return _phoneList; } }
        #endregion

        #region Phone1
        public string Phone1
        {
            get { return (PhoneList.Count >= 1) ? PhoneList[0] : ""; }
        }

        public bool IsPhone1FillIn
        {
            get { return !String.IsNullOrWhiteSpace(Phone1); }
        }
        #endregion

        #region Phone2
        public string Phone2
        {
            get { return (PhoneList.Count >= 2) ? PhoneList[1] : ""; }
        }
        public bool IsPhone2FillIn
        {
            get { return !String.IsNullOrWhiteSpace(Phone2); }
        }
        #endregion

        #region Phone3
        public string Phone3
        {
            get { return (PhoneList.Count >= 3) ? PhoneList[2] : ""; }
        }
        public bool IsPhone3FillIn
        {
            get { return !String.IsNullOrWhiteSpace(Phone3); }
        }
        #endregion

        #region Phone4
        public string Phone4
        {
            get { return (PhoneList.Count >= 4) ? PhoneList[3] : ""; }
        }
        public bool IsPhone4FillIn
        {
            get { return !String.IsNullOrWhiteSpace(Phone4); }
        }
        #endregion

        #region Phone5
        public string Phone5
        {
            get { return (PhoneList.Count >= 5) ? PhoneList[4] : ""; }
        }
        public bool IsPhone5FillIn
        {
            get { return !String.IsNullOrWhiteSpace(Phone5); }
        }
        #endregion

        #region PhoneCount
        private int _phoneCount;
        public int PhoneCount { get { return _phoneList.Count; } }
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
