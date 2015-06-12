using ScnDiscounts.Control.Pages;
using ScnDiscounts.Helpers;
using Xamarin.Forms;

namespace ScnDiscounts.Views.ContentUI
{
    public class RootContentUI : BaseContentUI
    {
        public RootContentUI()
        {
            txtCancel = new PropertyLang("Cancel", "Отменить", "Адмяніць");
            txtOk = new PropertyLang("OK", "ОК", "Добра");

            txtDistanceScaleCaption = new PropertyLang("km", "км", "км");
            txtDiscount = new PropertyLang("Discount", "Скидка", "Зніжка");

        }

        private PropertyLang txtCancel;
        public string TxtCancel
        {
            get { return txtCancel.ActualValue(); }
        }

        private PropertyLang txtOk;
        public string TxtOk
        {
            get { return txtOk.ActualValue(); }
        }
        
        public string IconBack
        {
            get { return Device.OnPlatform("Icon/back.png", "ic_menu_back.png", "Assets/Icon/back.png"); }
        }

        public string ImgDistance
        {
            get { return Device.OnPlatform("Icon/ic_marker_distance.png", "ic_marker_distance.png", "Assets/Icon/ic_marker_distance.png"); }
        }

        public string ImgDetail
        {
            get { return Device.OnPlatform("Icon/detail.png", "ic_detail.png", "Assets/Icon/detail.png"); }
        }

        private PropertyLang txtDistanceScaleCaption;
        public string TxtDistanceScaleValue
        {
            get { return txtDistanceScaleCaption.ActualValue(); }
        }

        private PropertyLang txtDiscount;
        public string TxtDiscount
        {
            get { return txtDiscount.ActualValue(); }
        }
    }
}
