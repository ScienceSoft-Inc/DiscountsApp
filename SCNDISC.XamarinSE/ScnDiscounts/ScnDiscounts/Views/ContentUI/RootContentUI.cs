using ScnDiscounts.Models;
using ScnPage.Plugin.Forms;

namespace ScnDiscounts.Views.ContentUI
{
    public class RootContentUI : BaseContentUI
    {
        public RootContentUI()
        {
            txtLoading = new LanguageStrings("loading...", "загрузка...");
            txtAwait = new LanguageStrings("wait...", "подождите...");

            _txtCancel = new LanguageStrings("Cancel", "Отменить");
            _txtOk = new LanguageStrings("OK", "ОК");
            _txtError = new LanguageStrings("Error", "Ошибка");

            _txtDistanceScaleCaption = new LanguageStrings("km", "км");
            _txtDiscount = new LanguageStrings("Discount", "Скидка");
        }

        private readonly LanguageStrings _txtCancel;
        public string TxtCancel => _txtCancel.Current;

        private readonly LanguageStrings _txtOk;
        public string TxtOk => _txtOk.Current;

        private readonly LanguageStrings _txtError;
        public string TxtError => _txtError.Current;

        public string IconBack => "ic_menu_back.png";

        public string ImgDistance => "ic_marker_distance.png";

        public string ImgDetail => "ic_detail.png";

        public string ImgLogo => "img_logo.png";

        private readonly LanguageStrings _txtDistanceScaleCaption;
        public string TxtDistanceScaleValue => _txtDistanceScaleCaption.Current;

        private readonly LanguageStrings _txtDiscount;
        public string TxtDiscount => _txtDiscount.Current;
    }
}
