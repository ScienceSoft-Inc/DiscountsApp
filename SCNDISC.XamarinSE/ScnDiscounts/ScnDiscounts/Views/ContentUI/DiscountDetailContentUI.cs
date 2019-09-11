using ScnDiscounts.Models;

namespace ScnDiscounts.Views.ContentUI
{
    public class DiscountDetailContentUI : RootContentUI
    {
        public DiscountDetailContentUI()
        {
            title = new LanguageStrings("Information", "Информация");
            _txtShowOnMap = new LanguageStrings("Show on map", "Показать на карте");
            _msgRatingSubmitError = new LanguageStrings("The rating value has not been submitted",
                "Значение рейтинга не было отправлено");
        }

        private readonly LanguageStrings _txtShowOnMap;
        public string TxtShowOnMap => _txtShowOnMap.Current;

        private readonly LanguageStrings _msgRatingSubmitError;
        public string MsgRatingSubmitError => _msgRatingSubmitError.Current;

        public string ImgPercentLabel => "ic_discount_label.png";
    }
}
