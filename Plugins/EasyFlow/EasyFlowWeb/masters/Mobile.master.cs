using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using Botwave.Web;

public partial class masters_Mobile : Botwave.Web.MasterPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session.Keys.Count == 0 || HttpContext.Current.Request.Cookies["ASP.NET_SessionId"] == null)
        //    Response.Redirect(WebUtils.GetAppPath() + "apps/gmcc/pages/loginmobile.aspx");
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e); 
        if (Botwave.Security.LoginHelper.UserName == "anonymous" || HttpContext.Current.Request.Cookies[Botwave.Security.LoginHelper.UserCookieKey]==null)
        {
            //Botwave.Security.LoginHelper.RemoveUserCache();
            //HttpContext.Current.Request.Cookies.Clear();
            Response.Redirect(WebUtils.GetAppPath() + "apps/gmcc/pages/loginmobile.aspx");
        }
    }
}
