using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.Clustering;
using ScnDiscounts.Control;
using ScnDiscounts.DependencyInterface;
using ScnDiscounts.Droid.Models;
using ScnDiscounts.Droid.Renderers;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MapTile), typeof(MapTileRenderer))]

namespace ScnDiscounts.Droid.Renderers
{
    public class MapTileRenderer : MapRenderer
    {
        protected Marker LocationMarker;
        protected MarkerOptions LocationMarkerOptions = new MarkerOptions();
        protected ClusterManager ClusterManager;

        public MapTileRenderer(Context context) 
            : base(context)
        {
        }

        protected override void OnMapReady(GoogleMap googleMap)
        {
            googleMap.UiSettings.ZoomControlsEnabled = false;

            ClusterManager = new ClusterManager(Context, googleMap);
            ClusterManager.Renderer = new ClusterRenderer(Context, googleMap, ClusterManager);

            var icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.ic_pin_navigation);
            LocationMarkerOptions.SetIcon(icon);
            LocationMarkerOptions.Anchor(0.5f, 0.5f);
            LocationMarkerOptions.InvokeZIndex(1);

            var mapTile = (MapTile) Element;
            mapTile.PinUpdating += (sender, args) => Functions.SafeCall(UpdatePins);
            mapTile.LocationUpdating += (sender, args) => Functions.SafeCall(UpdateLocationPinPosition);
            mapTile.HeadingUpdating += (sender, args) => Functions.SafeCall(UpdateLocationPinHeading);

            googleMap.MapClick += Map_MapClick;
            googleMap.CameraMoveStarted += Map_CameraMoveStarted;
            googleMap.CameraIdle += Map_CameraPositionIdle;
            googleMap.MarkerClick += Map_MarkerClick;

            Element.SizeChanged += (sender, args) => SetSafeAreaPadding();

            var minskPosition = new Position(MapTile.MinskLat, MapTile.MinskLong);
            var minskRegion = MapSpan.FromCenterAndRadius(minskPosition, Distance.FromKilometers(5));
            mapTile.MoveToRegion(minskRegion);

            SetSafeAreaPadding();
        }

        private void SetSafeAreaPadding()
        {
            var safeAreaInsets = DependencyService.Get<IPhoneService>().SafeAreaInsets;
            var left = (int) Context.ToPixels(safeAreaInsets.Left);
            var top = (int) Context.ToPixels(safeAreaInsets.Top);
            var right = (int) Context.ToPixels(safeAreaInsets.Right);
            var bottom = (int) Context.ToPixels(safeAreaInsets.Bottom);
            NativeMap.SetPadding(left, top, right, bottom);
        }

        private void CloseDetailInfo()
        {
            var mapTile = (MapTile) Element;
            mapTile.CloseDetailInfo();
        }

        private void Map_CameraMoveStarted(object sender, GoogleMap.CameraMoveStartedEventArgs e)
        {
            CloseDetailInfo();
        }

        private void Map_MapClick(object sender, GoogleMap.MapClickEventArgs e)
        {
            CloseDetailInfo();
        }

        private void Map_CameraPositionIdle(object sender, EventArgs e)
        {
            ClusterManager.OnCameraIdle();

            var mapTile = (MapTile) Element;

            if (!string.IsNullOrEmpty(mapTile.SelectedPinId))
            {
                mapTile.ShowPinDetailInfo(mapTile.SelectedPinId);
                mapTile.SelectedPinId = null;
            }
        }

        private void UpdatePins()
        {
            var mapTile = (MapTile) Element;
            var pinItems = mapTile.PinList;

            ClusterManager.ClearItems();
            NativeMap.Clear();

            var clusterItems = pinItems.Select(i => new ClusterItem(i)).ToList();
            ClusterManager.AddItems(clusterItems);

            ClusterManager.Cluster();

            if (LocationMarkerOptions.Position != null)
                LocationMarker = NativeMap.AddMarker(LocationMarkerOptions);

            UpdateLocationPinPosition();
        }

        private void UpdateLocationPinPosition()
        {
            if (LocationHelper.IsGeoServiceAvailable)
            {
                var position = AppMobileService.Locaion.CurrentLocation;
                if (position != null)
                {
                    var latLng = new LatLng(position.Latitude, position.Longitude);

                    LocationMarkerOptions.SetPosition(latLng);

                    if (LocationMarker != null)
                        LocationMarker.Position = latLng;
                    else
                        LocationMarker = NativeMap.AddMarker(LocationMarkerOptions);
                }
            }
        }

        private void UpdateLocationPinHeading()
        {
            if (LocationHelper.IsGeoServiceAvailable)
            {
                var oldRotation = LocationMarkerOptions.Rotation;
                var newRotation = ((float) AppMobileService.Locaion.CurrentHeading - 45 -
                                   (NativeMap.CameraPosition?.Bearing).GetValueOrDefault()) % 360;

                if (Math.Abs(newRotation - oldRotation) > MapTile.MinCompassRotation)
                {
                    LocationMarkerOptions.SetRotation(newRotation);

                    if (LocationMarker != null)
                        LocationMarker.Rotation = newRotation;
                }
            }
        }

        private void Map_MarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
        {
            var mapTile = (MapTile) Element;
            mapTile.SelectedPinId = e.Marker.Snippet;

            if (!string.IsNullOrEmpty(mapTile.SelectedPinId))
            {
                var camera = CameraUpdateFactory.NewLatLng(e.Marker.Position);
                NativeMap.AnimateCamera(camera, 200, null);
            }
        }
    }
}