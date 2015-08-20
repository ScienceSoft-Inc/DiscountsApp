using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using MapKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Maps;
using Google.Maps;

using ScnDiscounts.Control;
using ScnDiscounts.iOS.Renderers;
using System.ComponentModel;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models.Data;
using CoreLocation;
using System.Threading.Tasks;
using ScnDiscounts.Models;
using CoreAnimation;
using CoreGraphics;

[assembly: ExportRenderer(typeof(MapTile), typeof(MapTileRenderer))]

namespace ScnDiscounts.iOS.Renderers
{
    public class MapTileRenderer : MapRenderer
    {
        protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);
            var mapTile = (MapTile)Element;

            if (e.OldElement == null)
            {
                mapTile.PinUpdating += (ss, ee) => { UpdatePins(); };
                mapTile.LocationUpdating += (ss, ee) => { UpdatePins(); };
                mapTile.RegionMoved += (ss, ee) => { RegionMoved(ee); };

                SetGoogleMapControl();
            }
        }

        private void SetGoogleMapControl()
        {
            var mapTile = (MapTile)Element;
            var mkMap = Control as MKMapView;
            mkMap.Delegate = null; //!!!IMPORTANT!!! without this code app crashs 

            var mapView = new MapView();
            mapView.TappedMarker = (map, marker) => 
            {
                TappedMarker(marker);
                return false; 
            };

            SetNativeControl(mapView);
        }
        
        private void UpdatePins()
        {
            try
            {
                var mapTile = (MapTile)Element;
                var map = Control as MapView;
                map.Clear();
                foreach (var item in mapTile.PinList)
                {
                    var categoryParam = CategoryHelper.CategoryList[item.PrimaryCategory.TypeCode];

                    Marker marker = new Marker();
                    marker.Position = new CoreLocation.CLLocationCoordinate2D(item.Latitude, item.Longitude);
                    marker.Icon = UIImage.FromFile(categoryParam.Icon);
                    marker.Map = map;
                    marker.UserData = NSObject.FromObject(item.Id);
                }

                if (AppMobileService.Locaion.IsAvailable())
                {
                    Marker marker = new Marker();
                    marker.Position = new CoreLocation.CLLocationCoordinate2D(AppMobileService.Locaion.CurrentLocation.Latitude, AppMobileService.Locaion.CurrentLocation.Longitude);
                    marker.Icon = UIImage.FromFile(@"MapPins/ic_pin_navigation.png");
                    marker.Map = map;
                }
            }
            catch
            { }
        }

        async private void TappedMarker(Marker marker)
        {
            if ((marker == null) || (marker.UserData == null))
                return;

            string pinId = marker.UserData.ToString();

            var map = Control as MapView;
            var mapTile = (MapTile)Element;
            
            var mapPinData = mapTile.PinList.First(x => x.Id == pinId);
            if (mapPinData != null)
            {
                var categoryParam = CategoryHelper.CategoryList[mapPinData.PrimaryCategory.TypeCode];
                marker.Icon = UIImage.FromFile(categoryParam.Icon).Scale(new CGSize(marker.Icon.Size.Width * 1.2, marker.Icon.Size.Height * 1.2)); ;
                await Task.Delay(200);
                marker.Icon = UIImage.FromFile(categoryParam.Icon);
            }



            /*CABasicAnimation rotationAnimation = CABasicAnimation.FromKeyPath("opacity");
            rotationAnimation.From = NSNumber.FromDouble(1);
            rotationAnimation.To = NSNumber.FromDouble(0.4);
            rotationAnimation.RepeatCount = 1;
            rotationAnimation.Duration = 0.4;
            marker.Layer.AddAnimation(rotationAnimation, "rotationAnimation");*/
            

            var centerLocation = new CLLocationCoordinate2D(Math.Round(map.Camera.Target.Latitude, 4), Math.Round(map.Camera.Target.Longitude, 4));
            var pinLocation = new CLLocationCoordinate2D(Math.Round(marker.Position.Latitude, 4), Math.Round(marker.Position.Longitude, 4));

            if ((centerLocation.Latitude == pinLocation.Latitude) & (centerLocation.Longitude == pinLocation.Longitude))
            {
                mapTile.ShowPinDetailInfo(pinId);
                pinId = "";
            }
            else
            {
                float zoom = (map.Camera.Zoom > 13) ? map.Camera.Zoom : 13;
                CameraPosition camera = CameraPosition.FromCamera(marker.Position.Latitude, marker.Position.Longitude, zoom);
                map.Animate(camera);

                map.CameraPositionChanged += async (s, e) =>
                {
                    if (!String.IsNullOrWhiteSpace(pinId))
                    {
                        await Task.Delay(500);
                        mapTile.ShowPinDetailInfo(pinId);
                    }
                    pinId = "";
                };
            };
        }

        private void RegionMoved(MapRegionMoveEventArgs e)
        {
            var map = Control as MapView;

            float zoom = e.Zoom > 0 ? (float)e.Zoom : map.Camera.Zoom;
            CameraPosition camera = CameraPosition.FromCamera(e.Latitude, e.Longitude, zoom);
            map.Animate(camera);
        }
    }
}