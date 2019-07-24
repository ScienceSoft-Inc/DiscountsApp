using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.Clustering;
using Android.Gms.Maps.Utils.Clustering.View;
using ScnDiscounts.Droid.Models;

namespace ScnDiscounts.Droid.Renderers
{
    public class ClusterRenderer : DefaultClusterRenderer
    {
        public ClusterRenderer(Context context, GoogleMap map, ClusterManager clusterManager)
            : base(context, map, clusterManager)
        {
        }

        protected override void OnBeforeClusterItemRendered(Java.Lang.Object item, MarkerOptions markerOptions)
        {
            if (item is ClusterItem clusterItem)
                markerOptions.SetIcon(clusterItem.Icon);
            else
                base.OnBeforeClusterItemRendered(item, markerOptions);
        }
    }
}