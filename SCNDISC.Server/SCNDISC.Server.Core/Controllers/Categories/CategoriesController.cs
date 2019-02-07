using SCNDISC.Server.Application.Services.Categories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCNDISC.Server.Core.Models.Partner;
using Microsoft.AspNetCore.Mvc;
using SCNDISC.Server.Domain.Aggregates.Categories;

namespace SCNDISC.Server.Core.Controllers.Categories
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryApplicationService _сategoryApplicationService;

        public CategoriesController(ICategoryApplicationService сategoryApplicationService)
        {
            _сategoryApplicationService = сategoryApplicationService;
        }

        [HttpGet("{syncdate?}")]
        public async Task<IEnumerable<Category>> GetAllDiscountsAsync([FromQuery]DateTime? syncdate = null)
        {
            return await _сategoryApplicationService.GetCategoryListAsync(syncdate);
        }

        [HttpGet("list/{syncdate?}")]
        public async Task<IEnumerable<CategoryModel>> GetAllAsync([FromQuery]DateTime? syncdate = null)
        {
            var categories = await _сategoryApplicationService.GetCategoryListAsync(syncdate);
            IEnumerable<CategoryModel> categoriesModels = Mapper.CategoryMapper.MapToCategoryModels(categories);
            return categoriesModels;
        }

        [HttpPost]
        public async Task<bool> SaveCategories(CategoryModel[] categoryModels, [FromQuery]DateTime? syncdate = null)
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