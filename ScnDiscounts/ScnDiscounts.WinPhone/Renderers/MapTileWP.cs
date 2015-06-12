using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using ScnDiscounts.Control;
using ScnDiscounts.WinPhone.Renderers;
using Xamarin.Forms.Maps.WP8;
using Xamarin.Forms.Platform.WinPhone;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;
using System.Device.Location;
using ScnDiscounts.Models.Data;
using System.Windows.Media.Imaging;
using ScnDiscounts.Models;
using ScnDiscounts.Helpers;

[assembly: ExportRenderer(typeof(MapTile), typeof(MapTileWP))]

namespace ScnDiscounts.WinPhone.Renderers
{
    public class MapTileWP : MapRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Maps.Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                var map = Control;
                map.TileSources.Add(new GoogleTileSource());
            }

            var mapTile = (MapTile)Element;
            if (mapTile != null)
            {
                mapTile.PinUpdating += (ss, ee) => { UpdatePins(); };
                mapTile.LocationUpdating += mapTile_LocationUpdating;
            }
        }

        void mapTile_LocationUpdating(object sender, EventArgs e)
        {
            var map = Control;
            var mapTile = (MapTile)Element;

            var toRemove = new List<MapLayer>();
            foreach (var item in map.Layers)
            {
                if (item.Count > 0)
                    if (item[0].Content is LocationMarker)
                        toRemove.Add(item);
            }

            foreach (var item in toRemove)
            {
                map.Layers.Remove(item);
            }

             var locationMarker = new LocationMarker(AppMobileService.Locaion.CurrentLocation);
             ///locationMarker.Tap += locationMarker_Tap;

             MapOverlay myOverlay = new MapOverlay()
             {
                 GeoCoordinate = locationMarker.GeoCoordinate,
                 PositionOrigin = new System.Windows.Point(0.5, 1.0),
                 Content = locationMarker
             };

             MapLayer layerLocation = new MapLayer();
             layerLocation.Add(myOverlay);
             map.Layers.Add(layerLocation);
        }

        void locationMarker_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
        }

        private void UpdatePins()
        {
            var map = Control;
            var mapTile = (MapTile)Element;
            
            var toRemove = new List<MapLayer>();
            foreach (var item in map.Layers)
            {
                if (item.Count > 0)
                    if (item[0].Content is PinMarker)
                        toRemove.Add(item);
            }

            foreach (var item in toRemove)
            {
                map.Layers.Remove(item);
            }

            foreach (var item in mapTile.PinList)
            {
                var pinMarker = new PinMarker(item);
                pinMarker.Tap += pinMarker_Tap;

                MapOverlay overlayPin = new MapOverlay
                {
                    GeoCoordinate = new System.Device.Location.GeoCoordinate(item.Latitude, item.Longitude),
                    PositionOrigin = new System.Windows.Point(0.5, 1.0),
                    Content = pinMarker,
                };

                MapLayer layerPin = new MapLayer();
                layerPin.Add(overlayPin);
                map.Layers.Add(layerPin);
            }
        }

        void pinMarker_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var map = Control;
            var mapTile = (MapTile)Element;

            PinMarker pinMarker = sender as PinMarker;
            MapPinData data = pinMarker.DataContext as MapPinData;
            map.Center = new System.Device.Location.GeoCoordinate(data.Latitude, data.Longitude);
            mapTile.ShowDetailInfo(data.Id);
            //map.SetView(new GeoCoordinate(data.Latitude, data.Longitude), map.ZoomLevel);

            map.ViewChanged += (ss, ee) =>
            {
                mapTile.ShowDetailInfo(data.Id);
            };
        }

        public class GoogleTileSource : TileSource
        {
            public enum GoogleType
            {
                Street = 'm',
                Hybrid = 'y',
                Satellite = 's',
                Physical = 't',
                PhysicalHybrid = 'p',
                StreetOverlay = 'h',
                WaterOverlay = 'r'
            }

            public GoogleTileSource()
            {
                MapType = GoogleType.Street;
                UriFormat = @"http://mt{0}.google.com/vt/lyrs={1}&z={2}&x={3}&y={4}";
            }

            public GoogleType MapType { get; set; }

            public override Uri GetUri(int x, int y, int zoomLevel)
            {
                return new Uri(
                  string.Format(UriFormat, (x % 2) + (2 * (y % 2)),
                  (char)MapType, zoomLevel, x, y));
            }
        }

        public class PinMarker : MapChildControl
        {
            public PinMarker(MapPinData pinData)
            {
                if (pinData.CategoryType > 0)
                {
                    System.Windows.Controls.Image imgPin = new System.Windows.Controls.Image();
                    var categoryParam = CategoryHelper.CategoryList[pinData.CategoryType];
                    BitmapImage bmp = new BitmapImage(new Uri(categoryParam.Icon, UriKind.Relative));
                    imgPin.Source = bmp;
                    //imgPin.Width = 60;
                    //imgPin.Height = 60;

                    Content = imgPin;
                    DataContext = pinData;
                    GeoCoordinate = new System.Device.Location.GeoCoordinate(pinData.Latitude, pinData.Longitude);
                    PositionOrigin = new System.Windows.Point(0.0, 0.0);
                }
            }
        }

        public class LocationMarker : MapChildControl
        {
            public LocationMarker(ScnDiscounts.Helpers.GeoCoordinate coordinates)
            {
                System.Windows.Controls.Image imgPin = new System.Windows.Controls.Image();
                BitmapImage bmp = new BitmapImage(new Uri(@"/assets/MapPins/ic_pin_navigation.png", UriKind.Relative));
                imgPin.Source = bmp;
                //imgPin.Width = 60;
                //imgPin.Height = 60;

                Content = imgPin;
                GeoCoordinate = new System.Device.Location.GeoCoordinate(coordinates.Latitude, coordinates.Longitude);
                PositionOrigin = new System.Windows.Point(0.0, 0.0);
            }
        }
    
    }
}
