using System;
using System.Linq;
using System.Collections.Generic;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models;
using ScnDiscounts.Views;
using ScnDiscounts.Views.ContentUI;
using Xamarin.Forms;
using ScnDiscounts.Control;
using ScnPage.Plugin.Forms;
using ScnDiscounts.Helpers;

namespace ScnDiscounts.ViewModels
{
    class SettingsViewModel : BaseViewModel
    {
        private SettingsContentUI contentUI
        {
            get { return (SettingsContentUI)ContentUI; }
        }

        protected override void InitProperty()
        {
            ViewPage.Disappearing += ViewPage_Disappearing;

            _currLanguageName = LanguageHelper.LanguageList[AppParameters.Config.SystemLang];
            _mapName = MapTile.TileSourceList[AppParameters.Config.MapSource];
        }

        void ViewPage_Disappearing(object sender, EventArgs e)
        {
            AppParameters.Config.SaveValue();
        }

        //Call updates all properties. It's need for UI change when user changes system language.
        private void UpdateProperty()
        {
            OnPropertyChanged("Title");
            OnPropertyChanged("CurrLanguageTitle");
            OnPropertyChanged("CurrLanguageName");
            OnPropertyChanged("MapTitle");
            OnPropertyChanged("MapName");
        }

        //------------------
        // Property
        //------------------

        #region Title
        public string Title
        {
            get { return contentUI.Title; }
        }
        #endregion

        #region CurrLanguageTitle - Current language title
        public string CurrLanguageTitle
        {
            get { return contentUI.TxtLanguage; }
        }
        #endregion

        #region CurrLanguageName - Current language name
        private string _currLanguageName;
        public string CurrLanguageName
        {
            get { return _currLanguageName; }
            set
            {
                _currLanguageName = value;
               
                UpdateProperty();
            }
        }
        #endregion

        #region MapTitle - map
        public string MapTitle
        {
            get { return contentUI.TxtMap; }
        }
        #endregion

        #region MapName - map name
        private string _mapName;
        public string MapName
        {
            get { return _mapName; }
            set
            {
                _mapName = value;

                UpdateProperty();
            }
        }
        #endregion


        async public void LangSetting_Click(object sender, EventArgs e)
        {
            var list = new List<string>(LanguageHelper.LanguageList.Values);

            if (Device.OS == TargetPlatform.WinPhone)
            {
                var langPage = new SelectionPage(contentUI.TxtLanguageSel, list);

                langPage.SelList.ItemSelected += (ss, ee) =>
                {
                    var selLang = ee.SelectedItem.ToString();

                    if (!String.IsNullOrEmpty(selLang))
                    {
                        AppParameters.Config.SystemLang = LanguageHelper.LangNameToEnum(selLang);
                        CurrLanguageName = selLang;
                    }
                };

                await ViewPage.Navigation.PushModalAsync(langPage, true);
            }
            else
            {
                var selLang = await ViewPage.DisplayActionSheet(contentUI.TxtLanguageSel, null, null, list.ToArray());

                if (!String.IsNullOrEmpty(selLang))
                {
                    AppParameters.Config.SystemLang = LanguageHelper.LangNameToEnum(selLang);
                    CurrLanguageName = selLang;
                }
            }
        }

        async internal void MapSetting_Click(object sender, EventArgs e)
        {
            var list = new List<string>(MapTile.TileSourceList.Values);
            if (Device.OS == TargetPlatform.WinPhone)
            {
                var mapPage = new SelectionPage(contentUI.TxtMapSel, list);

                mapPage.SelList.ItemSelected += (ss, ee) =>
                {
                    var selMap = ee.SelectedItem.ToString();
                    SetMapSettings(selMap);
                };

                await ViewPage.Navigation.PushModalAsync(mapPage, true);
            }
            else
            {
                var selMap = await ViewPage.DisplayActionSheet(contentUI.TxtMapSel, null, null, list.ToArray());
                SetMapSettings(selMap);
            }
        }

        private void SetMapSettings(string mapName)
        {
            if (!String.IsNullOrEmpty(mapName))
            {
                MapTile.TileSourceEnum result = MapTile.TileSourceEnum.tsNative;
                foreach (var item in MapTile.TileSourceList)
                {
                    if (String.Compare(item.Value, mapName, StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        result = item.Key;
                        break;
                    }
                }
                AppParameters.Config.MapSource = result;
                MapName = mapName;
            }
        }
    }
}
