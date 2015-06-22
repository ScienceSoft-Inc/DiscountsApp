using ScnDiscounts.Helpers;
using Xamarin.Forms;

namespace ScnDiscounts.Views.ContentUI
{
    class SettingsContentUI : RootContentUI
    {
        public SettingsContentUI()
        {
            _title = new PropertyLang("Settings", "Настройки", "Налады");

            _txtLanguage = new PropertyLang("Language", "Язык", "Мова");
            _txtLanguageSel = new PropertyLang("Choose language", "Выберите язык", "Выберыце мову");

            _txtMap = new PropertyLang("Map", "Карта");
            _txtMapSel = new PropertyLang("Choose map", "Выберите карту");
        }

        public string Icon
        {
            get { return Device.OnPlatform("Icon/Settings.png", "ic_settings.png", "Assets/Icon/Settings.png"); }
        }

        private PropertyLang _txtLanguageSel;
        public string TxtLanguageSel 
        {
            get { return _txtLanguageSel.ActualValue(); }
        }

        private PropertyLang _txtLanguage;
        public string TxtLanguage
        {
            get { return _txtLanguage.ActualValue(); }
        }

        private PropertyLang _txtMap;
        public string TxtMap
        {
            get { return _txtMap.ActualValue(); }
        }

        private PropertyLang _txtMapSel;
        public string TxtMapSel
        {
            get { return _txtMapSel.ActualValue(); }
        }
    }
}
