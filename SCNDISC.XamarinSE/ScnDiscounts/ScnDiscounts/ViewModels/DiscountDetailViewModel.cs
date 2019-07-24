using ScnDiscounts.DependencyInterface;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models;
using ScnDiscounts.Models.Data;
using ScnPage.Plugin.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace ScnDiscounts.ViewModels
{
    public class DiscountDetailViewModel : BaseViewModel
    {
        protected DiscountDetailData DiscountDetailData { get; set; }

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

            CalculateDistance();
        }

        private void CalculateDistance()
        {
            DiscountDetailData.BranchList.ForEach(i => i.CalculateDistance());
        }

        public string ImageFileName => DiscountDetailData.ImageFileName;

        public string DiscountPercent => DiscountDetailData.Persent;

        public string DiscountType => DiscountDetailData.DiscountType;

        public List<CategoryData> Categories => DiscountDetailData.CategoryList;

        public List<WebAddressData> WebAddresses => DiscountDetailData.WebAddresses;

        public string NameCompany => DiscountDetailData.Title;

        public string Description => DiscountDetailData.Description;

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
            var view = (View)sender;
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
    }
}
