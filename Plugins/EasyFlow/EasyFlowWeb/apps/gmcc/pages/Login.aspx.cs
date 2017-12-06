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
using Botwave.Web;
using Spring.Context.Support;
using Spring.Context;

public partial class apps_gmcc_Login : Botwave.Security.Web.PageBase
{
    /// <summary>
    /// 用户安全目录.
    /// </summary>
    protected static string securityPath = AppPath + "contrib/security/";
    private string defaultRedirectPage = BasePage+"apps/gmcc/pages/default.aspx";
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
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/Base64.js");
        if (!IsPostBack)
        {
            this.btnLogin.ImageUrl = AppPath + "app_themes/gmcc/img/btn_login.gif";
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

    protected void btnLogin_Click(object sender, ImageClickEventArgs e)
    {

        string username = txtUserName.Value.Trim();
        string password = txtPassword.Value.Trim();
        username = DecodeBase64("utf-8",username);
        password = DecodeBase64("utf-8",password);
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

    /// <summary> 
    /// 将字符串使用base64算法解密 
    /// </summary> 
    /// <param name="code_type">编码类型</param> 
    /// <param name="code">已用base64算法加密的字符串</param> 
    /// <returns>解密后的字符串</returns> 
    public string DecodeBase64(string code_type, string code)
    {
        string decode = "";
        byte[] bytes = Convert.FromBase64String(code);  //将2进制编码转换为8位无符号整数数组. 
        try
        {
            decode = System.Text.Encoding.GetEncoding(code_type).GetString(bytes);  //将指定字节数组中的一个字节序列解码为一个字符串。 
        }
        catch
        {
            decode = code;
        }
        return decode;
    }
}
