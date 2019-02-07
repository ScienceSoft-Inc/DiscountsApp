using ScnDiscounts.Models;

namespace ScnDiscounts.Views.ContentUI
{
    public class DiscountContentUI : RootContentUI
    {
        public DiscountContentUI()
        {
            title = new LanguageStrings("Discounts", "Скидки");
            _titleErrLoading = new LanguageStrings("Error loading data", "Ошибка загрузки данных");
            _txtErrServiceConnection = new LanguageStrings("Error connection to service", "Ошибка соединения с сервисом");
            _txtSortBy = new LanguageStrings("Sort", "Сортировка");
            _txtSortByName = new LanguageStrings("by name", "по имени");
            _txtSortByDistance = new LanguageStrings("by distance", "по удалённости");
            _txtEmptyList = new LanguageStrings("No results were found for the selected criteria", "По выбранным критериям ничего не найдено");
            _txtSearchByText = new LanguageStrings("Search", "Поиск");
        }

        public string Icon => "ic_tag.png";

        public string IconMore => "ic_menu_more.png";

        public string IconMoreFilter => "ic_menu_more_filter.png";

        public string IconSortByName => "ic_menu_sort_alphabetically.png";

        public string IconSortByDistance => "ic_menu_mylocation.png";

        private readonly LanguageStrings _titleErrLoading;
        public string TitleErrLoading => _titleErrLoading.Current;

        private readonly LanguageStrings _txtErrServiceConnection;
        public string TxtErrServiceConnection => _txtErrServiceConnection.Current;

        private readonly LanguageStrings _txtSortBy;
        public string TxtSortBy => _txtSortBy.Current;

        private readonly LanguageStrings _txtSortByName;
        public string TxtSortByName => _txtSortByName.Current;

        private readonly LanguageStrings _txtSortByDistance;
        public string TxtSortByDistance => _txtSortByDistance.Current;

        private readonly LanguageStrings _txtEmptyList;
        public string TxtEmptyList => _txtEmptyList.Current;


        private readonly LanguageStrings _txtSearchByText;
        public string TxtSearchByText => _txtSearchByText.Current;
    }
}
