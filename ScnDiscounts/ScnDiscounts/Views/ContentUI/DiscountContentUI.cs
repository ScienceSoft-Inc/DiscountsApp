using ScnDiscounts.Helpers;
using Xamarin.Forms;

namespace ScnDiscounts.Views.ContentUI
{
    class DiscountContentUI : RootContentUI
    {
        public DiscountContentUI()
        {
            _title = new PropertyLang("Discounts", "Скидки", "Зніжкі");
        }

        public string Icon
        {
            get { return Device.OnPlatform("Icon/tag.png", "ic_tag.png", "Assets/Icon/tag.png"); }
        }

        public string ImgListBorder
        {
            get { return Device.OnPlatform("Image/list_border.png", "list_border.png", "Assets/Image/list_border.png"); }
        }
    }
}
