using ScnDiscounts.Helpers;
using ScnDiscounts.Models;
using Xamarin.Forms;

namespace ScnDiscounts.Views.ContentUI
{
    class DiscountDetailContentUI : RootContentUI
    {
        public DiscountDetailContentUI()
        {
            title = new LanguageStrings("Information", "Информация", "інфармацыя");
            _txtShowOnMap = new LanguageStrings("Show on map", "Показать на карте", "Паказаць на мапе");
        }

        private LanguageStrings _txtShowOnMap;
        public string TxtShowOnMap
        {
            get { return _txtShowOnMap.Current; }
        }

        public string IconPhone
        {
            get { return Device.OnPlatform("Icon/phone.png", "ic_menu_phone.png", "Assets/Icon/phone.png"); }
        }

        public string ImgPercentLabel
        {
            get { return Device.OnPlatform("Image/discount_label.png", "discount_label.png", "Assets/Image/discount_label.png"); }
        }
    }
}
