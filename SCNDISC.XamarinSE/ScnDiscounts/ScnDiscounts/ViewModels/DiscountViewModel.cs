using ScnDiscounts.Helpers;
using ScnDiscounts.Models;
using ScnDiscounts.Models.Data;
using ScnDiscounts.ValueConverter;
using ScnDiscounts.Views.ContentUI;
using ScnPage.Plugin.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace ScnDiscounts.ViewModels
{
    public class DiscountViewModel : BaseViewModel
    {
        private DiscountContentUI contentUI => (DiscountContentUI) ContentUI;

        public MainViewModel MainViewModel => (MainViewModel) App.RootPage.ViewModel;

        protected override void InitProperty()
        {
            ViewPage.Appearing += ViewPage_Appearing;
            ViewPage.Disappearing += ViewPage_Disappearing;

            RefreshData();
        }

        private void ViewPage_Appearing(object sender, EventArgs e)
        {
            FilterCategorySwitched += MainViewModel.SwitchFilter_Toggled;
            MainViewModel.DataRefreshing += Filter_DataRefreshing;
        }

        private void ViewPage_Disappearing(object sender, EventArgs e)
        {
            FilterCategorySwitched -= MainViewModel.SwitchFilter_Toggled;
            MainViewModel.DataRefreshing -= Filter_DataRefreshing;
        }

        private void Filter_DataRefreshing(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(IsCustomFiltering));

            RefreshData();
        }

        private bool _isSortByName = AppParameters.Config.Sorting == SortingEnum.ByName;

        public bool IsSortByName
        {
            get => _isSortByName;
            set
            {
                if (_isSortByName != value)
                {
                    _isSortByName = value;
                    _isSortByDistance = !value;

                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsSortByDistance));

                    RefreshData();

                    AppParameters.Config.Sorting = IsSortByName ? SortingEnum.ByName : SortingEnum.ByDistance;
                    AppParameters.Config.SaveValues();
                }
            }
        }

        private bool _isSortByDistance = AppParameters.Config.Sorting == SortingEnum.ByDistance;

        public bool IsSortByDistance
        {
            get => _isSortByDistance;
            set
            {
                if (_isSortByDistance != value)
                {
                    _isSortByDistance = value;
                    _isSortByName = !value;

                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsSortByName));

                    RefreshData();

                    AppParameters.Config.Sorting = IsSortByName ? SortingEnum.ByName : SortingEnum.ByDistance;
                    AppParameters.Config.SaveValues();
                }
            }
        }

        public bool IsCustomFiltering => !MainViewModel.IsAllFiltersSelected;

        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;

                    OnPropertyChanged();

                    RefreshData();
                }
            }
        }

        private List<DiscountData> _discountItems;
        public List<DiscountData> DiscountItems
        {
            get => _discountItems;
            set
            {
                _discountItems = value;
                OnPropertyChanged();

                OnPropertyChanged(nameof(HasDiscountItems));
                OnPropertyChanged(nameof(HasNoDiscountItems));
            }
        }

        public bool HasDiscountItems => DiscountItems?.Count > 0;

        public bool HasNoDiscountItems => !HasDiscountItems;

        private void RefreshData()
        {
            var partnerNameComparer = new PartnerNameComparer();

            var filteredCategories = MainViewModel.FilterCategoryList.Where(i => i.IsToggle)
                .Select(i => i.CategoryData.Id).ToArray();

            var items = AppData.Discount.DiscountCollection.Where(i =>
                i.CategoryList.Count == 0 || i.CategoryList.Any(j => filteredCategories.Contains(j.Id)));

            var searchText = SearchText.SafeTrim();
            if (!string.IsNullOrEmpty(searchText))
            {
                var searchWords = searchText.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                items = items.Where(i =>
                    i.Name != null &&
                    searchWords.Any(j => i.Name.IndexOf(j, StringComparison.OrdinalIgnoreCase) > -1) ||
                    i.Description != null &&
                    searchWords.Any(j => i.Description.IndexOf(j, StringComparison.OrdinalIgnoreCase) > -1));
            }

            if (IsSortByName)
                items = items.OrderBy(i => i.Name, partnerNameComparer);
            else if (IsSortByDistance)
                items = LocationHelper.IsCurrentLocationAvailable
                    ? items.OrderBy(i =>
                            AppData.Discount.MapPinCollection.Where(j => j.PartnerId == i.DocumentId)
                                .Select(j => j.DistanceValue).DefaultIfEmpty().Min())
                        .ThenBy(i => i.Name, partnerNameComparer)
                    : items.OrderBy(i => i.Name, partnerNameComparer);

            DiscountItems = new List<DiscountData>(items);
        }

        public async void OnDiscountItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is DiscountData discountData)
                await ViewPage.OpenDetailPage(discountData.DocumentId);
        }

        public void SortByName_Tap(object sender, EventArgs e)
        {
            IsSortByName = !IsSortByName;
        }

        public void SortByDistance_Tap(object sender, EventArgs e)
        {
            IsSortByDistance = !IsSortByDistance;
        }

        public event EventHandler<ToggledEventArgs> FilterCategorySwitched;

        public void OnFilterCategorySwitched(object sender, ToggledEventArgs e)
        {
            FilterCategorySwitched?.Invoke(sender, e);
        }
    }
}
