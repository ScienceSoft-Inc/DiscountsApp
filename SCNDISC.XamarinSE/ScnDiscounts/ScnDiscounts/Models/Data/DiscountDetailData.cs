using System.Collections.Generic;

namespace ScnDiscounts.Models.Data
{
    public class DiscountDetailData : NotifyPropertyChanged
    {
        #region Title

        public string Title { get; set; }

        #endregion

        #region ImageFileName

        public string ImageFileName { get; set; }

        #endregion

        #region Description

        public string Description { get; set; }

        #endregion

        #region Persent

        public string Persent { get; set; }

        public string DiscountType { get; set; }

        #endregion

        #region Categories

        public List<CategoryData> CategoryList { get; set; }

        #endregion

        #region WebAddresses

        public List<WebAddressData> WebAddresses { get; set; }

        #endregion

        #region Branchs

        public List<DiscountDetailBranchData> BranchList { get; set; }

        #endregion
    }
}
