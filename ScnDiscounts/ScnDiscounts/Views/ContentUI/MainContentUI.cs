using ScnDiscounts.Helpers;
using Xamarin.Forms;

namespace ScnDiscounts.Views.ContentUI
{
    public class MainContentUI : RootContentUI
    {
        public MainContentUI()
        {
            _title = new PropertyLang("Map", "Карта", "Мапа");
            _msgTitleNoGPS = new PropertyLang("GPS is turned off", "GPS выключен");
            _msgTxtNoGPS = new PropertyLang("Locating disabled. Turn on the GPS in your phone settings.", "Определение местоположения отключено. Включите функцию GPS в настройках телефона.");
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

        private PropertyLang _msgTitleNoGPS;
        public string MsgTitleNoGPS
        {
            get { return _msgTitleNoGPS.ActualValue(); }
        }

        private PropertyLang _msgTxtNoGPS;
        public string MsgTxtNoGPS
        {
            get { return _msgTxtNoGPS.ActualValue(); }
        }

    }
}
