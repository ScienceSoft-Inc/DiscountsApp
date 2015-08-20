using Xamarin.Forms;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models;

namespace ScnDiscounts.Views.ContentUI
{
    class SettingsContentUI : RootContentUI
    {
        public SettingsContentUI()
        {
            title = new LanguageStrings("Settings", "Настройки", "Налады");

            _txtLanguage = new LanguageStrings("Language", "Язык", "Мова");
            _txtLanguageSel = new LanguageStrings("Choose language", "Выберите язык", "Выбірыце мову");

            _txtMap = new LanguageStrings("Map", "Карта", "Мапа");
            _txtMapSel = new LanguageStrings("Choose map", "Выберите карту", "Выбірыце мапу");
        }

        public string Icon
        {
            get { return Device.OnPlatform("Icon/settings.png", "ic_settings.png", "Assets/Icon/settings.png"); }
        }

        private LanguageStrings _txtLanguageSel;
        public string TxtLanguageSel 
        {
            get { return _txtLanguageSel.Current; }
        }

        private LanguageStrings _txtLanguage;
        public string TxtLanguage
        {
            get { return _txtLanguage.Current; }
        }

        private LanguageStrings _txtMap;
        public string TxtMap
        {
            get { return _txtMap.Current; }
        }

        private LanguageStrings _txtMapSel;
        public string TxtMapSel
        {
            get { return _txtMapSel.Current; }
        }
    }
}
