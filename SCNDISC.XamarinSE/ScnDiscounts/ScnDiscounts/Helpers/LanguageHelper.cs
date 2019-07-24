using System.Collections.Generic;
using System.Linq;

namespace ScnDiscounts.Helpers
{
    public static class LanguageHelper
    {
        public enum LangTypeEnum
        {
            ltEn,
            ltRu
        }

        public static Dictionary<LangTypeEnum, string> LanguageList = new Dictionary<LangTypeEnum, string>
        {
            {LangTypeEnum.ltEn, "English"},
            {LangTypeEnum.ltRu, "Русский"}
        };

        public static LangTypeEnum LangNameToEnum(this string lang)
        {
            return LanguageList.Where(i => i.Value == lang).Select(i => i.Key).FirstOrDefault();
        }

        public static LangTypeEnum LangCodeToEnum(this string lang)
        {
            LangTypeEnum result;

            switch (lang)
            {
                case "EN":
                    result = LangTypeEnum.ltEn;
                    break;
                case "RU":
                    result = LangTypeEnum.ltRu;
                    break;
                default:
                    result = LangTypeEnum.ltEn;
                    break;
            }

            return result;
        }

        public static string LangEnumToCode(this LangTypeEnum langType)
        {
            string result;

            switch (langType)
            {
                case LangTypeEnum.ltEn:
                    result = "EN";
                    break;
                case LangTypeEnum.ltRu:
                    result = "RU";
                    break;
                default:
                    result = "EN";
                    break;
            }

            return result;
        }
    }
}
