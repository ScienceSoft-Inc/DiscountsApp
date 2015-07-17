using System;
using System.Collections.Generic;
using System.Text;
using ScnPage.Plugin.Forms;
using ScnDiscounts.Helpers;

namespace ScnDiscounts.Models
{
    public class LanguageStrings : BaseLanguageStrings
    {
        public LanguageStrings(string enUs = "", string ruRu = "", string beBe = "")
        {
            EnUsValue = enUs;
            RuRuValue = ruRu;
            BeBeValue = beBe;
        }
        
        public string EnUsValue { get; set; }
        public string RuRuValue { get; set; }
        public string BeBeValue { get; set; }

        public string Current
        {
            get { return GetCurrentLangString(); }
        }

        public override string GetCurrentLangString()
        {
            string stringDescription = EnUsValue;

            switch (AppParameters.Config.SystemLang)
            {
                case (LanguageHelper.LangTypeEnum.ltRu):
                    stringDescription = RuRuValue;
                    break;

                case (LanguageHelper.LangTypeEnum.ltBe):
                    stringDescription = BeBeValue;
                    break;

                case (LanguageHelper.LangTypeEnum.ltEn):
                default:
                    break;
            };

            return stringDescription;
        }
    }
}
