using CoreGraphics;
using CoreLocation;
using Foundation;
using Google.Maps;
using Google.Maps.Utils;
using ScnDiscounts.Control;
using ScnDiscounts.Helpers;
using ScnDiscounts.iOS.Models;
using ScnDiscounts.iOS.Renderers;
using ScnDiscounts.Models;
using System;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MapTile), typeof(MapTileRenderer))]

namespace ScnDiscounts.iOS.Renderers
{
    public class MapTileRenderer : ViewRenderer<MapTile, MapView>
    {
        protected Marker LocationMarker;
        protected GMUClusterManager ClusterManager;

        public MapTileRenderer()
        {
            LocationMarker = new Marker
            {
                Icon = UIImage.FromBundle("ic_pin_navigation.png"),
                GroundAnchor = new CGPoint(0.5, 0.5),
                ZIndex = 1
            };
        }

        protected override void OnElementChanged(ElementChangedEventArgs<MapTile> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
                return;

            var mapTile = Element;

            mapTile.PinUpdating += (sender, args) => Functions.SafeCall(UpdatePins);
            mapTile.LocationUpdating += (sender, args) => Functions.SafeCall(UpdateLocationPinPosition);
            mapTile.HeadingUpdating += (sender, args) => Functions.SafeCall(UpdateLocationPinHeading);

            mapTile.RegionMoved += RegionMoved;

            mapTile.SizeChanged += (sender, args) => CloseDetailInfo();

            if (Control == null)
                SetGoogleMapControl();
        }

        private void SetGoogleMapControl()
        {
            var mapView = new MapView
            {
                Camera = CameraPosition.FromCamera(MapTile.MinskLat, MapTile.MinskLong, 11.9f),
                TappedMarker = TappedMarker,
                Settings =
                {
                    CompassButton = true
                }
            };

            mapView.CameraPositionIdle += Map_CameraPositionIdle;
            mapView.CoordinateTapped += Map_CoordinateTapped;
            mapView.PoiWithPlaceIdTapped += Map_PoiWithPlaceIdTapped;
            mapView.WillMove += Map_WillMove;

            var buckets = new[] {NSNumber.FromInt32(int.MaxValue)};
            var images = new[] {UIImage.FromBundle("ic_cluster.png")};
            var iconGenerator = new GMUDefaultClusterIconGenerator(buckets, images);
            var algorithm = new GMUNonHierarchicalDistanceBasedAlgorithm();
            var renderer = new ClusterRenderer(mapView, iconGenerator);
            ClusterManager = new GMUClusterManager(mapView, algorithm, renderer);

            SetNativeControl(mapView);
        }

        private void CloseDetailInfo()
        {
            var mapTile = Element;
            mapTile.CloseDetailInfo();
        }

        private void Map_CoordinateTapped(object sender, GMSCoordEventArgs e)
        {
            CloseDetailInfo();
        }

        private void Map_PoiWithPlaceIdTapped(object o, GMSPoiWithPlaceIdEventEventArgs gmsPoiWithPlaceIdEventEventArgs)
        {
            CloseDetailInfo();
        }

        private void Map_WillMove(object sender, GMSWillMoveEventArgs e)
        {
            CloseDetailInfo();
        }

        private void Map_CameraPositionIdle(object sender, GMSCameraEventArgs e)
        {
            var mapTile = Element;

            if (!string.IsNullOrEmpty(mapTile.SelectedPinId))
            {
                mapTile.ShowPinDetailInfo(mapTile.SelectedPinId);
                mapTile.SelectedPinId = null;
            }
        }

        private void UpdatePins()
        {
            var mapTile = Element;
            var map = Control;
            var pinItems = mapTile.PinList;

            ClusterManager.ClearItems();
            map.Clear();

            var clusterItems = pinItems.Select(i => new ClusterItem(i)).Cast<IGMUClusterItem>().ToArray();
            ClusterManager.AddItems(clusterItems);

            ClusterManager.Cluster();

            LocationMarker.Map = map;

            UpdateLocationPinPosition();
        }

        private void UpdateLocationPinPosition()
        {
            if (LocationHelper.IsGeoServiceAvailable)
            {
                var position = AppMobileService.Locaion.CurrentLocation;
                if (position != null)
                {
                    var latLng = new CLLocationCoordinate2D(position.Latitude, position.Longitude);

                    LocationMarker.Position = latLng;
                }
            }
        }

        private void UpdateLocationPinHeading()
        {
            if (LocationHelper.IsGeoServiceAvailable)
            {
                var oldRotation = LocationMarker.Rotation;
                var newRotation = (AppMobileService.Locaion.CurrentHeading - 45 -
                                   (Control.Camera?.Bearing).GetValueOrDefault()) % 360;

                if (Math.Abs(newRotation - oldRotation) > MapTile.MinCompassRotation)
                    LocationMarker.Rotation = newRotation;
            }
        }

        private bool TappedMarker(MapView mapView, Marker marker)
        {
            var mapTile = Element;

            if (marker.UserData is ClusterItem clusterItem)
            {
                mapTile.SelectedPinId = clusterItem.Snippet;

                if (!string.IsNullOrEmpty(mapTile.SelectedPinId))
                {
                    var map = Control;

                    if (Math.Abs(map.Camera.Target.Latitude - marker.Position.Latitude) < 0.0001 &&
                        Math.Abs(map.Camera.Target.Longitude - marker.Position.Longitude) < 0.0001)
                    {
                        Map_CoordinateTapped(this, new GMSCoordEventArgs(marker.Position));
                        Map_CameraPositionIdle(this, new GMSCameraEventArgs(map.Camera));
                    }
                    else
                    {
                        var camera = CameraPosition.FromCamera(marker.Position, map.Camera.Zoom);
                        map.Animate(camera);
                    }
                }
            }

            return false;
        }

        private void RegionMoved(object sender, MapRegionMoveEventArgs e)
        {
            var map = Control;

            if (Math.Abs(map.Camera.Target.Latitude - e.Latitude) < 0.0001 &&
                Math.Abs(map.Camera.Target.Longitude - e.Longitude) < 0.0001 &&
                Math.Abs(map.Camera.Zoom - e.Zoom) < 0.01)
            {
                var position = new CLLocationCoordinate2D(e.Latitude, e.Longitude);
                Map_CoordinateTapped(this, new GMSCoordEventArgs(position));
                Map_CameraPositionIdle(this, new GMSCameraEventArgs(map.Camera));
            }
            else
            {
                var camera = CameraPosition.FromCamera(e.Latitude, e.Longitude, e.Zoom);
                map.Animate(camera);
            }
        }
    }
}