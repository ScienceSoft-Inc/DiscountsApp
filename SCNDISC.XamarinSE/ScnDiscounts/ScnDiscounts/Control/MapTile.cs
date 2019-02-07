using ScnDiscounts.Models.Data;
using ScnDiscounts.Views.ContentUI;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace ScnDiscounts.Control
{
    public class MapTile : Map
    {
        public MapTile()
        {
            MapLayout = new AbsoluteLayout();

            AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(this, new Rectangle(0f, 0f, 1f, 1f));

            _mapPinDetail = new MapPinDetail
            {
                IsVisible = false
            };

            AbsoluteLayout.SetLayoutFlags(_mapPinDetail, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(_mapPinDetail,
                new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            
            MapLayout.Children.Add(this);
            MapLayout.Children.Add(_mapPinDetail);
        }

        public new void MoveToRegion(MapSpan mapSpan)
        {
            if (Device.RuntimePlatform != Device.iOS)
                base.MoveToRegion(mapSpan);

            double zoom;
            if (Math.Abs(mapSpan.Radius.Meters - 250) < 0.01)
                zoom = 16.2;
            else if (Math.Abs(mapSpan.Radius.Meters - 500) < 0.01)
                zoom = 15.2;
            else
                zoom = 11.9;

            OnRegionMoved(new MapRegionMoveEventArgs(mapSpan.Center.Latitude, mapSpan.Center.Longitude, (float) zoom));
        }

        private List<MapPinData> _pinList = new List<MapPinData>();

        public List<MapPinData> PinList
        {
            get => _pinList;
            set
            {
                if (_pinList != value)
                {
                    _pinList = value;
                    OnPropertyChanged(nameof(PinList));

                    OnPinUpdate();
                }
            }
        }

        #region Event OnPinUpdate
        public event EventHandler PinUpdating;
        private void OnPinUpdate()
        {
            PinUpdating?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Event OnClickPinDetail
        public event EventHandler<MapPinDataEventArgs> ClickPinDetail;
        public void OnClickPinDetail(MapPinDataEventArgs e)
        {
            ClickPinDetail?.Invoke(this, e);
        }
        #endregion

        #region Event OnRegionMoved
        public event EventHandler<MapRegionMoveEventArgs> RegionMoved;
        public void OnRegionMoved(MapRegionMoveEventArgs e)
        {
            RegionMoved?.Invoke(this, e);
        }
        #endregion

        public AbsoluteLayout MapLayout { get; }

        public MainContentUI Context { get; set; }

        #region DetailInfo
        private readonly MapPinDetail _mapPinDetail;

        public bool IsShowDetailInfo { get; set; }
        public string SelectedPinId { get; set; }

        public async void ShowPinDetailInfo(string id)
        {
            if (string.IsNullOrEmpty(id))
                return;

            var mapPinData = PinList.FirstOrDefault(i => i.Id == id);
            if (mapPinData == null)
                return;

            IsShowDetailInfo = true;

            _mapPinDetail.DiscountCaption = Context?.TxtDiscount;
            _mapPinDetail.DistanceIcon = Context?.ImgDistance;
            _mapPinDetail.DistanceCaption = Context?.TxtDistanceScaleValue;
            _mapPinDetail.DetailIcon = Context?.ImgDetail;
            _mapPinDetail.DiscountValue = mapPinData.Discount + mapPinData.DiscountType;
            _mapPinDetail.PrimaryCategory =  mapPinData.PrimaryCategory;
            _mapPinDetail.Title = mapPinData.Name;
            _mapPinDetail.DistanceValue = mapPinData.Distance;

            await _mapPinDetail.Show();
            
            var tapPinDetail = new TapGestureRecognizer();
            tapPinDetail.Tapped += (sender, e) =>
            {
                CloseDetailInfo();
                OnClickPinDetail(new MapPinDataEventArgs(mapPinData));
            };

            _mapPinDetail.TapGesture = tapPinDetail;
        }

        public async void CloseDetailInfo()
        {
            IsShowDetailInfo = false;
            await _mapPinDetail.Hide();
        }

        public void LocationUpdate()
        {
            OnLocationUpdating();
        }

        public event EventHandler LocationUpdating;
        public void OnLocationUpdating()
        {
            LocationUpdating?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        public void ActivatePin(MapPinData mapPinData)
        {
            if (mapPinData != null)
            {
                SelectedPinId = mapPinData.Id;

                var position = new Position(mapPinData.Latitude, mapPinData.Longitude);
                var mapSpan = MapSpan.FromCenterAndRadius(position, Distance.FromMeters(500));
                MoveToRegion(mapSpan);
            }
        }
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
        public readonly float Zoom;

        public MapRegionMoveEventArgs(double latitude, double longitude, float zoom)
        {
            Latitude = latitude;
            Longitude = longitude;
            Zoom = zoom;
        }
    }
}
