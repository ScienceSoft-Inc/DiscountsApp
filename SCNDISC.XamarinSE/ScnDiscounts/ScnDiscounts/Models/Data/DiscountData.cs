using ScnDiscounts.Helpers;
using System.Collections.Generic;

namespace ScnDiscounts.Models.Data
{
    public class DiscountData : NotifyPropertyChanged
    {
        public string DocumentId { get; set; }

        public string LogoFileName { get; set; }

        public string DiscountPercent { get; set; }

        public string DiscountType { get; set; }

        #region Name

        private readonly Dictionary<LanguageHelper.LangTypeEnum, string> _name = new Dictionary<LanguageHelper.LangTypeEnum, string>();
        public void SetName(string langCode, string value)
        {
            var lang = LanguageHelper.LangCodeToEnum(langCode);
            _name.Add(lang, value);
        }

        public string Name => _name.ContainsKey(AppParameters.Config.SystemLang) ? _name[AppParameters.Config.SystemLang] : string.Empty;

        #endregion

        #region Description

        private readonly Dictionary<LanguageHelper.LangTypeEnum, string> _description = new Dictionary<LanguageHelper.LangTypeEnum, string>();
        public void SetDescription(string langCode, string value)
        {
            var lang = LanguageHelper.LangCodeToEnum(langCode);
            _description.Add(lang, value);
        }

        public string Description => _description.ContainsKey(AppParameters.Config.SystemLang) ? _description[AppParameters.Config.SystemLang] : string.Empty;

        #endregion

        #region Categories

        public List<CategoryData> CategoryList { get; set; } = new List<CategoryData>();

        #endregion
    }
}

