using SCNDISC.Server.Domain.Aggregates.Categories;
using SCNDISC.Server.Domain.Queries.Categories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCNDISC.Server.Domain.Commands.Categories;

namespace SCNDISC.Server.Application.Services.Categories
{
    public class CategoryApplicationService : ICategoryApplicationService
    {
        private readonly ICategoryListQuery _categoryListQuery;
        private readonly IUpsertCategoryCommand _upsertCategoryCommand;
        private readonly IDeleteCategoryCommand _deleteCategoryCommand;

	    public CategoryApplicationService(ICategoryListQuery categoryListQuery,  IUpsertCategoryCommand upsertCategoryCommand, IDeleteCategoryCommand deleteCategoryCommand)
	    {
	        _categoryListQuery = categoryListQuery;
	        _upsertCategoryCommand = upsertCategoryCommand;
	        _deleteCategoryCommand = deleteCategoryCommand;
	    }

        public async Task<IEnumerable<Category>> GetCategoryListAsync(DateTime? last = null)
        {
            return await _categoryListQuery.GetCategoryListAsync(last);
        }

        public async Task<Category> UpsertCategoryAsync(Category category)
        {
            return await _upsertCategoryCommand.UpsertCategoryAsync(category);
        }

        public async Task DeleteCategoryAsync(string categoryId)
        {
            await _deleteCategoryCommand.DeleteCategoryAsync(categoryId);
        }
    }
}
