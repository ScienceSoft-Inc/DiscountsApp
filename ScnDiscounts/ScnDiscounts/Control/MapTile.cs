using System;
using System.Collections.Generic;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models.Data;
using ScnDiscounts.Views.ContentUI;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace ScnDiscounts.Control
{
    public class MapTile : Map
    {
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

        public List<MapPinData> PinList = new List<MapPinData>();

        public void OnPinUpdate()
        {
            if (PinUpdating != null) PinUpdating(this, EventArgs.Empty);
        }
        public event EventHandler PinUpdating;

        public event EventHandler<MapPinDataEventArgs> ClickPinDetail;
        public void OnClickPinDetail(MapPinDataEventArgs e)
        {
            if (ClickPinDetail != null) ClickPinDetail(this, e);
        }

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

        public void ShowDetailInfo(string id)
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
            if (CategoryHelper.CategoryList.ContainsKey(cutPinData.CategoryType))
            {
                var categoryParam = CategoryHelper.CategoryList[cutPinData.CategoryType];
                categoryName = categoryParam.Name;
            }

            mapPinDetail.DiscountValue = cutPinData.Discount + "%";
            mapPinDetail.CategoryName = cutPinData.CaregoryName;
            mapPinDetail.Title = cutPinData.Name + ".";
            mapPinDetail.DistanceValue = cutPinData.Distance;
            mapPinDetail.Show();
            

            var tapPinDetail = new TapGestureRecognizer();
            tapPinDetail.Tapped += (sender, e) =>
            {
                CloseDetailInfo();
                OnClickPinDetail(new MapPinDataEventArgs(cutPinData));
            };
            mapPinDetail.TapPinDetail = tapPinDetail;
        }

        public void CloseDetailInfo()
        {
            _isShowDetailInfo = false;
            mapPinDetail.Hide();
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
    }

    public class MapPinDataEventArgs : EventArgs
    {
        public readonly MapPinData PinData;

        public MapPinDataEventArgs(MapPinData pinData)
        {
            PinData = pinData;
        } 
    }
}
