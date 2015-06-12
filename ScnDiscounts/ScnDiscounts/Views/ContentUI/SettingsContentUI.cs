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
    }
}
