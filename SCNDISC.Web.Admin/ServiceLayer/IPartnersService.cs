using System.Collections.Generic;

namespace SCNDISC.Web.Admin.ServiceLayer
{
    public interface IPartnersService
    {
        IEnumerable<TipForm> GetAll();
    }
}