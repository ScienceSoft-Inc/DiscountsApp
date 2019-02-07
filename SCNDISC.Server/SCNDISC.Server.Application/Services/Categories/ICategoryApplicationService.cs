using SCNDISC.Server.Domain.Aggregates.Categories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCNDISC.Server.Application.Services.Categories
{
    public interface ICategoryApplicationService
    {
        Task<IEnumerable<Category>> GetCategoryListAsync(DateTime? last = null);
        Task<Category> UpsertCategoryAsync(Category category);
        Task DeleteCategoryAsync(string categoryId);
    }
}
