using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Security;
using Botwave.Security.Domain;
using Botwave.Security.Service;

public partial class contrib_security_pages_ChangePassword : Botwave.Web.PageBase
{
    private IUserService userService = (IUserService)Ctx.GetObject("userService");

    public IUserService UserService
    {
        get { return userService; }
        set { userService = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    { }

    protected void btnChangePassword_Click(object sender, EventArgs e)
    {
        LoginUser currentUser = LoginHelper.User;
        string userName = currentUser.UserName;

        //string oldPassword = textOldPassword.Text;
        string newPassword = textNewPassword.Text;

        if (userService.ChangePassword(userName, null, newPassword) == 1)
        {
            //ShowSuccess("修改密码成功！");
        }
        else
        {
            //ShowError("原密码不正确！");
        }
    }
}
