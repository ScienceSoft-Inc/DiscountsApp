using Microsoft.Practices.ServiceLocation;
using MongoDB.Bson;
using SCNDISC.Server.Infrastructure.Imaging;
using SCNDISC.Web.Admin.Controls;
using SCNDISC.Web.Admin.ServiceLayer;
using SCNDISC.Web.Admin.ServiceLayer.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCNDISC.Server.Domain.Aggregates.Partners;

namespace SCNDISC.Web.Admin
{
    public partial class Default : Page
    {
        private readonly ICategoryService _categoryService = ServiceLocator.Current.GetInstance<ICategoryService>();
        private readonly ITipsService _tipService = ServiceLocator.Current.GetInstance<ITipsService>();
        private readonly IImageConverter _imageConverter = ServiceLocator.Current.GetInstance<IImageConverter>();
        private readonly IWebAddressCategoryService _webAddressCategoryService = ServiceLocator.Current.GetInstance<IWebAddressCategoryService>();
	    public static string WebApiUrl = ConfigurationManager.AppSettings.Get("web-api:url");

        protected override void OnInit(EventArgs e)
        {
            uxLangTips.Changed += OnLangTipsChanged;
            uxLangTipDetails.Changed += OnLangTipDetailsChanged;
            uxLangTipCategories.Changed += OnLangTipCategoriesChanged;

            base.OnInit(e);
        }

        protected void OnApplyFilter(object sender, EventArgs e)
        {
            var isSelectAll = CategoryFilter.CheckIfSelectedAll();
            var selectedCategories = isSelectAll ? null : CategoryFilter.GetSelectedValues();
            var partnerName = FilterName.Text;

            LoadTips(selectedCategories, partnerName);

            uxTips_Ru.DataBind();
            uxTips_Eng.DataBind();

            chkbx_selectAll.Checked = isSelectAll;
        }

        public override void DataBind()
        {
            var categories = _categoryService.GetAll().ToList();

            uxFormTypes.DataSource = categories;
            CategoryFilter.DataSource = categories;
            uxCategories.Items = categories;

            var tips = LoadTips();

            if (tips.Any())
            {
                uxNullScreen.Visible = false;
                uxAddNew.Visible = true;
                uxLangTips.Visible = true;
            }
            else
            {
                uxNullScreen.Visible = true;
                uxAddNew.Visible = false;
                uxLangTips.Visible = false;
            }

            base.DataBind();

            CategoryFilter.SelectAll();
        }

        private IEnumerable<TipForm> LoadTips(string[] selectedCategories = null, string partnerName = null)
        {
            var categories = _categoryService.GetAll();
            var tips = _tipService.GetAll(selectedCategories, partnerName).OrderBy(x => x.Id).ToList();
            tips.ForEach(i => i.Tags = categories.Where(c => i.Categories.Contains(c.Id)).ToArray());

            uxTips_Ru.DataSource = tips;
            uxTips_Eng.DataSource = tips;

            return tips;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
                DataBind();

            base.OnLoad(e);

            SyncLangControls();
        }

        protected void SyncLangControls()
        {
            var lang = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            uxLangTips.SelectedValue = lang;
            uxLangTipDetails.SelectedValue = lang;
            uxLangTipCategories.SelectedValue = lang;
        }

        const string uxLangTipsParam = "lang";
        const string uxLangTipsID = "uxLangTips";
        const string uxLangTipDetailsID = "uxLangTipDetails";
        const string uxLangTipCategoriesID = "uxLangTipCategories";

