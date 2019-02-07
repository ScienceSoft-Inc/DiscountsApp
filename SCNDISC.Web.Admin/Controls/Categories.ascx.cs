using SCNDISC.Web.Admin.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SCNDISC.Web.Admin.Controls
{
    public partial class Categories : UserControl
    {
        public List<Category> Items { get; set; }

        public Language Language { get; set; }

        public event EventHandler<Category> CategoryDeleted;

        public void SwitchLanguage(Language language)
        {
            Language = language;

            foreach (RepeaterItem item in uxCategories.Items)
            {
                item.FindControl("uxCategoryForm_Ru").Visible = language == Language.Ru;
                item.FindControl("uxCategoryForm_Eng").Visible = language == Language.En;
            }
        }

        public override void DataBind()
        {
            uxCategories.DataSource = Items;

            base.DataBind();

            SwitchLanguage(Language);
        }

        public List<Category> GetCategories()
        {
            var result = new List<Category>();

            foreach (RepeaterItem item in uxCategories.Items)
            {
                var category = new Category
                {
                    Id = ((HiddenField) item.FindControl("CategoryId")).Value,
                    Color = ((HtmlInputGenericControl) item.FindControl("uxCategoryColor")).Value,
                    Name_RU = ((TextBox) item.FindControl("uxCategoryName_Ru")).Text,
                    Name_EN = ((TextBox) item.FindControl("uxCategoryName_Eng")).Text
                };

                result.Add(category);
            }

            return result;
        }

        protected void uxDelete_OnCommand(object sender, CommandEventArgs e)
        {
            var index = int.Parse((string) e.CommandArgument);

            var categories = GetCategories();
            var item = categories.ElementAtOrDefault(index);

            if (item != null)
            {
                categories.Remove(item);

                if (!string.IsNullOrEmpty(item.Id))
                    OnCategoryDeleted(item);
            }

            Items = categories;
            DataBind();
        }

        protected void OnCategoryDeleted(Category category)
        {
            CategoryDeleted?.Invoke(this, category);
        }
    }
}