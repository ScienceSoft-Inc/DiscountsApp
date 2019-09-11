using Plugin.Geolocator.Abstractions;
using ScnDiscounts.Helpers;
using Xamarin.Forms;

namespace ScnDiscounts.Models.Data
{
    public class DiscountDetailBranchData : NotifyPropertyChanged //TODO: is NotifyPropertyChanged really needed?
    {
        public string DocumentId { get; set; }

        public string Address { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        #region Phone1

        private string _phone1;

        public string Phone1
        {
            get => _phone1;
            set
            {
                if (_phone1 != value)
                {
                    _phone1 = value.FormatPhoneNumber(out var phoneOperator);
                    OnPropertyChanged();

                    OnPropertyChanged(nameof(IsPhone1Exists));

                    PhoneOperator1 = phoneOperator;
                }
            }
        }

        private PhoneOperatorEnum _phoneOperator1;

        public PhoneOperatorEnum PhoneOperator1
        {
            get => _phoneOperator1;
            set
            {
                if (_phoneOperator1 != value)
                {
                    _phoneOperator1 = value;
                    OnPropertyChanged();

                    OnPropertyChanged(nameof(PhoneOperatorIcon1));
                }
            }
        }

        public ImageSource PhoneOperatorIcon1 => PhoneOperator1.GetPhoneOperatorIcon();

        public bool IsPhone1Exists => !string.IsNullOrWhiteSpace(Phone1);

        #endregion

        #region Phone2

        private string _phone2;

        public string Phone2
        {
            get => _phone2;
            set
            {
                if (_phone2 != value)
                {
                    _phone2 = value.FormatPhoneNumber(out var phoneOperator);
                    OnPropertyChanged();

                    OnPropertyChanged(nameof(IsPhone2Exists));

                    PhoneOperator2 = phoneOperator;
                }
            }
        }

        private PhoneOperatorEnum _phoneOperator2;

        public PhoneOperatorEnum PhoneOperator2
        {
            get => _phoneOperator2;
            set
            {
                if (_phoneOperator2 != value)
                {
                    _phoneOperator2 = value;
                    OnPropertyChanged();

                    OnPropertyChanged(nameof(PhoneOperatorIcon2));
                }
            }
        }

        public ImageSource PhoneOperatorIcon2 => PhoneOperator2.GetPhoneOperatorIcon();

        public bool IsPhone2Exists => !string.IsNullOrWhiteSpace(Phone2);

        #endregion

        #region Distance

        private double _distance;
        public double Distance
        {
            get => _distance;
            set
            {
                _distance = value;
                OnPropertyChanged();

                OnPropertyChanged(nameof(DistanceString));
            }
        }

        public string DistanceString => Distance.ToDistanceString();

        #endregion

        public void CalculateDistance()
        {
            var location = AppMobileService.Locaion.CurrentLocation;
            if (location != null)
            {
                var position = new Position(Latitude, Longitude);
                Distance = location.CalculateDistance(position, GeolocatorUtils.DistanceUnits.Kilometers);
            }
        }
    }
}
