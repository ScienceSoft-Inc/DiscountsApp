using System.Collections.Generic;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models.WebService.MongoDB;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System;

namespace ScnDiscounts.Models.Data
{
    public class DiscountData : NotifyPropertyChanged
    {
        public DiscountData()
        {
            _name = new Dictionary<LanguageHelper.LangTypeEnum, string>();
            _description = new Dictionary<LanguageHelper.LangTypeEnum, string>();

            _categorieList = new List<CategorieData>();
        }
        public DiscountData(DeserializeBranchItem branchItem) : this()
        {
            _documentId = branchItem.Id;
            _partnerId = branchItem.PartnerId;

            foreach (var item in branchItem.Name)
                SetName(item.Lan, item.LocText);

            foreach (var item in branchItem.Description)
                SetDescription(item.Lan, item.LocText);
            
            if (branchItem.Discounts.Count > 0 && branchItem.Discounts[0].Name.Count > 0)
                _discountPercent = branchItem.Discounts[0].Name[0].LocText;
           
            foreach (var item in branchItem.Categories)
            {
                var categorie = new CategorieData();
                int typeCode = 0;
                int.TryParse (item.Type, out typeCode);
                categorie.TypeCode = typeCode;
                foreach (var itemName in item.Name)
                    categorie.SetName(itemName.Lan, itemName.LocText);
                _categorieList.Add(categorie);
            }

            _urlAddress = branchItem.Url;

            _icon = branchItem.Icon;
        }

        #region DocumentId
        private string _documentId;
        public string DocumentId
        {
            get { return _documentId; }
            set { _documentId = value; }
        }
        #endregion

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

        public Dictionary<LanguageHelper.LangTypeEnum, string> NameList
        {
            get { return _name; }
        }
        #endregion

        #region IsFullDescription
        private bool _isFullDescription = false;
        public bool IsFullDescription
        {
            get { return _isFullDescription; }
            set { _isFullDescription = value; }
        }
        #endregion

        #region LogoFileName
        private string _logoFileName = "";
        public string LogoFileName
        {
            get { return _logoFileName; }
            set { _logoFileName = value; }
        }
        #endregion

        #region ImageFileName
        private string _imageFileName = "";
        public string ImageFileName
        {
            get { return _imageFileName; }
            set { _imageFileName = value; }
        }
        #endregion

        #region Description
        private Dictionary<LanguageHelper.LangTypeEnum, string> _description;
        public void SetDescription(string langCode, string value)
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

        public Dictionary<LanguageHelper.LangTypeEnum, string> DescriptionList
        {
            get { return _description; }
        }

        #endregion

        #region Categories
        private List<CategorieData> _categorieList;
        public List<CategorieData> CategorieList { get { return _categorieList; } }
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
        private string _discountPercent;
        public string DiscountPercent
        {
            get { return _discountPercent; }
            set { _discountPercent = value; }
        }
        #endregion

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
