using System.Collections.Generic;

namespace SCNDISC.Web.Admin.ServiceLayer
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAll();
        Category Save(Category category);
        void Delete(Category category);
    }
}