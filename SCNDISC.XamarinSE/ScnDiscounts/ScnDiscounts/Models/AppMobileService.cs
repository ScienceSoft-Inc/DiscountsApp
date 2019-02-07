using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Threading.Tasks;

namespace ScnDiscounts.Models
{
    public static class AppMobileService
    {
        public class GeoLocationService
        {
            public void StartListening()
            {
                if (!CrossGeolocator.Current.IsListening)
                {
                    CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(5), 10);
                    CrossGeolocator.Current.PositionChanged += GeoLocation_PositionChanged;
                }
            }

            public void StopListening()
            {
                CrossGeolocator.Current.PositionChanged -= GeoLocation_PositionChanged;
                CrossGeolocator.Current.StopListeningAsync();
            }

            public async Task UpdateCurrentLocation()
            {
                var position = await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(5));
                GeoLocation_PositionChanged(this, new PositionEventArgs(position));
            }

            public Position CurrentLocation { get; set; }

            private void GeoLocation_PositionChanged(object sender, PositionEventArgs e)
            {
                CurrentLocation = e.Position;

                OnPositionUpdated();
            }

            public event EventHandler PositionUpdated;

            public void OnPositionUpdated()
            {
                PositionUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public static GeoLocationService Locaion = new GeoLocationService();
    }
}
