using System;
using System.Threading.Tasks;
using ScnDiscounts.DependencyInterface.GeoLocation;
using Xamarin.Forms;

namespace ScnDiscounts.Helpers
{
    public static class LocationHelper
    {
        static async public Task<GeoCoordinate> CurrenLocation()
        {
            double latitude = 0.0;
            double longitude = 0.0;

            if (IsGeoServiceAvailable())
            {
                try
                {
                    Position position = await DependencyService.Get<IGeolocator>().GetPositionAsync(10000);
                    latitude = position.Latitude;
                    longitude = position.Longitude;
                }
                catch(Exception ex)
                {
                }
            };

            return new GeoCoordinate(latitude, longitude);
        }

        static public bool IsGeoServiceAvailable()
        {
            return DependencyService.Get<IGeolocator>().IsGeolocationEnabled && DependencyService.Get<IGeolocator>().IsGeolocationAvailable;
        }

        static public async Task<double> DistanceFromMeToLocation(double latitude, double longitude)
        {
            double distance = 0;

            if (IsGeoServiceAvailable() && (latitude > 0) && (longitude > 0))
            {
                GeoCoordinate myLocation = await CurrenLocation();
                distance = DistanceCalculate(myLocation, new GeoCoordinate(latitude, longitude));
            }

            return distance;
        }

        static public double DistanceFromMeToLocation(double myLatitude, double myLongitude, double latitude, double longitude)
        {
            double distance = 0;

            if ((myLatitude > 0) && (myLongitude > 0) && (latitude > 0) && (longitude > 0))
            {
                distance = DistanceCalculate(new GeoCoordinate(myLatitude, myLongitude), new GeoCoordinate(latitude, longitude));
            }

            return distance;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        static private double DistanceCalculate(GeoCoordinate fromCoordanate, GeoCoordinate toCoordanate)
        {
            double distance = 0.0;
            double circumference = 40000.0; // Earth's circumference at the equator in km

            //Calculate radians
            double latitudeFromRad = DegreesToRadians(fromCoordanate.Latitude);
            double longitudeFromRad = DegreesToRadians(fromCoordanate.Longitude);
            double latititudeToRad = DegreesToRadians(toCoordanate.Latitude);
            double longitudeToRad = DegreesToRadians(toCoordanate.Longitude);

            double logitudeDiff = Math.Abs(longitudeFromRad - longitudeToRad);

            if (logitudeDiff > Math.PI)
            {
                logitudeDiff = 2.0 * Math.PI - logitudeDiff;
            }

            double angleCalculation =
                Math.Acos(
                  Math.Sin(latititudeToRad) * Math.Sin(latitudeFromRad) +
                  Math.Cos(latititudeToRad) * Math.Cos(latitudeFromRad) * Math.Cos(logitudeDiff));

            distance = circumference * angleCalculation / (2.0 * Math.PI);

            return distance;
        }
    }

    public struct GeoCoordinate
    {
        public GeoCoordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude;
        public double Longitude;
    }

}
