using System;
using System.Linq;
using System.Web;
using Microsoft.Practices.ServiceLocation;
using SCNDISC.Web.Admin.ServiceLayer;

namespace SCNDISC.Web.Admin
{
    public class GetImage : IHttpHandler
    {
        private bool IsLogoRequest
        {
            get { return !String.IsNullOrEmpty(HttpContext.Current.Request["logo"]); }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "image";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            TipForm tip = ServiceLocator.Current.GetInstance<ITipsService>().GetById(context.Request["id"]).First(x => x.PartnerId == x.Id);
            context.Response.BinaryWrite(IsLogoRequest
                ? Convert.FromBase64String(tip.Icon)
                : Convert.FromBase64String(tip.Image));
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}