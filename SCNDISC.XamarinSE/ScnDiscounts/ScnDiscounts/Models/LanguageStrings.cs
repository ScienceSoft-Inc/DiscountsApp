using ScnDiscounts.Helpers;
using ScnPage.Plugin.Forms;

namespace ScnDiscounts.Models
{
    public class LanguageStrings : BaseLanguageStrings
    {
        public LanguageStrings(string enUs = "", string ruRu = "")
            : base(enUs)
        {
            EnUsValue = enUs;
            RuRuValue = ruRu;
        }

        public string RuRuValue { get; set; }

        public override string GetCurrentLangString()
        {
            string result;

            switch (AppParameters.Config.SystemLang)
            {
                case LanguageHelper.LangTypeEnum.ltEn:
                    result = EnUsValue;
                    break;
                case LanguageHelper.LangTypeEnum.ltRu:
                    result = RuRuValue;
                    break;
                default:
                    result = EnUsValue;
                    break;
            }

            return result;
        }
    }
}
