using System;
using System.Collections.Generic;
using ScnDiscounts.Models;

namespace ScnDiscounts.Helpers
{
    public static class LanguageHelper
    {
        public enum LangTypeEnum { ltEn, ltRu, ltBe };
        static public Dictionary<LangTypeEnum, string> LanguageList = new Dictionary<LangTypeEnum, string>
        {
            {LangTypeEnum.ltEn, "English" },
            {LangTypeEnum.ltRu, "Русский" },
            {LangTypeEnum.ltBe, "Беларуская" }
        };

        static public LangTypeEnum LangNameToEnum(string lang)
        {
            LangTypeEnum result = LangTypeEnum.ltEn;
            foreach (var item in LanguageList)
            {
                if (String.Compare(item.Value, lang, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    result = item.Key;
                    break;
                }
            }

            return result;
        }

        static public LangTypeEnum LangCodeToEnum(string lang)
        {
            if (String.Compare("EN", lang, StringComparison.CurrentCultureIgnoreCase) == 0)
                return LangTypeEnum.ltEn;
            if (String.Compare("RU", lang, StringComparison.CurrentCultureIgnoreCase) == 0)
                return LangTypeEnum.ltRu;
            if (String.Compare("BY", lang, StringComparison.CurrentCultureIgnoreCase) == 0)
                return LangTypeEnum.ltBe;
            return LangTypeEnum.ltEn;
        }

        static public string LangEnumToCode(LangTypeEnum langType)
        {
            switch (langType)
            {
                case LangTypeEnum.ltEn:
                    return "EN";

                case LangTypeEnum.ltRu:
                    return "RU";

                case LangTypeEnum.ltBe:
                    return "BY";

                default:
                    return "EN";
            }
        }
    }
}
