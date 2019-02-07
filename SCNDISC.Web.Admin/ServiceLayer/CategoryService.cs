using SCNDISC.Server.Application.Services.Admin;
using SCNDISC.Server.Application.Services.Categories;
using SCNDISC.Web.Admin.ServiceLayer.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCNDISC.Web.Admin.ServiceLayer
{
    public class CategoryService : ICategoryService
    {
        private readonly IAdminCommandApplicationService _adminCommandApplicationService;
        private readonly ICategoryApplicationService _categoryApplicationService;

        public CategoryService(IAdminCommandApplicationService adminCommandApplicationService,
            ICategoryApplicationService categoryApplicationService)
        {
            _adminCommandApplicationService = adminCommandApplicationService;
            _categoryApplicationService = categoryApplicationService;
        }

        public IEnumerable<Category> GetAll()
        {
            return Task.Factory.StartNew(() => _categoryApplicationService.GetCategoryListAsync())
                .Unwrap()
                .GetAwaiter()
                .GetResult().Where(i => !i.IsDeleted).Select(c => c.ToCategory());
        }

        public Category Save(Category category)
        {
            return Task.Factory.StartNew(() => _adminCommandApplicationService.UpsertCategoryAsync(category.ToCategory()))
                .Unwrap()
                .GetAwaiter()
                .GetResult().ToCategory();
        }

        public void Delete(Category category)
        {
            Task.Factory.StartNew(() => _adminCommandApplicationService.DeleteCategoryAsync(category.Id))
                .Unwrap()
                .GetAwaiter()
                .GetResult();
        }
    }
}