        protected override void InitializeCulture()
        {
            var postbackSource = Request.Params.Get("__EVENTTARGET");
            var currCulture = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;

            if (Request.HttpMethod == "GET" && !string.IsNullOrEmpty(Request[uxLangTipsParam])) // recover lang after adding/saving discount redirect (hack for form resubmit bug)
            {
                UICulture = Request[uxLangTipsParam].ToLower() == "en" ? "en" : "ru";
            }
            else if (!string.IsNullOrEmpty(postbackSource)) // POST becuase of postback event
            {
                if (postbackSource.Contains(uxLangTipDetailsID))
                    UICulture = Request.Form[uxLangTipDetailsID + "$uxLang"];
                else if (postbackSource.Contains(uxLangTipCategoriesID))
                    UICulture = Request.Form[uxLangTipCategoriesID + "$uxLang"];
                else UICulture = Request.Form[uxLangTipsID + "$uxLang"] ?? currCulture;  // it turned out that with empty DB language control is not rendered
            }
            else if (Request.HttpMethod == "POST") //any other POST request
            {
                UICulture = Request.Form[uxLangTipsID + "$uxLang"] ?? currCulture; // it turned out that with empty DB language control is not rendered
            }

            // And we will reach this point with not above branches executing in case of the vert first GET request. In this case culture will be set automaticaly (according to browser settings)

            if (currCulture != "en" && currCulture != "ru")
                UICulture = "ru"; // JIC, if default culture happens to be not within supported list

            base.InitializeCulture();
        }

