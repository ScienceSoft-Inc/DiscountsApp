using System;
using ScnDiscounts.DependencyInterface;
using ScnDiscounts.DependencyInterface.GeoLocation;
using ScnDiscounts.Helpers;
using Xamarin.Forms;

namespace ScnDiscounts.Models
{
    static public class AppMobileService
    {
        public class GeoLocationService
        {
            public void StartListening()
            {
                if (!DependencyService.Get<IGeolocator>().IsListening)
                {
                    DependencyService.Get<IGeolocator>().StartListening(5000, 10);
                    DependencyService.Get<IGeolocator>().PositionChanged += GeoLocation_PositionChanged;
                }
            }


            public void StopListening()
            {
                DependencyService.Get<IGeolocator>().PositionChanged -= GeoLocation_PositionChanged;
                DependencyService.Get<IGeolocator>().StopListening();
            }

            public void UpdateCurrentLocation()
            {
                DependencyService.Get<IGeolocator>().GetPositionAsync(5000);
            }

            private GeoCoordinate _currentLocation = new GeoCoordinate(0, 0);
            public GeoCoordinate CurrentLocation
            {
                get { return _currentLocation; }
            }

            void GeoLocation_PositionChanged(object sender, PositionEventArgs e)
            {
                Device.BeginInvokeOnMainThread ( () => 
                {
                    _currentLocation.Latitude = e.Position.Latitude;
                    _currentLocation.Longitude = e.Position.Longitude;

                    OnPositionUpdated();
                });
            }

            public event EventHandler PositionUpdated;
            public void OnPositionUpdated()
            {
                if (PositionUpdated != null) PositionUpdated(this, EventArgs.Empty);
            }

            public bool IsAvailable()
            {
                return DependencyService.Get<IGeolocator>().IsGeolocationEnabled && DependencyService.Get<IGeolocator>().IsGeolocationAvailable;
            }

        }

        public class NetworkService
        {
            public bool IsAvailable()
            {
                return (DependencyService.Get<INetwork>().InternetConnectionStatus() != NetworkStatus.NotReachable);
            }



        }

        static public GeoLocationService Locaion = new GeoLocationService();
        static public NetworkService Network = new NetworkService();
    }

    public class GeoLocationEventArgs : EventArgs
    {
        public readonly GeoCoordinate Coordinate;

        public GeoLocationEventArgs(GeoCoordinate coordinates)
        {
            Coordinate = coordinates;
        }
    }
}
