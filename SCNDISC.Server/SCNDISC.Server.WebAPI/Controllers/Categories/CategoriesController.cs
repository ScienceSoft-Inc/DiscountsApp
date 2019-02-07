using SCNDISC.Server.Application.Services.Categories;
using SCNDISC.Server.Domain.Aggregates.Categories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using SCNDISC.Server.WebAPI.Models.Partner;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SCNDISC.Server.WebAPI.Controllers.Categories
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CategoriesController : ApiController
    {
        private readonly ICategoryApplicationService _сategoryApplicationService;

        public CategoriesController(ICategoryApplicationService сategoryApplicationService)
        {
            _сategoryApplicationService = сategoryApplicationService;
        }

        [Route("categories/{syncdate?}")]
        [HttpGet]
        public async Task<IEnumerable<Category>> GetAllDiscountsAsync([FromUri]DateTime? syncdate = null)
        {
            return await _сategoryApplicationService.GetCategoryListAsync(syncdate);
        }

        [Route("categories/list/{syncdate?}")]
        [HttpGet]
        public async Task<IEnumerable<CategoryModel>> GetAllAsync([FromUri]DateTime? syncdate = null)
        {
            var categories = await _сategoryApplicationService.GetCategoryListAsync(syncdate);
            IEnumerable<CategoryModel> categoriesModels = Mapper.CategoryMapper.MapToCategoryModels(categories);
            return categoriesModels;
        }

        [Route("categories")]
        [HttpPost]
        public async Task<bool> SaveCategories(CategoryModel[] categoryModels, [FromUri]DateTime? syncdate = null)
        {
            var oldCategories = await _сategoryApplicationService.GetCategoryListAsync(syncdate);
            var categories = Mapper.CategoryMapper.MapToCategory(categoryModels);
            var deleteCategoryIds = Mapper.CategoryMapper.GetCategoriesForDeleting(categories, oldCategories);
            foreach (var category in categories)
            {
                var categor = await _сategoryApplicationService.UpsertCategoryAsync(category);
            }

            foreach (var deleteCategoryId in deleteCategoryIds)
            {
                await _сategoryApplicationService.DeleteCategoryAsync(deleteCategoryId);
            }
            return true;
        }

	}

  

}