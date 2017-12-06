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
using Botwave.Security.Extension.Web;
using IBatisNet;
using IBatisNet.Common;
using IBatisNet.DataMapper;
using common = IBatisNet.Common;
using dataMapper = IBatisNet.DataMapper;

public partial class test_Default : Botwave.Web.PageBase
{
    private IAuthService authService;

    public IAuthService AuthService
    {
        set { authService = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["conn"] != null)
            {
                Response.Write("ConnectionString:" + Botwave.Extension.IBatisNet.IBatisDbHelper.GetConnectionString());
            }
        }
    }

    private void loadPage()
    {
        string userName = SecurityHelper.GetLoginName();

        //if (string.IsNullOrEmpty(userName))
        //{
            Botwave.Security.LoginStatus loginStatus = authService.Login("admin", "password");

            if (loginStatus == Botwave.Security.LoginStatus.Success)
            {
                SecurityHelper.SetLoginName("admin");
            }
            else
            {
                string errorInfo = "登录未成功,原因如下:" + Botwave.Security.LoginStatusHelper.ToDescription(loginStatus);
                ShowError(errorInfo);
            }
        //}
    }
}
