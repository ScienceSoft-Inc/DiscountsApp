using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using System.Threading.Tasks;
using ScnDiscounts.Views.Styles;
using ScnDiscounts.Control.Pages;
using ScnDiscounts.Models;
using ScnDiscounts.Models.Data;
using ScnDiscounts.Views;

namespace ScnDiscounts.ViewModels
{
    class DiscountViewModel : BaseViewModel
    {
        protected override void InitProperty()
        {
            ViewPage.Appearing += AnimateListView_Appearing;
            int count = (AppData.Discount.DiscountCollection.Count > previewItemsCount) ? previewItemsCount : AppData.Discount.DiscountCollection.Count;
            _discountItems = new ObservableCollection<DiscountData>(AppData.Discount.DiscountCollection.Take(count));
        }

        private int previewItemsCount = 7; //count items for preview fast render
        private bool isFullInit = false;
        async void AnimateListView_Appearing(object sender, EventArgs e)
        {
            IsLoadBusy = true;
            ViewPage.Appearing -= AnimateListView_Appearing;

            await Task.Delay(300); //waiting appearing page
            await (ViewPage as DiscountPage).DiscountListView.ShowAnimation();

            if (!isFullInit)
            {
                await Task.Delay(100); //waiting full render after animation
                var skipCount = (AppData.Discount.DiscountCollection.Count > previewItemsCount) ? previewItemsCount : 0;

                if (skipCount > 0)
                {
                    int count = AppData.Discount.DiscountCollection.Count - skipCount;
                    AppData.Discount.DiscountCollection.Skip(skipCount).Take(count).ToList().ForEach(DiscountItems.Add);
                    OnPropertyChanged("DiscountItemsCount");
                }

                isFullInit = true;
            }
        }

        //------------------
        // Property
        //------------------

        #region DiscountItems - property
        private ObservableCollection<DiscountData> _discountItems;
        public ObservableCollection<DiscountData> DiscountItems
        {
            get { return _discountItems; }
            set
            {
                _discountItems = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public int DiscountItemsCount
        {
            get { return DiscountItems.Count; }
        }

        public async void OnDiscountItemTapped(object sender, ItemTappedEventArgs e)
        {
            DiscountData discountData = e.Item as DiscountData;
            ((ListView)sender).SelectedItem = null;

            ViewPage.Appearing += AnimateListView_Appearing;
            if (Device.OS != TargetPlatform.Android)
                (ViewPage as DiscountPage).DiscountListView.HideAnimation();

            try
            {
                IsLoadActivity = true;
                await AppData.Discount.LoadBranchList(discountData);
            }
            finally
            {
                IsLoadActivity = false;
            }

            await ViewPage.Navigation.PushAsync(new DiscountDetailPage(discountData), true);
        }

        internal void BranchView_AnimationFinished(object sender, EventArgs e)
        {
            IsLoadBusy = false;
        }
    }
}
