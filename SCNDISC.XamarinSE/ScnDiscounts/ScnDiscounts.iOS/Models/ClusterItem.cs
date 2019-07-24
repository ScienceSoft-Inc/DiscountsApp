using CoreLocation;
using Foundation;
using Google.Maps.Utils;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models.Data;
using UIKit;

namespace ScnDiscounts.iOS.Models
{
    public class ClusterItem : GMUClusterItem
    {
        public MapPinData Data { get; }

        public override CLLocationCoordinate2D Position { get; }

        public string Snippet { get; }

        public string Title { get; }

        public UIImage Icon { get; }

        public ClusterItem(MapPinData pinItem)
        {
            Data = pinItem;

            Position = new CLLocationCoordinate2D(pinItem.Latitude, pinItem.Longitude);
            Snippet = pinItem.Id;
            Title = pinItem.Name;

            var imageBytes = pinItem.PrimaryCategory.GetIconThemeBytes();
            Icon = UIImage.LoadFromData(NSData.FromArray(imageBytes), UIScreen.MainScreen.Scale);
        }
    }
}