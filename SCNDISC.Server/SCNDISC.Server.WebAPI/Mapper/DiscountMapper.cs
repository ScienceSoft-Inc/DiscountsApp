using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using SCNDISC.Server.Application.Services.Categories;
using SCNDISC.Server.Domain.Aggregates.Categories;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.WebAPI.Models.Partner;

namespace SCNDISC.Server.WebAPI.Mapper
{
    public abstract class DiscountMapper
    {
        public static IEnumerable<DiscountModel> MapToDiscountModel(IEnumerable<Branch> partners, IEnumerable<CategoryModel> categories)
        {
            List<DiscountModel> discounts = new List<DiscountModel>();
            DiscountModel discount = new DiscountModel();
            foreach (var partner in partners)
            {
                discount = FromBranchToDiscount(partner, categories);
                discounts.Add(discount);
            }
            return discounts;
        }

        private static DiscountModel FromBranchToDiscount(Branch branch, IEnumerable<CategoryModel> categories)
        {
            DiscountModel discount = new DiscountModel();
            discount.Name_Ru = branch.Name.First(x => x.Lan == "RU").LocText;
            discount.Name_En = branch.Name.First(x => x.Lan == "EN").LocText;
            discount.Description_Ru = branch.Description.First(x => x.Lan == "RU").LocText;
            discount.Description_En = branch.Description.First(x => x.Lan == "EN").LocText;
            //discount.Image = branch.Image;
            discount.Logo = branch.Icon;
            List<CategoryModel> categoriesList = new List<CategoryModel>();
            foreach (var id in branch.CategoryIds)
            {
                var category = categories.FirstOrDefault(x => x.Id == id);
                if (category != null)
                {
                    categoriesList.Add(category);
                }
            }
            discount.Categories = categoriesList;
            discount.Id = branch.Id;
            return discount;
        }
    }
}