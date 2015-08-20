using System;
using System.Collections.Generic;
using System.Text;

namespace ScnDiscounts.Models.Data
{
    public class DiscountDetailData : NotifyPropertyChanged
    {
        public DiscountDetailData()
        {
            _categorieList = new List<CategorieData>();
            _branchList = new List<DiscountDetailBranchData>();
        }

        #region Title
        private string _title = "";
        public string Title
        {
            get { return _title; }
            set { _title = value; }
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
        private string _description = "";
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        #endregion

        #region Persent
        private string _persent = "";
        public string Persent
        {
            get { return _persent; }
            set { _persent = value; }
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

        #region Categories
        private List<CategorieData> _categorieList;
        public List<CategorieData> CategorieList { get { return _categorieList; } }
        #endregion

        #region Branchs
        private List<DiscountDetailBranchData> _branchList;
        public List<DiscountDetailBranchData> BranchList { get { return _branchList; } }
        #endregion
    }
}
