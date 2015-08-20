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
using System.ComponentModel;

[assembly: ExportRenderer(typeof(MapTile), typeof(MapTileWP))]

namespace ScnDiscounts.WinPhone.Renderers
{
    public class MapTileWP : MapRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Maps.Map> e)
        {
            base.OnElementChanged(e);

            var mapTile = (MapTile)Element;

            if (e.OldElement == null)
                SetMapTilesSource();

            if (mapTile != null)
            {
                mapTile.PinUpdating += (ss, ee) => { UpdatePins(); };
                mapTile.LocationUpdating += mapTile_LocationUpdating;
            }
        }


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (String.Compare("MapTilesSource", e.PropertyName, StringComparison.CurrentCultureIgnoreCase) == 0)
                SetMapTilesSource();
        }

        private void SetMapTilesSource()
        {
            var mapTile = (MapTile)Element;
            var map = Control;
            
            if (mapTile != null)
            {
                map.TileSources.Clear();

                if (mapTile.MapTilesSource == MapTile.TileSourceEnum.tsGoogle)
                    map.TileSources.Add(new GoogleTileSource());
                else
                    if (mapTile.MapTilesSource == MapTile.TileSourceEnum.tsOSM)
                        map.TileSources.Add(new OSMTileSource());
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

        private PinMarker tapMarker;

        void pinMarker_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var map = Control;
            var mapTile = (MapTile)Element;

            PinMarker pinMarker = sender as PinMarker;
            tapMarker = pinMarker;
            MapPinData data = pinMarker.DataContext as MapPinData;

            var centerLocation = new System.Device.Location.GeoCoordinate(Math.Round(map.Center.Latitude, 4), Math.Round(map.Center.Longitude, 4));
            var pinLocation = new System.Device.Location.GeoCoordinate(Math.Round(data.Latitude, 4), Math.Round(data.Longitude, 4));
            //map.Center = pinLocation;
            //mapTile.ShowDetailInfo(data.Id);
            if (centerLocation == pinLocation)
                mapTile.ShowPinDetailInfo(data.Id);
            else
            {
                map.ViewChanged += map_ViewChanged;
                map.SetView(pinLocation, map.ZoomLevel, MapAnimationKind.Linear);
            }
        }

        void map_ViewChanged(object sender, MapViewChangedEventArgs e)
        {
            (sender as Map).ViewChanged -= map_ViewChanged;
            var map = Control;
            var mapTile = (MapTile)Element;
            MapPinData data = tapMarker.DataContext as MapPinData;

            var centerLocation = new System.Device.Location.GeoCoordinate(Math.Round(map.Center.Latitude, 4), Math.Round(map.Center.Longitude, 4));
            var pinLocation = new System.Device.Location.GeoCoordinate(Math.Round(data.Latitude, 4), Math.Round(data.Longitude, 4));
            if (centerLocation == pinLocation)
                mapTile.ShowPinDetailInfo(data.Id);
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

        public class OSMTileSource : TileSource
        {
            public OSMTileSource()
            {
                UriFormat = "http://{0}.tile.openstreetmap.org/{1}/{2}/{3}.png";
            }

            private readonly static string[] TilePathPrefixes = new[] { "a", "b", "c" };

            public override Uri GetUri(int x, int y, int zoomLevel)
            {
                if (zoomLevel > 0)
                {
                    var url = string.Format(UriFormat, TilePathPrefixes[y % 3], zoomLevel, x, y);
                    return new Uri(url);
                }
                return null;
            }
        }

        public class PinMarker : MapChildControl
        {
            public PinMarker(MapPinData pinData)
            {
                if (pinData.PrimaryCategory != null)
                {
                    System.Windows.Controls.Image imgPin = new System.Windows.Controls.Image();
                    var categoryParam = CategoryHelper.CategoryList[pinData.PrimaryCategory.TypeCode];
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