        private void RenderFormClientScriptBehaivor(bool visibility)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), GetType().ToString(),
                string.Format("new TipForm().show({0});", visibility.ToString().ToLower()), true);
        }

        private TipForm ExtractDetails()
        {
            var inputIcon64Base = "";
            var inputTheme64Base = "";

            if (uxFormAvatarUpload.FileBytes.Length > 0)
            {
                var icon = _imageConverter.Convert(uxFormAvatarUpload.FileBytes, ImageOptions.Icon);
                inputIcon64Base = Convert.ToBase64String(icon);
            }

            if (uxFormThemeUpload.FileBytes.Length > 0)
            {
                var image = _imageConverter.Convert(uxFormThemeUpload.FileBytes, ImageOptions.Image);
                inputTheme64Base = Convert.ToBase64String(image);
            }

            if ((string.IsNullOrEmpty(inputIcon64Base) || string.IsNullOrEmpty(inputTheme64Base)) &&
                !string.IsNullOrEmpty(uxTipID.Value))
            {
                var tip = _tipService.GetById(uxTipID.Value).First(x => x.PartnerId == x.Id);
                if (string.IsNullOrEmpty(inputIcon64Base))
                {
                    inputIcon64Base = tip.Icon;
                }

                if (string.IsNullOrEmpty(inputTheme64Base))
                {
                    inputTheme64Base = tip.Image;
                }
            }

            var contact = uxContacts.Value.First();
            return new TipForm
            {
                Id = uxTipID.Value,
                Name_RU = uxName_Ru.Text,
                Name_EN = uxName_Eng.Text,
                Description_RU = uxDescription_Ru.Text,
                Description_EN = uxFormDecription_Eng.Text,
                PartnerId = uxTipID.Value,
			    //Url = uxFormUrl.Text,
                Categories = uxFormTypes.GetSelectedValues().ToList(),
                Discount = uxFormDiscount.Text,
                Image = inputTheme64Base,
                Icon = inputIcon64Base,
                Address_RU = contact.Address_RU,
                Address_EN = contact.Address_EN,
                Point = contact.Point,
                Phone1 = contact.Phone1,
                Phone2 = contact.Phone2,
				Comment = uxComment.Text,
				DiscountType = DiscountType.SelectedValue,
				WebAddresses = GetWebAddresses()
            };
        }

        private void OnLangTipDetailsChanged(object sender, LanguageChangedArgs args)
        {
            uxServiceDetails_Ru.Visible = false;
            uxServiceDetails_Eng.Visible = false;

            if (args.Language == Language.Ru)
            {
                uxServiceDetails_Ru.Visible = true;
                uxFormTypes.CssClass = "x-form-types x-form-types-ru";
                uxContacts.SwitchLanguage(Language.Ru);
            }

            if (args.Language == Language.En)
            {
                uxFormTypes.CssClass = "x-form-types x-form-types-en";
                uxServiceDetails_Eng.Visible = true;
                uxContacts.SwitchLanguage(Language.En);
            }
        }

        private void OnLangTipCategoriesChanged(object sender, LanguageChangedArgs args)
        {
            if (args.Language == Language.Ru)
                uxCategories.SwitchLanguage(Language.Ru);

            if (args.Language == Language.En)
                uxCategories.SwitchLanguage(Language.En);
        }

        private void OnLangTipsChanged(object sender, LanguageChangedArgs args)
        {
            uxTips_Ru.Visible = false;
            uxTips_Eng.Visible = false;

            if (args.Language == Language.Ru)
            {
                UICulture = "ru-Ru";
                CategoryFilter.CssClass = SwitchCssClass(CategoryFilter.CssClass, "x-form-types-en", "x-form-types-ru");
                uxTips_Ru.Visible = true;
            }

            if (args.Language == Language.En)
            {
                UICulture = "en";
                CategoryFilter.CssClass = SwitchCssClass(CategoryFilter.CssClass, "x-form-types-ru", "x-form-types-en");
                uxTips_Eng.Visible = true;
            }
        }

        private static string SwitchCssClass(string cssClass, string oldClass, string newClass)
        {
            if (cssClass.Contains(oldClass))
            {
                return cssClass.Replace(oldClass, newClass);
            }

            return cssClass.Trim() + " " + newClass;
        }

        protected void OnNewAdd(object sender, EventArgs e)
        {
            RenderFormClientScriptBehaivor(true);
            InitForm(new List<TipForm> { new TipForm() });
        }

        protected void OnLogout(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }

        protected void OnItemCommand(object source, RepeaterCommandEventArgs e)
        {
            InitForm(_tipService.GetById(e.CommandArgument.ToString()).ToList());
        }

        private void InitForm(List<TipForm> details)
        {
            var tipForm = details.First(x => x.Id == x.PartnerId);
            uxTipID.Value = tipForm.Id;
            uxName_Ru.Text = tipForm.Name_RU;
            uxName_Eng.Text = tipForm.Name_EN;
            uxDescription_Ru.Text = tipForm.Description_RU;
            uxFormDecription_Eng.Text = tipForm.Description_EN;
            //uxFormUrl.Text = tipForm.Url;
            uxFormDiscount.Text = tipForm.Discount;
            //uxFormUrl.Text = tipForm.Url;
            uxComment.Text = tipForm.Comment;
            uxFormTypes.SetSelectedValues(tipForm.Categories.ToArray());
            uxLangTipDetails.SelectedValue = uxLangTips.SelectedValue;
            uxFormDelete.Visible = !string.IsNullOrEmpty(tipForm.Id);

			DiscountType.SelectedValue = tipForm.DiscountType ?? "0";

			InitWebAddresses(tipForm.WebAddresses);

	        uxAddWebAddress.CommandArgument = tipForm.Id;

            var tipForms = new List<TipForm> {tipForm};
            tipForms.AddRange(details.Where(x => x.Id != x.PartnerId));
            uxContacts.Value = tipForms;

			RenderFormClientScriptBehaivor(true);
        }

	    private void InitWebAddresses(List<WebAddressItem> items)
	    {
		    uxWebUrls.DataSource = items;
		    uxWebUrls.DataBind();
	    }

	    protected void OnSaveTip(object sender, EventArgs e)
	    {
	        var newTipForm = ExtractDetails();
	        var contacts = ExtractContacts();

            if (!string.IsNullOrEmpty(newTipForm.Id))
                _tipService.RemoveAllBranches(newTipForm.Id);

            var tipForm = _tipService.Save(newTipForm);

            contacts.ForEach(c => c.Description_RU = tipForm.Description_RU);
            contacts.ForEach(c => c.Description_EN = tipForm.Description_EN);
            contacts.ForEach(c => c.Name_RU = tipForm.Name_RU);
            contacts.ForEach(c => c.Name_EN = tipForm.Name_EN);
            contacts.ForEach(c => c.Discount = tipForm.Discount);
            contacts.ForEach(c => c.Categories = tipForm.Categories);
            contacts.ForEach(x => x.PartnerId = tipForm.Id);
            contacts.ForEach(x => _tipService.Save(x));

            //DataBind();

            Response.Redirect(AppRelativeVirtualPath + "?" + uxLangTipsParam + "=" + HttpUtility.HtmlEncode(uxLangTips.SelectedValue));
            // form resubmission catch - prevent this event firing due to form resubmission by browser (clicking Refresh right after adding a discount) and thus doube adding discounts
        }

        private List<TipForm> ExtractContacts()
        {
            return uxContacts.Value.Skip(1).ToList();
        }

        protected void OnDeleteTip(object sender, EventArgs e)
        {
            var tipForm = ExtractDetails();
            _tipService.Delete(tipForm);
            RenderFormClientScriptBehaivor(false);
            DataBind();
        }

        protected void OnAddCategory(object sender, EventArgs e)
        {
            var categories = uxCategories.GetCategories();
            categories.Add(new Category());

            uxCategories.Items = categories;
            uxCategories.DataBind();
        }

        protected void OnSaveCategories(object sender, EventArgs e)
        {
            var oldCategories = _categoryService.GetAll().ToList();
            var categories = uxCategories.GetCategories();

            foreach (var category in categories)
            {
                _categoryService.Save(category);
            }

            foreach (var category in oldCategories.Where(i => categories.All(j => j.Id != i.Id)))
            {
                _categoryService.Delete(category);
            }

            DataBind();

            uxCategoryFormDeleteList_Ru.Value = null;
            uxCategoryFormDeleteList_Eng.Value = null;
        }

        protected void OnCancelCategories(object sender, EventArgs e)
        {
            var categories = _categoryService.GetAll().ToList();

            uxCategories.Items = categories;
            uxCategories.DataBind();

            uxCategoryFormDeleteList_Ru.Value = null;
            uxCategoryFormDeleteList_Eng.Value = null;
        }

        protected void OnAddWebAddressCommand(object sender, CommandEventArgs e)
	    {
		    var addresses = GetWebAddresses();

		    var next = addresses.Count == 0 ? 1 : addresses.Max(x => x.Index) + 1;
		    addresses.Add(new WebAddressItem
		    {
			    Id = ObjectId.GenerateNewId().ToString(),
			    Index = next
		    });

			InitWebAddresses(addresses);
		}

	    private List<WebAddressItem> GetWebAddresses()
	    {
		    var items = new List<WebAddressItem>();
		    foreach (RepeaterItem repeaterItem in uxWebUrls.Items)
		    {
			    items.Add(new WebAddressItem
			    {
				    Id = ((HiddenField) repeaterItem.FindControl("WebAddressId")).Value,
				    Url = ((TextBox) repeaterItem.FindControl("uxWebUrl")).Text,
				    Category = ((DropDownList) repeaterItem.FindControl("WebAddressCategory")).SelectedValue
			    });
		    }

		    var index = 0;
		    items.ForEach(x=>x.Index = ++index);

			return items;
	    }

	    protected void OnRemoveWebAddressCommand(object source, RepeaterCommandEventArgs e)
	    {
		    var addresses = GetWebAddresses();

		    var id = e.CommandArgument.ToString();
		    var item = addresses.FirstOrDefault(x => x.Id == id);
		    addresses.Remove(item);

		    InitWebAddresses(addresses);
		}

	    protected void OnWebAddressItemDataBound(object sender, RepeaterItemEventArgs e)
	    {
			if (e.Item.ItemType == ListItemType.Item ||
			    e.Item.ItemType == ListItemType.AlternatingItem)
			{
				var dataItem = (WebAddressItem) e.Item.DataItem;

				var webAddressCategory = ((DropDownList)e.Item.FindControl("WebAddressCategory"));
				webAddressCategory.DataSource = GetWebAddressCategories();

				if (!string.IsNullOrEmpty(dataItem.Category))
				{
					webAddressCategory.SelectedValue = dataItem.Category;
				}
				
				webAddressCategory.DataBind();
			}
		}

	    private IEnumerable<WebAddressCategory> GetWebAddressCategories()
	    {
		    return _webAddressCategoryService.GetAll();
	    }
		
        protected void OnCategoryDeleted(object sender, Category category)
        {
            uxCategoryFormDeleteList_Ru.Value =
                (uxCategoryFormDeleteList_Ru.Value + ", " + category.Name_RU).Trim(' ', ',');

            uxCategoryFormDeleteList_Eng.Value =
                (uxCategoryFormDeleteList_Eng.Value + ", " + category.Name_EN).Trim(' ', ',');
        }    
    }
}