<%@ WebHandler Language="C#" Class="RegisterCheck" %>
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Botwave.Security.Domain;
using Botwave.Security.Service;

    /// <summary>
    /// 检查注册用户名处理.
    /// </summary>
public class RegisterCheck : IHttpHandler
{
    private IUserService userService;

    public IUserService UserService
    {
        get { return userService; }
        set { userService = value; }
    }

    public RegisterCheck()
    {
        this.userService = new Botwave.Security.IBatisNet.UserService();
    }

    #region IHttpHandler 成员

    public bool IsReusable
    {
        get { return false; }
    }

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string userName = context.Request.QueryString["name"];

        if (string.IsNullOrEmpty(userName) || userService == null)
        {
            context.Response.Write("null");
        }
        else
        {
            userName = HttpUtility.UrlDecode(userName);
            if (ExistsUserName(userName))
                context.Response.Write("0");
            else
                context.Response.Write("1");
        }
        context.Response.End();
    }

    /// <summary>
    /// 是否存在指定用户名.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private bool ExistsUserName(string name)
    {
        UserInfo result = userService.GetUserByUserName(name);
        return (result != null);
    }

    #endregion
}
