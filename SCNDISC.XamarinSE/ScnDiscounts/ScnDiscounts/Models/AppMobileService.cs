using Plugin.Compass;
using Plugin.Compass.Abstractions;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using ScnDiscounts.Helpers;
using System;
using System.Threading.Tasks;

namespace ScnDiscounts.Models
{
    public static class AppMobileService
    {
        public class GeoLocationService
        {
            public Position CurrentLocation { get; set; }

            public double CurrentHeading { get; set; }

            public void StartListening()
            {
                if (!CrossGeolocator.Current.IsListening)
                {
                    CrossGeolocator.Current.PositionChanged += GeoLocation_PositionChanged;
                    Functions.SafeCall(() => CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(5), 10));

                    if (CrossCompass.IsSupported)
                    {
                        CrossCompass.Current.CompassChanged += Compass_CompassChanged;
                        CrossCompass.Current.Start(SensorSpeed.UI);
                    }
                }
            }

            public void StopListening()
            {
                if (CrossCompass.IsSupported)
                {
                    CrossCompass.Current.Stop();
                    CrossCompass.Current.CompassChanged -= Compass_CompassChanged;
                }

                Functions.SafeCall(() => CrossGeolocator.Current.StopListeningAsync());
                CrossGeolocator.Current.PositionChanged -= GeoLocation_PositionChanged;
            }

            public async Task UpdateCurrentLocation()
            {
                var position = await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(5));
                GeoLocation_PositionChanged(this, new PositionEventArgs(position));
            }

            private void GeoLocation_PositionChanged(object sender, PositionEventArgs e)
            {
                CurrentLocation = e.Position;

                OnPositionUpdated();
            }

            private void Compass_CompassChanged(object sender, CompassChangedEventArgs e)
            {
                CurrentHeading = e.Heading;

                OnHeadingUpdated();
            }

            public event EventHandler PositionUpdated;

            public void OnPositionUpdated()
            {
                PositionUpdated?.Invoke(this, EventArgs.Empty);
            }

            public event EventHandler HeadingUpdated;

            public void OnHeadingUpdated()
            {
                HeadingUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public static GeoLocationService Locaion = new GeoLocationService();
    }
}
