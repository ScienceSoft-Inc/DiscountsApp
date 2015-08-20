using System;
using ScnDiscounts.Helpers;
using Xamarin.Forms;
using ScnDiscounts.Control;
using System.Collections.Generic;
using System.Globalization;

namespace ScnDiscounts.Models
{
    static public class AppParameters
    {
        public class ConfigContainer
        {
            //system (phone) language
            private string SystemLangParamName = "SystemLang";
            private LanguageHelper.LangTypeEnum _systemLang = LanguageHelper.LangTypeEnum.ltEn;
            public LanguageHelper.LangTypeEnum SystemLang 
            { 
                get { return _systemLang; }
                set { _systemLang = value; } 
            }

            //map source
            private string MapSourceParamName = "MapSource";
            private MapTile.TileSourceEnum _mapSource = MapTile.TileSourceEnum.tsNative;
            public MapTile.TileSourceEnum MapSource
            {
                get { return _mapSource; }
                set { _mapSource = value; }
            }

            //category filter
            private string CategoryFilterPrefix = "CategotyFilter_";
            private List<FilterCategoryItem> _filterCategoryList = new List<FilterCategoryItem>();
            public List<FilterCategoryItem> FilterCategoryList
            {
                get { return _filterCategoryList; }
            }

            //data modification hash
            private string DataMidificationHashParamName = "MapSource";
            private string _dataMidificationHash = String.Empty;
            public string DataMidificationHash
            {
                get { return _dataMidificationHash; }
                set { _dataMidificationHash = value; }
            }

            public void Init()
            {
                LoadValue();
            }

            public void SaveValue()
            {
                Application.Current.Properties[SystemLangParamName] = SystemLang.ToString();
                Application.Current.Properties[MapSourceParamName] = MapSource.ToString();
                Application.Current.Properties[DataMidificationHashParamName] = DataMidificationHash;

                SaveCategoryFilter();
            }

            public void LoadValue()
            {
                if (Application.Current.Properties.ContainsKey(SystemLangParamName))
                {
                    string lang = Application.Current.Properties[SystemLangParamName] as string;
                    LanguageHelper.LangTypeEnum tmpLang;
                    if (Enum.TryParse(lang, out tmpLang))
                        SystemLang = tmpLang;
                }
                else
                {
                    var langNameISO = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                    SystemLang = LanguageHelper.LangCodeToEnum(langNameISO);
                }

                if (Application.Current.Properties.ContainsKey(MapSourceParamName))
                {
                    string map = Application.Current.Properties[MapSourceParamName] as string;
                    MapTile.TileSourceEnum tmpMap;
                    if (Enum.TryParse(map, out tmpMap))
                        MapSource = tmpMap;
                }

                if (Application.Current.Properties.ContainsKey(DataMidificationHashParamName))
                    DataMidificationHash = Application.Current.Properties[DataMidificationHashParamName] as string;

                LoadCategoryFilter();
            }

            private void SaveCategoryFilter()
            {
                foreach (var item in _filterCategoryList)
                    Application.Current.Properties[item.ParamName] = item.IsToggle.ToString();
            }

            private void LoadCategoryFilter()
            {
                _filterCategoryList.Clear();

                foreach (var item in CategoryHelper.CategoryList)
                {
                    var filterItem = new FilterCategoryItem 
                    {
                        Id = item.Key,
                        ParamName = CategoryFilterPrefix + item.Value.DefaultName,

                        IsToggle = true,
                    };

                    if (Application.Current.Properties.ContainsKey(filterItem.ParamName))
                    {
                        string value = Application.Current.Properties[filterItem.ParamName] as string;
                        bool resValue = true;
                        if (bool.TryParse(value, out resValue))
                            filterItem.IsToggle = resValue;
                    }

                    _filterCategoryList.Add(filterItem);
                }
            }

            
        }

        static public ConfigContainer Config = new ConfigContainer();
    }
}
