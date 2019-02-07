using ScnDiscounts.Helpers;
using ScnDiscounts.Models.Data;
using Xamarin.Forms;

namespace ScnDiscounts.Models
{
    public class FilterCategoryItem : NotifyPropertyChanged
    {
        public CategoryData CategoryData { get; }

        public string Name => CategoryData.Name;
        public Color Color => CategoryData.GetColorTheme();
        public ImageSource Icon => CategoryData.GetIconThemeImage(IsToggle);

        private bool _isToggle = true;
        public bool IsToggle
        {
            get => _isToggle;
            set 
            {
                if (_isToggle != value)
                {
                    _isToggle = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Icon));
                }
            }
        }

        public bool SkipRefreshing { get; set; }

        public FilterCategoryItem(CategoryData categoryData)
        {
            CategoryData = categoryData;
        }

        public void RefreshName()
        {
            OnPropertyChanged(nameof(Name));
        }
    }
}
