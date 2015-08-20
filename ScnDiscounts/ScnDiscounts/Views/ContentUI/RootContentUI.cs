using ScnDiscounts.Helpers;
using ScnDiscounts.Models;
using ScnPage.Plugin.Forms;
using Xamarin.Forms;

namespace ScnDiscounts.Views.ContentUI
{
    public class RootContentUI : BaseContentUI
    {
        public RootContentUI()
        {
            txtLoading = new LanguageStrings("loading...", "загрузка...", "пампаванне...");
            txtAwait = new LanguageStrings("wait...", "подождите...", "чакайце...");

            txtCancel = new LanguageStrings("Cancel", "Отменить", "Адмяніць");
            txtOk = new LanguageStrings("OK", "ОК", "Добра");

            txtDistanceScaleCaption = new LanguageStrings("km", "км", "км");
            txtDiscount = new LanguageStrings("Discount", "Скидка", "Зніжка");
            txtTurnOn = new LanguageStrings("On", "Вкл.", "Укл.");
            txtTurnOff = new LanguageStrings("Off", "Выкл.", "Выкл.");
        }

        private LanguageStrings txtCancel;
        public string TxtCancel
        {
            get { return txtCancel.Current; }
        }

        private LanguageStrings txtOk;
        public string TxtOk
        {
            get { return txtOk.Current; }
        }
        
        public string IconBack
        {
            get { return Device.OnPlatform("Icon/back.png", "ic_menu_back.png", "Assets/Icon/back.png"); }
        }

        public string ImgDistance
        {
            get { return Device.OnPlatform("Icon/marker_distance.png", "ic_marker_distance.png", "Assets/Icon/marker_distance.png"); }
        }

        public string ImgDetail
        {
            get { return Device.OnPlatform("Icon/detail.png", "ic_detail.png", "Assets/Icon/detail.png"); }
        }

        private LanguageStrings txtDistanceScaleCaption;
        public string TxtDistanceScaleValue
        {
            get { return txtDistanceScaleCaption.Current; }
        }

        private LanguageStrings txtDiscount;
        public string TxtDiscount
        {
            get { return txtDiscount.Current; }
        }

        private LanguageStrings txtTurnOn;
        public string TxtTurnOn
        {
            get { return txtTurnOn.Current; }
        }

        private LanguageStrings txtTurnOff;
        public string TxtTurnOff
        {
            get { return txtTurnOff.Current; }
        }
    }
}
