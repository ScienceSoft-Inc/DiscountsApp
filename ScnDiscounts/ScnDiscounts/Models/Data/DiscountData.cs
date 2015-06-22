using System.Collections.Generic;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models.WebService.MongoDB;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace ScnDiscounts.Models.Data
{
    public class DiscountData : NotifyPropertyChanged
    {
        public DiscountData(DeserializeBranchItem branchItem)
        {
            _id = branchItem.Id;
            _partnerId = branchItem.PartnerId;

            _name = new Dictionary<LanguageHelper.LangTypeEnum, string>();
            foreach (var item in branchItem.Name)
                SetName(item.Lan, item.LocText);

            _description = new Dictionary<LanguageHelper.LangTypeEnum, string>();
            foreach (var item in branchItem.Description)
                SetDescription(item.Lan, item.LocText);

            _discountValueList = new List<DiscountValue>();
            foreach (var item in branchItem.Discounts)
            {
                var discountValue = new DiscountValue();
                foreach (var itemName in item.Name)
                    discountValue.SetName(itemName.Lan, itemName.LocText);
                _discountValueList.Add(discountValue);
            }

            _categorieList = new List<Categorie>();
            foreach (var item in branchItem.Categories)
            {
                var categorie = new Categorie();
                int typeCode = 0;
                int.TryParse (item.Type, out typeCode);
                categorie.TypeCode = typeCode;
                foreach (var itemName in item.Name)
                    categorie.SetName(itemName.Lan, itemName.LocText);
                _categorieList.Add(categorie);
            }

            _urlAddress = branchItem.Url;

            _address = new Dictionary<LanguageHelper.LangTypeEnum, string>();
            foreach (var item in branchItem.Address)
                SetAddress(item.Lan, item.LocText);

            _phoneList = new List<string>();
            foreach (var item in branchItem.Phones)
                _phoneList.Add(item.Number);

            _icon = branchItem.Icon;

            _branchList = new ObservableCollection<BranchData>();
        }

        #region Id
        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        #endregion

        #region Name
        private Dictionary<LanguageHelper.LangTypeEnum, string> _name;
        private void SetName(string langCode, string value)
        {
            var lang = LanguageHelper.LangCodeToEnum(langCode);
            _name.Add(lang, value);
        }

        public string Name
        {
            get
            {
                if (_name.ContainsKey(AppParameters.Config.SystemLang))
                    return _name[AppParameters.Config.SystemLang];
                return "empty";
            }
        }
        #endregion

        #region Description
        private Dictionary<LanguageHelper.LangTypeEnum, string> _description;
        private void SetDescription(string langCode, string value)
        {
            var lang = LanguageHelper.LangCodeToEnum(langCode);
            _description.Add(lang, value);
        }

        public string Description
        {
            get
            {
                if (_description.ContainsKey(AppParameters.Config.SystemLang))
                    return _description[AppParameters.Config.SystemLang];
                return "empty";
            }
        }
        #endregion

        #region DiscountValues
        private List<DiscountValue> _discountValueList;
        public List<DiscountValue> DiscountValueList { get { return _discountValueList; } }
        #endregion

        #region Categories
        private List<Categorie> _categorieList;
        public List<Categorie> CategorieList { get { return _categorieList; } }
        #endregion

        #region FirstCategoryName
        public string FirstCategoryName
        {
            get
            {
                var categoryName = "noname";

                if (_categorieList.Count > 0)
                {
                    if (CategoryHelper.CategoryList.ContainsKey(_categorieList[0].TypeCode))
                    {
                        var categoryParam = CategoryHelper.CategoryList[_categorieList[0].TypeCode];
                        categoryName = categoryParam.Name;
                    }
                }

                return categoryName.ToUpper();
            }
        }
        #endregion

        #region FirstCategoryColor
        public Color FirstCategoryColor
        {
            get
            {
                var categoryColor = Color.Transparent;

                if (_categorieList.Count > 0)
                {
                    if (CategoryHelper.CategoryList.ContainsKey(_categorieList[0].TypeCode))
                    {
                        var categoryParam = CategoryHelper.CategoryList[_categorieList[0].TypeCode];
                        categoryColor = categoryParam.ColorTheme;
                    }
                }

                return categoryColor;
            }
        }
        #endregion

        #region IsCategoryMore
        public bool IsCategoryMore
        {
            get { return (_categorieList.Count > 1); }
        }
        #endregion

        #region UrlAddress
        private string _urlAddress = "";
        public string UrlAddress
        {
            get { return _urlAddress; }
            set { _urlAddress = value; }
        }
        #endregion

        #region Address
        private Dictionary<LanguageHelper.LangTypeEnum, string> _address;
        private void SetAddress(string langCode, string value)
        {
            var lang = LanguageHelper.LangCodeToEnum(langCode);
            _address.Add(lang, value);
        }

        public string Address
        {
            get
            {
                if (_address.ContainsKey(AppParameters.Config.SystemLang))
                    return _address[AppParameters.Config.SystemLang];
                return "empty";
            }
        }
        #endregion

        #region Phones
        private List<string> _phoneList;
        public List<string> PhoneList { get { return _phoneList; } }
        #endregion

        #region Icon
        private string _icon = "";
        public string Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }
        #endregion

        #region Image
        private string _image = "";
        public string Image
        {
            get { return _image; }
            set { _image = value; }
        }
        #endregion

        #region PartnerId
        private string _partnerId;
        public string PartnerId
        {
            get { return _partnerId; }
            set { _partnerId = value; }
        }
        #endregion

        #region DiscountPercent
        public string DiscountPercent
        {
            get
            {
                var percentValue = "0";

                if (_discountValueList.Count > 0)
                {
                    percentValue = _discountValueList[0].Name;
                }

                return percentValue;
            }
        }
        #endregion

        #region Branchs
        private ObservableCollection<BranchData> _branchList;
        public ObservableCollection<BranchData> BranchList 
        {
            get { return _branchList; } 
        }
        #endregion

        public class Categorie
        {
            public Categorie ()
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

            public string Name
            {
                get
                {
                    if (_name.ContainsKey(AppParameters.Config.SystemLang))
                        return _name[AppParameters.Config.SystemLang];
                    return "empty";
                }
            }
            #endregion
        }
       
        public class DiscountValue
        {
            public DiscountValue()
            {
                _name = new Dictionary<LanguageHelper.LangTypeEnum, string>();
            }

            #region Name
            private Dictionary<LanguageHelper.LangTypeEnum, string> _name;
            public void SetName(string langCode, string value)
            {
                var lang = LanguageHelper.LangCodeToEnum(langCode);
                _name.Add(lang, value);
            }

            public string Name
            {
                get
                {
                    if (_name.ContainsKey(AppParameters.Config.SystemLang))
                        return _name[AppParameters.Config.SystemLang];
                    return "empty";
                }
            }
            #endregion
        }
    }
}
