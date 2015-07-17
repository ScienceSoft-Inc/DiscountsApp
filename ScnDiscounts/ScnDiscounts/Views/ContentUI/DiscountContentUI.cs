using Xamarin.Forms;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models;

namespace ScnDiscounts.Views.ContentUI
{
    class DiscountContentUI : RootContentUI
    {
        public DiscountContentUI()
        {
            title = new LanguageStrings("Discounts", "Скидки", "Зніжкі");
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
