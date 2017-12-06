using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Botwave.Security;
using Spring.Context.Support;

public partial class contrib_security_pages_Logout : System.Web.UI.Page
{
    private string redirectPage = "Login.aspx";
    private string mobilePage = Botwave.Web.WebUtils.GetAppPath() + "apps/gmcc/pages/loginmobile.aspx";
    private IAuthService authService = (IAuthService)ContextRegistry.GetContext().GetObject("authService");

    public string RedirectPage
    {
        set { redirectPage = value; }
    }

    public IAuthService AuthService
    {
        set { authService = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string username = "anonymous";
        authService.Logout(username);
        if (Request.QueryString["type"] == "mobile")
        {
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            HttpContext.Current.Request.Cookies.Clear();
            Response.Redirect(mobilePage);
        }
        else
            Response.Redirect(redirectPage);
    }
}
