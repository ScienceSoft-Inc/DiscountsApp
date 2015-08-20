using ScnDiscounts.Helpers;
using ScnDiscounts.Models;
using Xamarin.Forms;

namespace ScnDiscounts.Views.ContentUI
{
    public class MainContentUI : RootContentUI
    {
        public MainContentUI()
        {
            title = new LanguageStrings("Map", "Карта", "Мапа");
            _msgTitleNoGPS = new LanguageStrings("GPS is turned off", "GPS выключен", "GPS выключаны");
            _msgTxtNoGPS = new LanguageStrings(
                "Locating disabled. Turn on the GPS in your phone settings.", 
                "Определение местоположения отключено. Включите функцию GPS в настройках телефона.",
                "Вызначэнне месцазнаходжання адключана. Уключыце функцыю GPS у наладах тэлефона.");
        }

        public string IconMenuSideBar
        {
            get { return Device.OnPlatform("Icon/menu.png", "ic_menu.png", "Assets/Icon/menu.png"); }
        }

        public string IconLocation
        {
            get { return Device.OnPlatform("Icon/location.png", "ic_location.png", "Assets/Icon/location.png"); }
        }

        public string ImgLogo
        {
            get { return Device.OnPlatform("Image/img_logo.png", "img_logo.png", "Assets/Image/img_logo.png"); }
        }

        public string IconMap
        {
            get { return Device.OnPlatform("Icon/pin.png", "ic_pin.png", "Assets/Icon/pin.png"); }
        }

        public string IconFilter
        {
            get { return Device.OnPlatform("Icon/filter.png", "ic_filter.png", "Assets/Icon/filter.png"); }
        }

        private LanguageStrings _msgTitleNoGPS;
        public string MsgTitleNoGPS
        {
            get { return _msgTitleNoGPS.Current; }
        }

        private LanguageStrings _msgTxtNoGPS;
        public string MsgTxtNoGPS
        {
            get { return _msgTxtNoGPS.Current; }
        }

    }
}
