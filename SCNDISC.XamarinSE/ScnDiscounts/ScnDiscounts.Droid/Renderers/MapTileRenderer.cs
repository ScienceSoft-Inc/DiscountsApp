using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using ScnDiscounts.Control;
using ScnDiscounts.Droid.Renderers;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MapTile), typeof(MapTileRenderer))]

namespace ScnDiscounts.Droid.Renderers
{
    public class MapTileRenderer : MapRenderer
    {
        protected readonly object Locker = new object();

        public MapTileRenderer(Context context) 
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            var mapTile = (MapTile) Element;
            if (mapTile != null)
            {
                mapTile.PinUpdating += (sender, args) =>
                {
                    lock (Locker)
                    {
                        Functions.SafeCall(UpdatePins);
                    }
                };

                mapTile.LocationUpdating += (sender, args) =>
                {
                    lock (Locker)
                    {
                        Functions.SafeCall(UpdatePins);
                    }
                };

                if (NativeMap == null)
                    Control.GetMapAsync(this);
            }
        }

        protected override void OnMapReady(GoogleMap googleMap)
        {
            NativeMap.MapClick += Map_MapClick;
            NativeMap.CameraMoveStarted += Map_CameraMoveStarted;
            NativeMap.CameraIdle += Map_CameraPositionIdle;
            NativeMap.MarkerClick += Map_MarkerClick;
            NativeMap.UiSettings.ZoomControlsEnabled = false;
        }

        private void Map_CameraPositionIdle(object sender, EventArgs e)
        {
            var mapTile = (MapTile) Element;

            if (!string.IsNullOrEmpty(mapTile.SelectedPinId))
            {
                mapTile.ShowPinDetailInfo(mapTile.SelectedPinId);
                mapTile.SelectedPinId = null;
            }
        }

        private void Map_CameraMoveStarted(object sender, GoogleMap.CameraMoveStartedEventArgs e)
        {
            var mapTile = (MapTile) Element;
            mapTile.CloseDetailInfo();
        }

        private void Map_MapClick(object sender, GoogleMap.MapClickEventArgs e)
        {
            var mapTile = (MapTile) Element;
            mapTile.CloseDetailInfo();
        }

        private void UpdatePins()
        {
            var mapTile = (MapTile) Element;

            NativeMap.Clear();

            var pinItems = mapTile.PinList;
            foreach (var pinItem in pinItems)
            {
                var imageBytes = pinItem.PrimaryCategory.GetIconThemeBytes();
                var icon = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);

                var marker = new MarkerOptions();
                marker.SetPosition(new LatLng(pinItem.Latitude, pinItem.Longitude));
                marker.SetSnippet(pinItem.Id);
                marker.SetIcon(BitmapDescriptorFactory.FromBitmap(icon));

                NativeMap.AddMarker(marker);
            }

            if (LocationHelper.IsGeoServiceAvailable)
            {
                var position = AppMobileService.Locaion.CurrentLocation;
                if (position != null)
                {
                    var icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.ic_pin_navigation);

                    var marker = new MarkerOptions();
                    marker.SetPosition(new LatLng(position.Latitude, position.Longitude));
                    marker.SetIcon(icon);
                    marker.InvokeZIndex(1);

                    NativeMap.AddMarker(marker);
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