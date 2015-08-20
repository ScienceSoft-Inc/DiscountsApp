using System;
using System.ComponentModel;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using ScnDiscounts.Control;
using ScnDiscounts.Droid.Renderers;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;
using Object = Java.Lang.Object;
//using Android.Views;

[assembly: ExportRenderer(typeof(MapTile), typeof(MapTileDroid))]

namespace ScnDiscounts.Droid.Renderers
{
    public class MapTileDroid : MapRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            var mapTile = (MapTile)Element;
            if (mapTile != null)
            {
                mapTile.PinUpdating += (ss, ee) => { UpdatePins(); };
                mapTile.LocationUpdating += (ss, ee) => { UpdatePins(); };

                NativeMap.MarkerClick += Map_MarkerClick;
                NativeMap.MapClick += Map_MapClick;
                NativeMap.UiSettings.ZoomControlsEnabled = false;
                
                //NativeMap.CameraChange += Map_CameraChange;
            }
        }

        void Map_CameraChange(object sender, GoogleMap.CameraChangeEventArgs e)
        {
            CloseDetail(); 
        }

        void Map_MapClick(object sender, GoogleMap.MapClickEventArgs e)
        {
            CloseDetail();
        }

        private const string CurrentLocationSnippet = "{E5B8792A-65DD-4ED7-9763-20301FEEC648}";

        void Map_MarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
        {
            var marker = e.Marker;
            var mapTile = (MapTile)Element;

            var newCamera = CameraUpdateFactory.NewLatLng(marker.Position);
            var moveCallBack = new MoveCallback();
            moveCallBack.Finishing += (ss, ee) => 
            {
                if (marker.Snippet != CurrentLocationSnippet)
                    mapTile.ShowPinDetailInfo(marker.Snippet);
            };
            NativeMap.AnimateCamera(newCamera, 200, moveCallBack);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }

        private void UpdatePins()
        {
            try
            {
                var mapTile = (MapTile)Element;

                NativeMap.Clear();

                foreach (var item in mapTile.PinList)
                {
                    var markerWithIcon = new MarkerOptions();

                    markerWithIcon.SetPosition(new LatLng(item.Latitude, item.Longitude));
                    markerWithIcon.SetTitle("");
                    markerWithIcon.SetSnippet(item.Id);
                    try
                    {
                        if (CategoryHelper.CategoryList.ContainsKey(item.PrimaryCategory.TypeCode))
                        {
                            var categoryParam = CategoryHelper.CategoryList[item.PrimaryCategory.TypeCode];
                            var constIcon = 0;

                            if (categoryParam.Icon == CategoryHelper.ic_pin_coffee)
                                constIcon = Resource.Drawable.ic_pin_coffee;
                            else if (categoryParam.Icon == CategoryHelper.ic_pin_cinema)
                                constIcon = Resource.Drawable.ic_pin_cinema;
                            else if (categoryParam.Icon == CategoryHelper.ic_pin_photo)
                                constIcon = Resource.Drawable.ic_pin_photo;
                            else if (categoryParam.Icon == CategoryHelper.ic_pin_clothing)
                                constIcon = Resource.Drawable.ic_pin_clothing;
                            else if (categoryParam.Icon == CategoryHelper.ic_pin_entertainment)
                                constIcon = Resource.Drawable.ic_pin_entertainment;
                            else if (categoryParam.Icon == CategoryHelper.ic_pin_sport)
                                constIcon = Resource.Drawable.ic_pin_sport;
                            else if (categoryParam.Icon == CategoryHelper.ic_pin_food)
                                constIcon = Resource.Drawable.ic_pin_food;

                            if (constIcon != 0)
                                markerWithIcon.InvokeIcon(BitmapDescriptorFactory.FromResource(constIcon));
                        }
                    }
                    catch (Exception)
                    {
                        markerWithIcon.InvokeIcon(BitmapDescriptorFactory.DefaultMarker());
                    }

                    NativeMap.AddMarker(markerWithIcon);
                }

                if (AppMobileService.Locaion.IsAvailable())
                {
                    var locationMarker = new MarkerOptions();

                    locationMarker.SetPosition(new LatLng(AppMobileService.Locaion.CurrentLocation.Latitude, AppMobileService.Locaion.CurrentLocation.Longitude));
                    locationMarker.SetTitle("");
                    locationMarker.SetSnippet(CurrentLocationSnippet);
                    try
                    {
                        locationMarker.InvokeIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.ic_pin_navigation));
                    }
                    catch (Exception)
                    {
                        locationMarker.InvokeIcon(BitmapDescriptorFactory.DefaultMarker());
                    }

                    NativeMap.AddMarker(locationMarker);
                }
            }
            catch (Exception ex)
            { 
            }
        }
    
        private void CloseDetail()
        {
            var mapTile = (MapTile)Element;
            mapTile.CloseDetailInfo();
        }
    }

    public class MoveCallback : Object,  GoogleMap.ICancelableCallback
    {
        public void OnCancel()
        {
        }

        public void OnFinish()
        {
            OnFinishing(EventArgs.Empty);
        }

        public event EventHandler Finishing;

        protected virtual void OnFinishing(EventArgs e)
        {
            if (Finishing != null) Finishing(this, e);
        }
    }

    /*public class InfoWindow : Java.Lang.Object, GoogleMap.IInfoWindowAdapter
    {
        #region IInfoWindowAdapter implementation

        public Android.Views.View GetInfoContents(Marker p0)
        {
            //throw new NotImplementedException();
            return null;
        }

        public Android.Views.View GetInfoWindow(Marker p0)
        {
            //throw new NotImplementedException();
            return null;
        }

        #endregion
    }*/

    /*public class CustomTileProvider : UrlTileProvider
    {
        string urlTemplate;

        public CustomTileProvider(int x, int y, string urlTemplate)
            : base(x, y)
        {
            //this.urlTemplate = urlTemplate;
        }

        public override Java.Net.URL GetTileUrl(int x, int y, int z)
        {
            //var url = urlTemplate.Replace("{z}", z.ToString()).Replace("{x}", x.ToString()).Replace("{y}", y.ToString());
            //Console.WriteLine(url);
            //return new Java.Net.URL(url);
        }
    }*/
}