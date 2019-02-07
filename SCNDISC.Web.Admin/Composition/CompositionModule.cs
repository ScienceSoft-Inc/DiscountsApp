using SCNDISC.Server.Application;
using SCNDISC.Server.Infrastructure.Imaging;
using SCNDISC.Web.Admin.ServiceLayer;

namespace SCNDISC.Web.Admin
{
    public class CompositionModule : ServerModule
    {
        public override void Load()
        {
            Kernel.Bind<ITipsService>().To<TipService>();
            Kernel.Bind<ICategoryService>().To<CategoryService>();
            Kernel.Bind<IPartnersService>().To<PartnerService>();
            Kernel.Bind<IImageConverter>().To<ImageConverter>();
            Kernel.Bind<IWebAddressCategoryService>().To<WebAddressCategoryService>();
            base.Load();
        }
    }
}