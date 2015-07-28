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
            DefaultValue = enUs;

            EnUsValue = enUs;
            RuRuValue = ruRu;
            BeBeValue = beBe;
        }

        private string DefaultValue { get; set; }

        public string EnUsValue { get; set; }
        public string RuRuValue { get; set; }
        public string BeBeValue { get; set; }

        public string Current
        {
            get { return GetCurrentLangString(); }
        }

        public string Default
        {
            get { return DefaultValue; }
        }

        public override string GetCurrentLangString()
        {
            string stringDescription = DefaultValue;

            switch (AppParameters.Config.SystemLang)
            {
                case (LanguageHelper.LangTypeEnum.ltEn):
                    stringDescription = EnUsValue;
                    break;

                case (LanguageHelper.LangTypeEnum.ltRu):
                    stringDescription = RuRuValue;
                    break;

                case (LanguageHelper.LangTypeEnum.ltBe):
                    stringDescription = BeBeValue;
                    break;

                default:
                    break;
            };

            return stringDescription;
        }
    }
}
