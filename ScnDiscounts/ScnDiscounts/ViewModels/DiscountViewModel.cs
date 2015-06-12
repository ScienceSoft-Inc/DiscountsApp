using System;
using System.Collections.ObjectModel;
using ScnDiscounts.Control.Pages;
using ScnDiscounts.Models;
using ScnDiscounts.Models.Data;
using ScnDiscounts.Views;
using Xamarin.Forms;

namespace ScnDiscounts.ViewModels
{
    class DiscountViewModel : BaseViewModel
    {
        protected override void InitProperty()
        {
            ViewPage.Appearing += ViewPage_Appearing;
            _discountItems = new ObservableCollection<DiscountData>(AppData.Discount.DiscountCollection);
        }

        void ViewPage_Appearing(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
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

        public async void OnDiscountItemTapped(object sender, ItemTappedEventArgs e)
        {
            DiscountData discountData = e.Item as DiscountData;
            BaseContentPage page = null;

            IsLoadActivity = true;
            try
            {
                //Load branch
                await AppData.Discount.LoadBranchList(discountData);
                page = new DiscountDetailPage(discountData);
            }
            finally
            {
                IsLoadActivity = false;
            }

            if (page != null)
                await ViewPage.Navigation.PushAsync(page, true);

            ((ListView)sender).SelectedItem = null;
        }
    }
}
