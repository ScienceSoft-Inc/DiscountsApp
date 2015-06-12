using System;
using System.Collections.Generic;
using ScnDiscounts.Control.Pages;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models;
using ScnDiscounts.Views;
using ScnDiscounts.Views.ContentUI;
using Xamarin.Forms;

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
        }

        //------------------
        // Property
        //------------------

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
    }
}
