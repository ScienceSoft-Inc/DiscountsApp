using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.Clustering;
using Android.Graphics;
using Java.Lang;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models.Data;

namespace ScnDiscounts.Droid.Models
{
    public class ClusterItem : Object, IClusterItem
    {
        public MapPinData Data { get; }

        public LatLng Position { get; }

        public string Snippet { get; }

        public string Title { get; }

        public BitmapDescriptor Icon { get; }

        public ClusterItem(MapPinData pinItem)
        {
            Data = pinItem;

            Position = new LatLng(pinItem.Latitude, pinItem.Longitude);
            Snippet = pinItem.Id;
            Title = pinItem.Name;

            var imageBytes = pinItem.PrimaryCategory.GetIconThemeBytes();
            var icon = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
            Icon = BitmapDescriptorFactory.FromBitmap(icon);
        }
    }
}