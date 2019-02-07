using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SCNDISC.Server.Domain.Aggregates.Categories;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.WebAPI.Models.Partner;

namespace SCNDISC.Server.WebAPI.Test
{
    [TestClass]
    public class CategoryMapperTest
    {
        [TestMethod]
        public void MapToCategoryModels()
        {
            //Arrange
            var categories = CreateCategories();
            //Act
            var categoryModels = Mapper.CategoryMapper.MapToCategoryModels(categories);
            //Assert
            for (var i = 0; i < categories.Count(); i++)
            {
                var category = categories.ElementAt(i);
                Assert.AreEqual(category.Color, categoryModels.ElementAt(i).Color);
                Assert.AreEqual(category.Id, categoryModels.ElementAt(i).Id);
                Assert.AreEqual(category.Name.First(x => x.Lan == Languages.Ru).LocText, categoryModels.ElementAt(i).Name_Ru);
                Assert.AreEqual(category.Name.First(x => x.Lan == Languages.En).LocText, categoryModels.ElementAt(i).Name_En);
            }
        }

        [TestMethod]
        public void MapToCategory()
        {
            //Arrange
            var categoryModels = CreateCategoryModels();
            //Act
            var categories = Mapper.CategoryMapper.MapToCategory(categoryModels);
            //Assert
            var categoryArray = categories as Category[] ?? categories.ToArray();
            for (var i = 0; i < categoryArray.Count(); i++)
            {
                var category = categoryArray.ElementAt(i);
                Assert.AreEqual(category.Color, categoryModels.ElementAt(i).Color);
                Assert.AreEqual(category.Id, categoryModels.ElementAt(i).Id);
                Assert.AreEqual(category.Name.First(x => x.Lan == Languages.Ru).LocText, categoryModels.ElementAt(i).Name_Ru);
                Assert.AreEqual(category.Name.First(x => x.Lan == Languages.En).LocText, categoryModels.ElementAt(i).Name_En);
            }
        }

        private List<CategoryModel> CreateCategoryModels()
        {
            var categoryModels = new List<CategoryModel>
            {
                new CategoryModel
                {
                    Color = "#123",
                    Id = "CategoryIds1",
                    Name_Ru = "Category1Ru",
                    Name_En = "Category1En"
                },
                new CategoryModel
                {
                    Color = "#234",
                    Id = "CategoryIds2",
                    Name_Ru = "Category2Ru",
                    Name_En = "Category2En"
                },
                new CategoryModel
                {
                    Color = "#345",
                    Id = "CategoryIds3",
                    Name_Ru = "Category3Ru",
                    Name_En = "Category3En"
                }
            };
            return categoryModels;
        }

        private List<Category> CreateCategories()
        {
            var categories = new List<Category>()
            {
                new Category
                {
                    Color = "#123",
                    Id = "CategoryIds1",
                    Name = SetValue("Category1Ru", "Category1En")
                },
                new Category
                {
                    Color = "#234",
                    Id = "CategoryIds2",
                    Name = SetValue("Category2Ru", "Category2En")
                },
                new Category
                {
                    Color = "#345",
                    Id = "CategoryIds3",
                    Name = SetValue("Category3Ru", "Category3En")
                    
                }
            };
            return categories;
        }

        private List<LocalizableText> SetValue(string valueRu, string valueEn)
        {
            var value = new List<LocalizableText>()
            {
                new LocalizableText()
                {
                    Lan = Languages.Ru,
                    LocText = valueRu
                },
                new LocalizableText()
                {
                    Lan = Languages.En,
                    LocText = valueEn
                }
            };
            return value;
        }
            
    }
}