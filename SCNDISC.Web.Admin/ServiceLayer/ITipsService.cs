using System.Collections.Generic;

namespace SCNDISC.Web.Admin.ServiceLayer
{
    public interface ITipsService
    {
        TipForm Save(TipForm tip);
        void Delete(TipForm tip);
        IEnumerable<TipForm> GetById(string id);
        IEnumerable<TipForm> GetAll(string[] selectedCategories, string partnerName);
        void RemoveAllBranches(string partnerId);
    }
}