using System;
using System.Collections.ObjectModel;
using System.Linq;
using ScnDiscounts.Control;
using ScnDiscounts.Control.Pages;
using ScnDiscounts.DependencyInterface;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models;
using ScnDiscounts.Models.Data;
using ScnDiscounts.Views.ContentUI;
using Xamarin.Forms;
using System.Threading.Tasks;
using ScnDiscounts.Views;

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
            _branchItems = new ObservableCollection<BranchData>();

            ViewPage.Appearing += ViewPage_Appearing;
            ViewPage.Appearing += InitListView_Appearing;
            ViewPage.Disappearing += ViewPage_Disappearing;
        }

        private int previewItemsCount = 2;
        void ViewPage_Appearing(object sender, EventArgs e)
        {
            AppMobileService.Locaion.PositionUpdated += MapLocation_Position;
        }

        async void InitListView_Appearing(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.Android)
            {
                (ViewPage as DiscountDetailPage).InitBranchListView();

                await Task.Delay(1000); //waiting init listview control

                var skipCount = (currentDiscount.BranchList.Count > previewItemsCount) ? previewItemsCount : 0;
                if (skipCount > 0)
                {
                    int count = currentDiscount.BranchList.Count - skipCount;
                    currentDiscount.BranchList.Skip(skipCount).Take(count).ToList().ForEach(BranchItems.Add);
                    OnPropertyChanged("BranchItemsCount");

                    CalculateDistance();
                }
            }
            else if (Device.OS == TargetPlatform.WinPhone)
                (ViewPage as DiscountDetailPage).InitBranchListView();
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

            if (Device.OS == TargetPlatform.Android)
            {
                var count = (currentDiscount.BranchList.Count > previewItemsCount) ? previewItemsCount : currentDiscount.BranchList.Count;
                currentDiscount.BranchList.Take(count).ToList().ForEach(BranchItems.Add);
            }
            else if ((Device.OS == TargetPlatform.WinPhone) || (Device.OS == TargetPlatform.iOS))
                BranchItems = currentDiscount.BranchList;

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
                return currentDiscount.Image; 
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
        private ObservableCollection<BranchData> _branchItems;
        public ObservableCollection<BranchData> BranchItems
        {
            get { return _branchItems; }
            set
            {
                _branchItems = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public int BranchItemsCount
        {
            get { return BranchItems.Count; }
        }

        //------------------
        // Command
        //------------------

        #region CallCommand
        private Command callCommand;
        public Command CallCommand
        {
            get
            {
                return callCommand ??
                    (callCommand = new Command(ExecuteCall));
            }
        }

        private void ExecuteCall(object callNumber)
        {
            string phoneNumber = callNumber as String;
            if (!String.IsNullOrWhiteSpace(phoneNumber))
                DependencyService.Get<IPhoneService>().DialNumber(phoneNumber, NameCompany);
        }
        #endregion
        
        //------------------
        // Methods
        //------------------
        async internal void txtShowOnMap_Click(object sender, EventArgs e)
        {
            AppData.Discount.ActiveMapPinId = (sender as LabelExtended).Tag;
            Console.WriteLine(AppData.Discount.ActiveMapPinId);

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

        internal void BranchView_AnimationFinished(object sender, EventArgs e)
        {
            //IsLoadBusy = false;
        }

        internal void BtnCall_Click(object sender, EventArgs e)
        {
            if (!(sender is BorderBox))
                return;

            string phoneNumber = (sender as BorderBox).Tag;
            int index = phoneNumber.IndexOfAny("0123456789".ToCharArray());
            if (index > 0)
                phoneNumber = phoneNumber.Remove(0, index - 1);
            phoneNumber = phoneNumber.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");
            if (!String.IsNullOrWhiteSpace(phoneNumber))
                DependencyService.Get<IPhoneService>().DialNumber(phoneNumber, NameCompany);
        }
    }
}
