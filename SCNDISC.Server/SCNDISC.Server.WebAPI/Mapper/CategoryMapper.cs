using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SCNDISC.Server.Application.Services.Categories;
using SCNDISC.Server.Domain.Aggregates.Categories;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.WebAPI.Models.Partner;

namespace SCNDISC.Server.WebAPI.Mapper
{
    public abstract class CategoryMapper
    {
        public static IEnumerable<CategoryModel> MapToCategoryModels(IEnumerable<Category> categories)
        {
            List<CategoryModel> categoriesResult = new List<CategoryModel>();
            foreach (var category in categories)
            {
                if (category.IsDeleted)
                {
                    continue;
                }
                var categoryModel = FromCategoryToCategoryModel(category);
                categoriesResult.Add(categoryModel);
            }

            return categoriesResult;
        }

        private static CategoryModel FromCategoryToCategoryModel(Category category)
        {
            var categoryModel = new CategoryModel
            {
                Id = category.Id,
                Color = category.Color,
                Name_Ru = PartnerMapper.GetValue(category.Name, Languages.Ru),
                Name_En = PartnerMapper.GetValue(category.Name, Languages.En)
            };
            return categoryModel;
        }

        public static IEnumerable<Category> MapToCategory(IEnumerable<CategoryModel> categoryModels)
        {
            var categories = new List<Category>();
            foreach (var categoryModel in categoryModels)
            {
                var category = FromCategoryModelToCategory(categoryModel);
                categories.Add(category);
            }

            return categories;
        }

        private static Category FromCategoryModelToCategory(CategoryModel categoryModel)
        {
            Category category = new Category
            {
                Color = categoryModel.Color,
                Name = PartnerMapper.SetValue(categoryModel.Name_Ru, categoryModel.Name_En)
        };
            if (categoryModel.Id != null)
            {
                category.Id = categoryModel.Id;
            }

            return category;
        }

        public static IEnumerable<string> GetCategoriesForDeleting(IEnumerable<Category> newCategories,
            IEnumerable<Category> oldCategories)
        {
            List<string> deleteCategoryIds = new List<string>();
            foreach (var oldCategory in oldCategories)
            {
                var category = newCategories.FirstOrDefault(x => x.Id == oldCategory.Id);
                if (category == null)
                {
                    deleteCategoryIds.Add(oldCategory.Id);
                }
            }
            return deleteCategoryIds;
        }
    }
}