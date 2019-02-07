using Plugin.Geolocator;

namespace ScnDiscounts.Helpers
{
    public static class LocationHelper
    {
        public static bool IsGeoServiceEnabled => CrossGeolocator.IsSupported && CrossGeolocator.Current.IsGeolocationEnabled;

        public static bool IsGeoServiceAvailable => IsGeoServiceEnabled && CrossGeolocator.Current.IsGeolocationAvailable;

        public static string ToDistanceString(this double value)
        {
            return value.ToString("0.0#");
        }
    }
}
