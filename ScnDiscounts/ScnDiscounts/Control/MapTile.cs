using System;
using System.Collections.Generic;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models.Data;
using ScnDiscounts.Views.ContentUI;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Threading.Tasks;

namespace ScnDiscounts.Control
{
    public class MapTile : Map
    {
        public enum TileSourceEnum { tsNative, tsGoogle, tsOSM };
        static public Dictionary<TileSourceEnum, string> TileSourceList = new Dictionary<TileSourceEnum, string>
        {
            {TileSourceEnum.tsNative, Device.OnPlatform("", "Google", "Here") },
            {TileSourceEnum.tsGoogle, "Google" },
            {TileSourceEnum.tsOSM, "OpenStreetMap" }
        };

        public MapTile()
        {
            _mapLayout = new AbsoluteLayout();
            AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(this, new Rectangle(0f, 0f, 1f, 1f));
            _mapLayout.Children.Add(this);

            mapPinDetail = new MapPinDetail();
            mapPinDetail.IsVisible = false;

            AbsoluteLayout.SetLayoutFlags(mapPinDetail, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(mapPinDetail, new Rectangle(0f, 0f, 1f, 1f));
            
            _mapLayout.Children.Add(mapPinDetail);
        }

        public new void MoveToRegion(MapSpan mapSpan)
        {
            if (Device.OS != TargetPlatform.iOS)
                base.MoveToRegion(mapSpan);

            Console.WriteLine(mapSpan.Radius.Kilometers.ToString());
            OnRegionMoved(new MapRegionMoveEventArgs(mapSpan.Center.Latitude, mapSpan.Center.Longitude, (mapSpan.Radius.Kilometers == 2) ? 13 : 11));
        }

        public List<MapPinData> PinList = new List<MapPinData>();

        #region Event OnPinUpdate
        public event EventHandler PinUpdating;
        public void OnPinUpdate()
        {
            if (PinUpdating != null) PinUpdating(this, EventArgs.Empty);
        }
        #endregion

        #region Event OnClickPinDetail
        public event EventHandler<MapPinDataEventArgs> ClickPinDetail;
        public void OnClickPinDetail(MapPinDataEventArgs e)
        {
            if (ClickPinDetail != null) ClickPinDetail(this, e);
        }
        #endregion

        #region Event OnRegionMoved
        public event EventHandler<MapRegionMoveEventArgs> RegionMoved;
        public void OnRegionMoved(MapRegionMoveEventArgs e)
        {
            if (RegionMoved != null) RegionMoved(this, e);
        }
        #endregion

        private AbsoluteLayout _mapLayout;
        public AbsoluteLayout MapLayout
        {
            get { return _mapLayout; }
        }

        public MainContentUI Context { get; set; }

        #region DetailInfo
        private MapPinDetail mapPinDetail;

        static object locker = new object();

        private bool _isShowDetailInfo;
        public bool IsShowDetailInfo
        {
            get
            {
                lock (locker)
                    return _isShowDetailInfo;
            }
            set
            {
                lock (locker)
                {
                    _isShowDetailInfo = value;

                    if (!value)
                        CloseDetailInfo();
                }
            }
        }

        async public void ShowPinDetailInfo(string id)
        {
            MapPinData cutPinData = null;
            foreach (var item in PinList)
                if (item.Id == id)
                {
                    cutPinData = item;
                    break;
                }

            if (cutPinData == null)
                return;

            _isShowDetailInfo = true;

            if (Context != null)
            {
                mapPinDetail.DiscountCaption = Context.TxtDiscount;
                mapPinDetail.DistanceIcon = Context.ImgDistance;
                mapPinDetail.DistanceCaption = Context.TxtDistanceScaleValue;
                mapPinDetail.DetailIcon = Context.ImgDetail;
            }

            string categoryName = "";
            Color categoryColor = Color.Black;
            if (CategoryHelper.CategoryList.ContainsKey(cutPinData.PrimaryCategory.TypeCode))
            {
                var categoryParam = CategoryHelper.CategoryList[cutPinData.PrimaryCategory.TypeCode];
                categoryName = categoryParam.Name;
                categoryColor = categoryParam.ColorTheme;
            }

            mapPinDetail.DiscountValue = cutPinData.Discount + "%";
            mapPinDetail.CategoryName =  CategoryHelper.GetName(cutPinData.PrimaryCategory.TypeCode);
            mapPinDetail.CategoryColor = CategoryHelper.GetColorTheme(cutPinData.PrimaryCategory.TypeCode);
            mapPinDetail.Title = cutPinData.Name + ".";
            mapPinDetail.DistanceValue = cutPinData.Distance;
            await mapPinDetail.Show();
            
            var tapPinDetail = new TapGestureRecognizer();
            tapPinDetail.Tapped += async (sender, e) =>
            {
                await CloseDetailInfo();
                OnClickPinDetail(new MapPinDataEventArgs(cutPinData));
            };
            mapPinDetail.TapPinDetail = tapPinDetail;
        }

        async public Task CloseDetailInfo()
        {
            _isShowDetailInfo = false;
            await mapPinDetail.Hide();
        }

        public void LocationUpdate()
        {
            OnLocationUpdating();
        }

        public event EventHandler LocationUpdating;
        public void OnLocationUpdating()
        {
            if (LocationUpdating != null) LocationUpdating(this, EventArgs.Empty);
        }

        #endregion

        #region MapTilesSource
        public static readonly BindableProperty MapTilesSourceProperty =
            BindableProperty.Create<MapTile, TileSourceEnum>(p => p.MapTilesSource, TileSourceEnum.tsNative);

        public TileSourceEnum MapTilesSource
        {
            get { return (TileSourceEnum)GetValue(MapTilesSourceProperty); }
            set { SetValue(MapTilesSourceProperty, value); }
        }
        #endregion
    }

    public class MapPinDataEventArgs : EventArgs
    {
        public readonly MapPinData PinData;

        public MapPinDataEventArgs(MapPinData pinData)
        {
            PinData = pinData;
        } 
    }

    public class MapRegionMoveEventArgs : EventArgs
    {
        public readonly double Latitude;
        public readonly double Longitude;
        public readonly double Zoom;

        public MapRegionMoveEventArgs(double latitude, double longitude, double zoom)
        {
            Latitude = latitude;
            Longitude = longitude;
            Zoom = zoom;
        }
    }

}
