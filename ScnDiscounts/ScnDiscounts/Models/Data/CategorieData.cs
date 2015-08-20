using ScnDiscounts.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ScnDiscounts.Models.Data
{
    public class CategorieData
    {
        public CategorieData()
        {
            _name = new Dictionary<LanguageHelper.LangTypeEnum, string>();
        }

        private int _typeCode;
        public int TypeCode
        {
            get { return _typeCode; }
            set { _typeCode = value; }
        }

        #region Name
        private Dictionary<LanguageHelper.LangTypeEnum, string> _name;
        public void SetName(string langCode, string value)
        {
            var lang = LanguageHelper.LangCodeToEnum(langCode);
            _name.Add(lang, value);
        }
        #endregion
    }
}
