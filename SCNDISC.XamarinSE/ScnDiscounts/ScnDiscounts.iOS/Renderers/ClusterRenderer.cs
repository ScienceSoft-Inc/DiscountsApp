using Foundation;
using Google.Maps;
using Google.Maps.Utils;
using ScnDiscounts.iOS.Models;

namespace ScnDiscounts.iOS.Renderers
{
    public class ClusterRenderer : GMUDefaultClusterRenderer, IGMUClusterRendererDelegate
    {
        public ClusterRenderer(MapView mapView, IGMUClusterIconGenerator iconGenerator)
            : base(mapView, iconGenerator)
        {
            Delegate = this;
        }

        [Export("renderer:willRenderMarker:")]
        public void WillRenderMarker(IGMUClusterRenderer renderer, Marker marker)
        {
            if (marker.UserData is ClusterItem clusterItem)
                marker.Icon = clusterItem.Icon;
        }
    }
}