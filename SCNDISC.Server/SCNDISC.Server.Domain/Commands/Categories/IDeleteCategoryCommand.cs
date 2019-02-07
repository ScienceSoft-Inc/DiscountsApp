using System.Threading.Tasks;

namespace SCNDISC.Server.Domain.Commands.Categories
{
    public interface IDeleteCategoryCommand
    {
        Task DeleteCategoryAsync(string categoryId);
    }
}
