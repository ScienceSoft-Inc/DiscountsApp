using System;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;

namespace SCNDISC.Web.Admin
{
    public partial class Login : Page
    {
        protected void OnLoginClicked(object sender, EventArgs e)
        {
            var username = uxUserName.Text.Trim();
            if (username == ConfigurationManager.AppSettings["DefaultUserName"] && uxPassword.Text.Trim() == ConfigurationManager.AppSettings["DefaultPassword"])
                FormsAuthentication.RedirectFromLoginPage(username, true);
            else
                uxError.Visible = true;
        }
    }
}