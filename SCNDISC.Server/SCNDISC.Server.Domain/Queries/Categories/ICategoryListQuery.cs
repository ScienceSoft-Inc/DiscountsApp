using SCNDISC.Server.Domain.Aggregates.Categories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCNDISC.Server.Domain.Queries.Categories
{
    public interface ICategoryListQuery
    {
        Task<IEnumerable<Category>> GetCategoryListAsync(DateTime? syncdate = null);
    }
}
