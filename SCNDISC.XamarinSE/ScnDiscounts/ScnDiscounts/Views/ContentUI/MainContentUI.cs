using ScnDiscounts.Models;

namespace ScnDiscounts.Views.ContentUI
{
    public class MainContentUI : RootContentUI
    {
        public MainContentUI()
        {
            title = new LanguageStrings("Map", "Карта");

            _msgTitleNoGps = new LanguageStrings("GPS is turned off", "GPS выключен");

            _msgTxtNoGps = new LanguageStrings(
                "Locating disabled. Turn on the GPS in phone settings.", 
                "Определение местоположения отключено. Включите функцию GPS в настройках телефона.");

            _msgTxtDeniedGps = new LanguageStrings(
                "Locating disabled. Turn on the GPS in app settings.",
                "Определение местоположения отключено. Включите функцию GPS в настройках приложения.");

            _txtPhoneSettings = new LanguageStrings("Phone settings", "Настройки телефона");

            _txtCategories = new LanguageStrings("Discount categories", "Категории скидок");
        }

        public string IconMenuSideBar => "ic_menu.png";

        public string IconLocation => "ic_location.png";

        public string IconMap => "ic_pin.png";

        public string IconFilter => "ic_filter.png";

        private readonly LanguageStrings _msgTitleNoGps;
        public string MsgTitleNoGps => _msgTitleNoGps.Current;

        private readonly LanguageStrings _msgTxtNoGps;
        public string MsgTxtNoGps => _msgTxtNoGps.Current;

        private readonly LanguageStrings _msgTxtDeniedGps;
        public string MsgTxtDeniedGps => _msgTxtDeniedGps.Current;

        private readonly LanguageStrings _txtCategories;
        public string TxtCategories => _txtCategories.Current;

        private readonly LanguageStrings _txtPhoneSettings;
        public string TxtPhoneSettings => _txtPhoneSettings.Current;
    }
}
