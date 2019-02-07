using ScnDiscounts.Helpers;
using ScnDiscounts.Models.Data;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace ScnDiscounts.Models
{
    public static class AppParameters
    {
        public class ConfigContainer
        {
            private const string SystemLangParamName = "SystemLang";
            public LanguageHelper.LangTypeEnum SystemLang { get; set; } = LanguageHelper.LangTypeEnum.ltEn;

            private const string CategoryFilterPrefix = "CategotyFilter_";
            public List<FilterCategoryItem> FilterCategoryList { get; set; }

            private const string SortingParamName = "Sorting";
            public SortingEnum Sorting { get; set; } = SortingEnum.ByDistance;

            private const string FeedbackNameParamName = "FeedbackName";
            public string FeedbackName { get; set; }

            public async void SaveValues()
            {
                Application.Current.Properties[SystemLangParamName] = (int) SystemLang;
                Application.Current.Properties[SortingParamName] = (int) Sorting;
                Application.Current.Properties[FeedbackNameParamName] = FeedbackName;

                await Application.Current.SavePropertiesAsync();
            }

            public void LoadValues()
            {
                Functions.SafeCall(() =>
                {
                    if (Application.Current.Properties.ContainsKey(SystemLangParamName))
                    {
                        SystemLang = (LanguageHelper.LangTypeEnum)Application.Current.Properties[SystemLangParamName];
                    }
                    else
                    {
                        var langName = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                        SystemLang = LanguageHelper.LangCodeToEnum(langName);
                    }
                });

                Functions.SafeCall(() =>
                {
                    if (Application.Current.Properties.ContainsKey(SortingParamName))
                        Sorting = (SortingEnum) Application.Current.Properties[SortingParamName];
                });

                Functions.SafeCall(() =>
                {
                    if (Application.Current.Properties.ContainsKey(FeedbackNameParamName))
                        FeedbackName = (string) Application.Current.Properties[FeedbackNameParamName];
                });
            }

            public async void SaveCategoryFilter(FilterCategoryItem filterCategoryItem)
            {
                var paramName = CategoryFilterPrefix + filterCategoryItem.CategoryData.DocumentId;
                Application.Current.Properties[paramName] = filterCategoryItem.IsToggle;

                await Application.Current.SavePropertiesAsync();
            }

            public void LoadCategoryFilter(List<CategoryData> categories)
            {
                FilterCategoryList = categories.Select(i=>
                {
                    var paramName = CategoryFilterPrefix + i.DocumentId;

                    return new FilterCategoryItem(i)
                    {
                        IsToggle = !Application.Current.Properties.ContainsKey(paramName) ||
                                   (bool) Application.Current.Properties[paramName]
                    };
                }).ToList();
            }
        }

        public static ConfigContainer Config = new ConfigContainer();
    }
}
