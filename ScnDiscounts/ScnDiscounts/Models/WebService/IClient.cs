using System.Threading.Tasks;

namespace ScnDiscounts.Models.WebService
{
    interface IClient
    {
        Task<bool> CheckConnection();
    }
}
