using System;
using System.Collections.ObjectModel;
using ScnDiscounts.Control;
using ScnDiscounts.Control.Pages;
using ScnDiscounts.DependencyInterface;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models;
using ScnDiscounts.Models.Data;
using ScnDiscounts.Views.ContentUI;
using Xamarin.Forms;

namespace ScnDiscounts.ViewModels
{
    class DiscountDetailViewModel : BaseViewModel
    {
        private DiscountDetailContentUI contentUI
        {
            get { return (DiscountDetailContentUI)ContentUI; }
        }

        protected override void InitProperty()
        {
            ViewPage.Appearing += ViewPage_Appearing;
            ViewPage.Disappearing += ViewPage_Disappearing;
            //BranchItems = new ObservableCollection<BranchData>(AppData.Discount.BranchCollection);
        }

        void ViewPage_Appearing(object sender, EventArgs e)
        {
            AppMobileService.Locaion.PositionUpdated += MapLocation_Position;
        }

        void ViewPage_Disappearing(object sender, EventArgs e)
        {
            AppMobileService.Locaion.PositionUpdated -= MapLocation_Position;
        }

        private void MapLocation_Position(object sender, EventArgs e)
        {
            CalculateDistance();
        }

        public DiscountData currentDiscount;
        public void SetDiscount(DiscountData discountData)
        {
            currentDiscount = discountData;
            CalculateDistance();
        }

        private void CalculateDistance()
        {
            foreach (var item in BranchItems)
                item.CalculateDistance();
        }

        //------------------
        // Property
        //------------------
        #region ImgPhoto -
        public string ImgPhoto
        {
            get 
            {
                return AppData.Discount.PreviewImage; 
            }
        }
        #endregion

        #region ImgLogo -
        public string ImgLogo
        {
            get
            {
                return currentDiscount.Icon;
            }
        }
        #endregion

        #region DiscountPercent -
        public string DiscountPercent
        {
            get { return currentDiscount.DiscountPercent; }
        }
        #endregion

        #region Categories
        public int CategoriesCount
        {
            get { return currentDiscount.CategorieList.Count; }
        }

        public string CategoryIndexName(int index)
        {
            string name = "empty";
            if (index < CategoriesCount)
                name = currentDiscount.CategorieList[index].Name;
            return name.ToUpper();
        }

        public Color CategoryIndexColor(int index)
        {
            Color color = Color.Transparent;

            if (index < CategoriesCount)
            {
                if (CategoryHelper.CategoryList.ContainsKey(currentDiscount.CategorieList[index].TypeCode))
                {
                    var categoryParam = CategoryHelper.CategoryList[currentDiscount.CategorieList[index].TypeCode];
                    color = categoryParam.ColorTheme;
                }
            }

            return color;
        }
        #endregion

        #region CategoryName
        public string CategoryName
        {
            get { return currentDiscount.FirstCategoryName; }
        }
        #endregion

        #region CategoryColor
        public Color CategoryColor
        {
            get { return currentDiscount.FirstCategoryColor; }
        }
        #endregion

        #region NameCompany
        public string NameCompany
        {
            get { return currentDiscount.Name; }
        }
        #endregion

        #region Description -
        public string Description
        {
            get { return currentDiscount.Description; }
        }
        #endregion

        #region UrlAddress -
        public string UrlAddress
        {
            get { return currentDiscount.UrlAddress; }
        }
        #endregion

        #region PartnerAddress -
        public string PartnerAddress
        {
            get { return currentDiscount.Address; }
        }
        #endregion

        #region Distance -
        private string _distance = "0.0";
        public string Distance
        {
            get { return _distance; }
            set
            {
                _distance = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region BranchItems - property
        public ObservableCollection<BranchData> BranchItems
        {
            get { return AppData.Discount.BranchCollection; }
        }
        #endregion

        //------------------
        // Methods
        //------------------

        public void OnPhoneViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            string phoneNumber = e.Item as string;
            if (!String.IsNullOrWhiteSpace(phoneNumber))
                DependencyService.Get<IPhoneService>().DialNumber(phoneNumber, NameCompany);
            ((ListView)sender).SelectedItem = null;
        }
   
        async internal void txtShowOnMap_Click(object sender, EventArgs e)
        {
            AppData.Discount.ActiveMapPinId = (sender as LabelExtended).Tag;
            await ViewPage.Navigation.PopToRootAsync(true);
        }

        internal void txtUrlAddress_Click(object sender, EventArgs e)
        {
            LabelExtended label = sender as LabelExtended;
            if (!String.IsNullOrWhiteSpace(label.Text))
                Device.OpenUri(new Uri(label.Text));
        }

        internal void OnBranchViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}
