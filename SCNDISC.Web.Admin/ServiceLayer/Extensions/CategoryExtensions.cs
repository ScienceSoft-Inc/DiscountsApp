namespace SCNDISC.Web.Admin.ServiceLayer.Extensions
{
    public static class CategoryExtensions
    {
        public static Category ToCategory(this Server.Domain.Aggregates.Categories.Category categoryDomain)
        {
            var categoryUI = EnitiyExtensionsMapper.MapPublicProperties<Server.Domain.Aggregates.Categories.Category, Category>(categoryDomain);
            EnitiyExtensionsMapper.MapLocalizableToFlatProperties(categoryDomain, categoryUI);
            return categoryUI;
        }

        public static Server.Domain.Aggregates.Categories.Category ToCategory(this Category categoryUI)
        {
            var categoryDomain = EnitiyExtensionsMapper.MapPublicProperties<Category, Server.Domain.Aggregates.Categories.Category>(categoryUI);
            EnitiyExtensionsMapper.MapFlatToLocalizableProperties(categoryUI, categoryDomain);
            return categoryDomain;
        }
    }
}