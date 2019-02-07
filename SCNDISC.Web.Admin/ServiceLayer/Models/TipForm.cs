using System.Collections.Generic;

namespace SCNDISC.Web.Admin.ServiceLayer
{
    public class TipForm
    {
        public TipForm()
        {
            Categories = new List<string>();
			WebAddresses = new List<WebAddressItem>();
            Discount = "0";
        }

        public string Id { get; set; }
        public string PartnerId { get; set; }
        public Category[] Tags { get; set; }
        public List<string> Categories { get; set; }
        public string Name_EN { get; set; }
        public string Description_EN { get; set; }
        public string Address_EN { get; set; }
        public string Discount { get; set; }
        public string DiscountType { get; set; }
        public string Name_RU { get; set; }
        public string Description_RU { get; set; }
        public string Address_RU { get; set; }
        public string Url { get; set; }
		public List<WebAddressItem> WebAddresses { get; set; }

        public string Point { get; set; }
        public string Icon { get; set; }
        public string Image { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Comment { get; set; }
    }

	public class WebAddressItem
	{
		public int Index { get; set; }
		public string Id { get; set; }
		public string Url { get; set; }
		public string Category { get; set; }
	}
}