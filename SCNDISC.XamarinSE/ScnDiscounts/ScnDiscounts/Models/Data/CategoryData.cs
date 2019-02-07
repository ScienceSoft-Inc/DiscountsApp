using ScnDiscounts.Helpers;
using System.Collections.Generic;

namespace ScnDiscounts.Models.Data
{
    public class CategoryData
    {
        public CategoryData()
        {
            _name = new Dictionary<LanguageHelper.LangTypeEnum, string>();
        }

        public int Id { get; set; }
        public string DocumentId { get; set; }

        public string Color { get; set; }

        #region Name

        private readonly Dictionary<LanguageHelper.LangTypeEnum, string> _name;
        public void SetName(string langCode, string value)
        {
            var lang = LanguageHelper.LangCodeToEnum(langCode);
            _name.Add(lang, value);
        }

        public string Name => _name.ContainsKey(AppParameters.Config.SystemLang) ? _name[AppParameters.Config.SystemLang] : string.Empty;

        #endregion
    }
}
