using SCNDISC.Server.Domain.Aggregates.Categories;
using System.Threading.Tasks;

namespace SCNDISC.Server.Domain.Commands.Categories
{
    public interface IUpsertCategoryCommand
    {
        Task<Category> UpsertCategoryAsync(Category category);
    }
}
