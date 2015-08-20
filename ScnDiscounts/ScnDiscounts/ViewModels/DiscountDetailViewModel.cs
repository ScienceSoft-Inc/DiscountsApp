using System;
using System.Collections.ObjectModel;
using System.Linq;
using ScnDiscounts.Control;
using ScnDiscounts.DependencyInterface;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models;
using ScnDiscounts.Models.Data;
using ScnDiscounts.Views.ContentUI;
using Xamarin.Forms;
using System.Threading.Tasks;
using ScnDiscounts.Views;
using ScnPage.Plugin.Forms;
using ScnViewGestures.Plugin.Forms;

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
            _branchItems = new ObservableCollection<DiscountDetailBranchData>();

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

                var skipCount = (discountDetailData.BranchList.Count > previewItemsCount) ? previewItemsCount : 0;
                if (skipCount > 0)
                {
                    int count = discountDetailData.BranchList.Count - skipCount;
                    discountDetailData.BranchList.Skip(skipCount).Take(count).ToList().ForEach(BranchItems.Add);
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

        public void SetDiscount(string discountID)
        {
            discountDetailData = AppData.Discount.DB.LoadDiscountDetail(discountID);
            if (Device.OS == TargetPlatform.Android)
            {
                var count = (discountDetailData.BranchList.Count > previewItemsCount) ? previewItemsCount : discountDetailData.BranchList.Count;
                discountDetailData.BranchList.Take(count).ToList().ForEach(BranchItems.Add);
            }
            else if ((Device.OS == TargetPlatform.WinPhone) || (Device.OS == TargetPlatform.iOS))
                BranchItems = new ObservableCollection<DiscountDetailBranchData>(discountDetailData.BranchList);

            CalculateDistance();
        }

        private void CalculateDistance()
        {
            foreach (var item in BranchItems)
                item.CalculateDistance();
        }

        private DiscountDetailData discountDetailData;

        //------------------
        // Property
        //------------------
        #region ImageFileName -
        public string ImageFileName
        {
            get 
            {
                return discountDetailData.ImageFileName; 
            }
        }
        #endregion

        #region LogoFileName -
        public string LogoFileName
        {
            get
            {
                return discountDetailData.LogoFileName;
            }
        }
        #endregion

        #region DiscountPercent -
        public string DiscountPercent
        {
            get { return  discountDetailData.Persent; }
        }
        #endregion

        #region Categories
        public int CategoriesCount
        {
            get { return  discountDetailData.CategorieList.Count; }
        }

        public string CategoryIndexName(int index)
        {
            string name = "empty";
            if (index < CategoriesCount)
                name = CategoryHelper.CategoryList[discountDetailData.CategorieList[index].TypeCode].Name;
            return name.ToUpper();
        }

        public Color CategoryIndexColor(int index)
        {
            Color color = Color.Transparent;

            if (index < CategoriesCount)
            {
                if (CategoryHelper.CategoryList.ContainsKey(discountDetailData.CategorieList[index].TypeCode))
                {
                    var categoryParam = CategoryHelper.CategoryList[discountDetailData.CategorieList[index].TypeCode];
                    color = categoryParam.ColorTheme;
                }
            }

            return color;
        }
        #endregion

        #region NameCompany
        public string NameCompany
        {
            get { return discountDetailData.Title; }
        }
        #endregion

        #region Description -
        public string Description
        {
            get { return discountDetailData.Description; }
        }
        #endregion

        #region UrlAddress -
        public string UrlAddress
        {
            get { return discountDetailData.UrlAddress; }
        }
        #endregion

        #region BranchItems - property
        private ObservableCollection<DiscountDetailBranchData> _branchItems;
        public ObservableCollection<DiscountDetailBranchData> BranchItems
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
            get 
            {
                //gag for WP, beause label doesn't expand automatically in this case
                if ((Device.OS == TargetPlatform.WinPhone) && (BranchItems.Count == 1))
                    return BranchItems.Count + 1; // +1 supplementary space for label
                
                return BranchItems.Count; 
            }
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
        internal void ShowOnMap_Click(object sender, EventArgs e)
        {
            var parentElement = (sender as VisualElement).Parent;
            if (!(parentElement is ViewGestures))
                return;

            AppData.Discount.ActiveMapPinId = (parentElement as ViewGestures).Tag;

            var page = ViewPage.Navigation.NavigationStack[1];
            if (page != null)
            {
                if (page is BaseContentPage)
                    (page as BaseContentPage).OnDisposing();

                ViewPage.Navigation.RemovePage(page);
            }
            ViewPage.Navigation.PopAsync(true); 
            //ViewPage.Navigation.PopToRootAsync(true); - BUG on WP
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
