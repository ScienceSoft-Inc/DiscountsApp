using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace SCNDISC.Web.Admin
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            if (Request.UserAgent.Contains("AppleWebKit"))
                return;

            if (Request.UserAgent.Contains("Firefox"))
                return;

            Response.Clear();
            Response.Write(
                "<style> * {font-family:Arial;font-size:32px; padding-left:4px; line-height:52px}</style>" +
                "<p>Browser not supported. Please use one of those" +
                "<a href='http://www.opera.com'>Opera</a>, " +
                "<a href='https://www.google.com/chrome'>Google Chrome</a>, " +
                "<a href='https://browser.yandex.com/'>Yandex.Browser</a>, " +
                "<a href='https://www.mozilla.org/en-US/firefox/products/'>Firefox</a>" +
                "</p>" +        
                "<p>Ваш браузер не поддерживается. Установите любой из " +
                "<a href='http://www.opera.com'>Opera</a>, " +
                "<a href='https://www.google.com/chrome'>Google Chrome</a>, " +
                "<a href='https://browser.yandex.com/'>Yandex.Browser</a>, " +
                "<a href='https://www.mozilla.org/en-US/firefox/products/'>Firefox</a>" +
                "</p>");
        }
    }
}