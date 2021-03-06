﻿using System;
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

public partial class contrib_security_pages_Login : Botwave.Web.PageBase
{
    /// <summary>
    /// 用户安全目录.
    /// </summary>
    protected static string securityPath = AppPath + "contrib/security/";
    private string defaultRedirectPage = "~/default.aspx";
    private IAuthService authService= (IAuthService)Ctx.GetObject("authService");

    public IAuthService AuthService
    {
        set { authService = value; }
    }

    public string DefaultRedirectPage
    {
        set { defaultRedirectPage = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.Title = string.Format("{0} - 系统登录", AppName);
            // cookie 中存在用户时
            string userName = SecurityHelper.GetLoginName();
            if (!string.IsNullOrEmpty(userName))
            {
                this.txtUserName.Value = userName;
                this.txtPassword.Focus();
            }
            else
            {
                this.txtUserName.Focus();
            }
        }
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string username = txtUserName.Value.Trim();
        string password = txtPassword.Value.Trim();
        if (username.Length > 0 && password.Length > 0)
        {
            Botwave.Security.LoginStatus loginStatus = authService.Login(username, password);
            if (loginStatus == Botwave.Security.LoginStatus.Success)
            {
                SecurityHelper.SetLoginName(username);
                RedirectPage();
            }
            else
            {
                string errorInfo = "登录未成功,原因如下:" + Botwave.Security.LoginStatusHelper.ToDescription(loginStatus);
                ShowError(errorInfo);
            }
        }
        else
        {
            divMessage.InnerHtml = "<font color='red' style='font-size:12px'>登录失败，请输入密码！</font>";
        }
    }

    /// <summary>
    /// 登录成功后，重定向页面.
    /// </summary>
    private void RedirectPage()
    {
        string url = this.Request.QueryString["returnUrl"];
        //if (string.IsNullOrEmpty(url))
        //{
        //    url = this.Request.UrlReferrer.PathAndQuery;
        //}
        if (!string.IsNullOrEmpty(url))
        {
            Response.Redirect(url);
        }
        else
        {
            Response.Redirect(defaultRedirectPage);
        }
    }
}
