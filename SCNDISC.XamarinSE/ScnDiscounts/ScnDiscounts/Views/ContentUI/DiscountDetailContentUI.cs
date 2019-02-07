using ScnDiscounts.Models;

namespace ScnDiscounts.Views.ContentUI
{
    public class DiscountDetailContentUI : RootContentUI
    {
        public DiscountDetailContentUI()
        {
            title = new LanguageStrings("Information", "Информация");
            _txtShowOnMap = new LanguageStrings("Show on map", "Показать на карте");
        }

        private readonly LanguageStrings _txtShowOnMap;
        public string TxtShowOnMap => _txtShowOnMap.Current;

        public string ImgPercentLabel => "ic_discount_label.png";
    }
}
