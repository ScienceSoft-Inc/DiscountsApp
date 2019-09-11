using ScnDiscounts.DependencyInterface;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models;
using ScnDiscounts.Models.Data;
using ScnDiscounts.Views.ContentUI;
using ScnPage.Plugin.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ScnDiscounts.ViewModels
{
    public class DiscountDetailViewModel : BaseViewModel
    {
        private DiscountDetailContentUI contentUI => (DiscountDetailContentUI) ContentUI;

        public DiscountDetailData DiscountDetailData { get; set; }

        public string ImageFileName => DiscountDetailData.ImageFileName;

        public string DiscountPercent => DiscountDetailData.Persent;

        public string DiscountType => DiscountDetailData.DiscountType;

        public List<CategoryData> Categories => DiscountDetailData.CategoryList;

        public List<WebAddressData> WebAddresses => DiscountDetailData.WebAddresses;

        public string NameCompany => DiscountDetailData.Title;

        public string Description => DiscountDetailData.Description;

        private int _userRating;

        public int UserRating
        {
            get => _userRating;
            set
            {
                if (_userRating != value)
                {
                    _userRating = value;
                    OnPropertyChanged();

                    UpdatePersonalRating();
                }
            }
        }

        private bool _isSubmittingRating;

        public bool IsSubmittingRating
        {
            get => _isSubmittingRating;
            set
            {
                if (_isSubmittingRating != value)
                {
                    _isSubmittingRating = value;
                    OnPropertyChanged();

                    OnPropertyChanged(nameof(IsLoadingRating));
                    OnPropertyChanged(nameof(IsDiscountRatingAvailable));
                }
            }
        }

        private bool _isLoadingDiscountRating;

        public bool IsLoadingDiscountRating
        {
            get => _isLoadingDiscountRating;
            set
            {
                if (_isLoadingDiscountRating != value)
                {
                    _isLoadingDiscountRating = value;
                    OnPropertyChanged();

                    OnPropertyChanged(nameof(IsLoadingRating));
                }
            }
        }

        public bool IsDiscountRatingAvailable => DiscountDetailData.DiscountRating != null && !IsSubmittingRating;

        public bool IsLoadingRating => IsSubmittingRating || IsLoadingDiscountRating;

        public string RatingString => DiscountDetailData.DiscountRating.ToRatingString();

        public List<DiscountDetailBranchData> BranchItems => DiscountDetailData.BranchList
            .OrderBy(i => i.Distance).ThenBy(i => i.DocumentId).ToList();

        public List<string> GalleryImages
        {
            get
            {
                var result = DiscountDetailData.GalleryImages.Select(i => i.FileName).ToList();

                if (!string.IsNullOrEmpty(ImageFileName))
                    result.Insert(0, ImageFileName);

                return result;
            }
        }

        public bool HasGalleryImages => GalleryImages.Count > 1;

        protected override void InitProperty()
        {
            ViewPage.Appearing += ViewPage_Appearing;
            ViewPage.Disappearing += ViewPage_Disappearing;
        }

        private void ViewPage_Appearing(object sender, EventArgs e)
        {
            AppMobileService.Locaion.PositionUpdated += MapLocation_Position;
        }

        private void ViewPage_Disappearing(object sender, EventArgs e)
        {
            AppMobileService.Locaion.PositionUpdated -= MapLocation_Position;
        }

        private void MapLocation_Position(object sender, EventArgs e)
        {
            CalculateDistance();
        }

        public void SetDiscount(DiscountDetailData discountDetailData)
        {
            DiscountDetailData = discountDetailData;

            InitUserRating();

            RefreshDiscountRating();

            CalculateDistance();
        }

        private void InitUserRating()
        {
            _userRating = (DiscountDetailData.PersonalRating?.Mark).GetValueOrDefault();
        }

        private void RefreshUserRating()
        {
            InitUserRating();
            OnPropertyChanged(nameof(UserRating));
        }

        private void CalculateDistance()
        {
            DiscountDetailData.BranchList.ForEach(i => i.CalculateDistance());
        }

        public void ShowOnMap_Click(object sender, EventArgs e)
        {
            var view = (View) sender;
            view.ClickAnimation(async () =>
            {
                var args = e as TappedEventArgs;
                var tag = args?.Parameter?.ToString();
                if (!string.IsNullOrEmpty(tag))
                {
                    AppData.Discount.ActiveMapPinId = tag;
                    await ViewPage.Navigation.PopToRootAsync(true);
                }
            });
        }

        public void TxtUrlAddress_Click(object sender, EventArgs e)
        {
            var view = (View) sender;
            view.ClickAnimation(() =>
            {
                var args = e as TappedEventArgs;
                var tag = args?.Parameter?.ToString();
                var link = tag.NormalizeLink();
                if (!string.IsNullOrEmpty(link))
                    Functions.SafeCall(() => Device.OpenUri(new Uri(link)));
            });
        }

        public void BtnCall_Click(object sender, EventArgs e)
        {
            var view = (View) sender;
            view.ClickAnimation(() =>
            {
                var args = e as TappedEventArgs;
                var tag = args?.Parameter?.ToString();
                var phoneNumber = tag.NormalizePhoneNumber();
                if (!string.IsNullOrEmpty(phoneNumber))
                    DependencyService.Get<IPhoneService>().DialNumber(phoneNumber);
            });
        }

        private async void UpdatePersonalRating()
        {
            IsSubmittingRating = true;

            var partnerId = DiscountDetailData.DocumentId;
            var personalRating = DiscountDetailData.PersonalRating ?? new PersonalRatingData();

            personalRating.PartnerId = partnerId;
            personalRating.DeviceId = DependencyService.Get<IPhoneService>().DeviceId;
            personalRating.Mark = UserRating;

            var isSuccess = await AppData.Discount.SendPersonalRating(personalRating);

            DiscountDetailData.PersonalRating = await AppData.Discount.Db.LoadPersonalRating(partnerId);
            RefreshUserRating();

            if (isSuccess)
                await RefreshDiscountRating();
            else
                await ViewPage.DisplayAlert(contentUI.TxtError, contentUI.MsgRatingSubmitError, contentUI.TxtOk);

            IsSubmittingRating = false;
        }

        private async Task RefreshDiscountRating()
        {
            IsLoadingDiscountRating = true;

            var partnerId = DiscountDetailData.DocumentId;

            await AppData.Discount.SyncRatingFor(partnerId);

            DiscountDetailData.DiscountRating = await AppData.Discount.Db.LoadDiscountRating(partnerId);
            OnPropertyChanged(nameof(IsDiscountRatingAvailable));
            OnPropertyChanged(nameof(RatingString));

            IsLoadingDiscountRating = false;
        }
    }
}
