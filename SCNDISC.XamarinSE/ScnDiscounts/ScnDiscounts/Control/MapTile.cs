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
        public const int MinCompassRotation = 5;

        public MapTile()
        {
            MapLayout = new AbsoluteLayout();

            AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(this, new Rectangle(0f, 0f, 1f, 1f));

            MapPinDetail = new MapPinDetail
            {
                IsVisible = false
            };

            AbsoluteLayout.SetLayoutFlags(MapPinDetail, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(MapPinDetail,
                new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            
            MapLayout.Children.Add(this);
            MapLayout.Children.Add(MapPinDetail);
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
        public MapPinDetail MapPinDetail { get; }

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

            MapPinDetail.DiscountCaption = Context?.TxtDiscount;
            MapPinDetail.DistanceIcon = Context?.ImgDistance;
            MapPinDetail.DistanceCaption = Context?.TxtDistanceScaleValue;
            MapPinDetail.DetailIcon = Context?.ImgDetail;
            MapPinDetail.DiscountValue = mapPinData.Discount + mapPinData.DiscountType;
            MapPinDetail.PrimaryCategory =  mapPinData.PrimaryCategory;
            MapPinDetail.Title = mapPinData.Name;
            MapPinDetail.DistanceValue = mapPinData.Distance;

            await MapPinDetail.Show();
            
            var tapPinDetail = new TapGestureRecognizer();
            tapPinDetail.Tapped += (sender, e) =>
            {
                CloseDetailInfo();
                OnClickPinDetail(new MapPinDataEventArgs(mapPinData));
            };

            MapPinDetail.TapGesture = tapPinDetail;
        }

        public async void CloseDetailInfo()
        {
            IsShowDetailInfo = false;
            await MapPinDetail.Hide();
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

        public void HeadingUpdate()
        {
            OnHeadingUpdating();
        }

        public event EventHandler HeadingUpdating;
        public void OnHeadingUpdating()
        {
            HeadingUpdating?.Invoke(this, EventArgs.Empty);
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
