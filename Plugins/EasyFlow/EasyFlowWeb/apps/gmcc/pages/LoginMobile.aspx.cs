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
using Botwave.Web.HttpHandler;

public partial class apps_gmcc_pages_LoginMobile : Botwave.XQP.Web.Security.PageBase
{
    /// <summary>
    /// 用户安全目录.
    /// </summary>
    protected static string securityPath = AppPath + "contrib/security/";
    private string defaultRedirectPage = "~/contrib/mobile/pages/default.aspx";

    protected string guid { get { return (string)ViewState["guid"]; } set { ViewState["guid"] = value; } }

    protected string randomStr
    {
        get { return (string)ViewState["randomStr"]; }
        set { ViewState["randomStr"] = value; }
    }

    protected string textfield
    {
        get { return (string)ViewState["textfield"]; }
        set { ViewState["textfield"] = value; }
    }

    protected string password1
    {
        get { return (string)ViewState["password1"]; }
        set { ViewState["password1"] = value; }
    }

    protected string txtRoom
    {
        get { return (string)ViewState["txtRoom"]; }
        set { ViewState["txtRoom"] = value; }
    }

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
        Botwave.Web.WebUtils.RegisterScriptReference(this.Page, Botwave.Web.WebUtils.GetAppPath() + "apps/gmcc/scripts/jQuery.md5.js");
        if (!IsPostBack)
        {
            this.Title = string.Format("{0} - 系统登录", AppName);
            // cookie 中存在用户时
            string userName = SecurityHelper.GetLoginName();
            //if (!string.IsNullOrEmpty(userName))
            //{
            //    this.txtUserName.Value = userName;
            //    this.txtPassword.Focus();
            //}
            //else
            //{
            //    this.txtUserName.Focus();
            //}
            guid = Guid.NewGuid().ToString();
            textfield = Botwave.XQP.Commons.XQPHelper.md5(Guid.NewGuid().ToString());
            password1 = Botwave.XQP.Commons.XQPHelper.md5(Guid.NewGuid().ToString());
            txtRoom = Botwave.XQP.Commons.XQPHelper.md5(Guid.NewGuid().ToString());
            randomStr = GenerateCheckCode(10);
            txtCode.Value = Botwave.Commons.TripleDESHelper.Encrypt(randomStr);
        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (!this.VerifyCheckCode())
        {
            txtCheckCode.Value = string.Empty;
            return;
        }
        Session.Clear();
        Session.Abandon();
        Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
        //Session["Ucs_CheckCode_RandomCode"] = randomStr;
        //string username = txtUserName.Value.Trim();
        //string password = txtPassword.Value.Trim();
        //username = DecodeBase64("utf-8", username);
        //password = DecodeBase64("utf-8", password);
        Session["Ucs_CheckCode_RandomCode"] = randomStr;
        //string username = txtUserName.Value.Trim();
        string pwd = Request.Form[txtRoom];
        pwd = DecodeBase64("utf-8", pwd);
        string username = string.Empty;
        string password = string.Empty;
        if (pwd.Length < 64)
        {
            Session.Remove("Ucs_CheckCode_RandomCode");
            txtCheckCode.Value = string.Empty;
            string errorInfo = "登录失败，令牌非法";
            divMessage.InnerHtml = errorInfo;
            divMessage.Visible = true;
            divMessage.Attributes.Remove("style");
            textfield = Botwave.XQP.Commons.XQPHelper.md5(Guid.NewGuid().ToString());
            password1 = Botwave.XQP.Commons.XQPHelper.md5(Guid.NewGuid().ToString());
            txtRoom = Botwave.XQP.Commons.XQPHelper.md5(Guid.NewGuid().ToString());
            randomStr = GenerateCheckCode(10);
            txtCode.Value = Botwave.Commons.TripleDESHelper.Encrypt(randomStr);
        }
        else
        {
            string[] rstrArr = System.Text.RegularExpressions.Regex.Split(pwd, Botwave.XQP.Commons.XQPHelper.md5(randomStr), System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            username = rstrArr[0];
            password = rstrArr[1].Substring(0, rstrArr[1].Length - 32);
        }
        if (username.Length > 0 && password.Length > 0)
        {
            Botwave.Security.LoginStatus loginStatus = authService.Login(username, password);
            //txtPassword.Value = string.Empty;
            Session["Ucs_CheckCode_RandomCode"] = null;
            if (loginStatus == Botwave.Security.LoginStatus.Success)
            {
                SecurityHelper.SetLoginName(username);
                RedirectPage();
            }
            else
            {
                //txtUserName.Value = username;
                txtCheckCode.Value = string.Empty;
                string errorInfo = "登录未成功,原因如下:" + Botwave.Security.LoginStatusHelper.ToDescription(loginStatus);
                divMessage.Attributes.Remove("style");
                divMessage.InnerHtml = errorInfo;
            }
        }
        else
        {
            //txtUserName.Value = username;
            txtCheckCode.Value = string.Empty;
            divMessage.Attributes.Remove("style");
            divMessage.InnerHtml = "登录失败，请输入密码！";
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

    private bool VerifyCheckCode()
    {
        this.divMessage.Visible = true;
        if (this.Session[CheckCodeHandler.CheckCode_Key] == null)
        {
            this.divMessage.InnerHtml = "验证码无效！";
            divMessage.Attributes.Remove("style");
            return false;
        }
        string checkedCode = this.Session[CheckCodeHandler.CheckCode_Key].ToString();
        checkedCode = Botwave.XQP.Commons.XQPHelper.md5(checkedCode);
        if (!checkedCode.Equals(this.txtCheckCode.Value, StringComparison.OrdinalIgnoreCase))
        {
            textfield = Botwave.XQP.Commons.XQPHelper.md5(Guid.NewGuid().ToString());
            password1 = Botwave.XQP.Commons.XQPHelper.md5(Guid.NewGuid().ToString());
            txtRoom = Botwave.XQP.Commons.XQPHelper.md5(Guid.NewGuid().ToString());
            this.txtCode.Value = string.Empty;
            randomStr = GenerateCheckCode(10);
            txtCheckCode.Value = string.Empty;
            txtCode.Value = Botwave.Commons.TripleDESHelper.Encrypt(randomStr);
            this.divMessage.InnerHtml = "请输入正确的验证码！";
            divMessage.Attributes.Remove("style");
            return false;
        }
        string checkCode = Botwave.Commons.TripleDESHelper.Decrypt(txtCode.Value);
        checkCode = Botwave.XQP.Commons.XQPHelper.md5(checkCode);
        string verifyCode = Botwave.XQP.Commons.XQPHelper.md5(randomStr);
        if (!checkCode.Equals(verifyCode, StringComparison.OrdinalIgnoreCase))
        {
            textfield = Botwave.XQP.Commons.XQPHelper.md5(Guid.NewGuid().ToString());
            password1 = Botwave.XQP.Commons.XQPHelper.md5(Guid.NewGuid().ToString());
            txtRoom = Botwave.XQP.Commons.XQPHelper.md5(Guid.NewGuid().ToString());
            this.txtCode.Value = string.Empty;
            randomStr = GenerateCheckCode(10);
            txtCode.Value = Botwave.Commons.TripleDESHelper.Encrypt(randomStr);
            this.divMessage.InnerHtml = "请输入正确的登录信息！";
            divMessage.Attributes.Remove("style");
            return false;
        }

        return true;
    }

    /// <summary>
    /// 生成验证码.
    /// </summary>
    /// <param name="length">验证码的长度.</param>
    /// <returns></returns>
    private string GenerateCheckCode(int length)
    {
        string checkCode = string.Empty;

        Random random = new Random();
        for (int i = 0; i < length; i++)
        {
            int number = random.Next();
            char code;
            if (number % 2 == 0)
                code = (char)('0' + (char)(number % 10));
            else if (number % 3 == 0)
                code = (char)('a' + (char)(number % 26));
            else
                code = (char)('A' + (char)(number % 26));
            checkCode += code.ToString();
        }
        return Botwave.XQP.Commons.XQPHelper.md5(checkCode);
    }
}