using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Botwave.Security;
using Botwave.Security.Domain;
using Botwave.Security.Service;

public partial class contrib_security_pages_Register : Botwave.Web.PageBase
{
    private IUserService userService=(IUserService)Ctx.GetObject("userService");
    private IRoleService roleService= (IRoleService)Ctx.GetObject("roleService");

    public IUserService UserService
    {
        get { return userService; }
        set { userService = value; }
    }

    public IRoleService RoleService
    {
        get { return roleService; }
        set { roleService = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string userName = txtportal.Text.Trim();
        string password = txtpassword.Text;
        string email = txtEmail.Text;
        string realName = txtName.Text.Trim();
        string mobile = txtMobile.Text.Trim();
        string phone = txtPhone.Text.Trim();

        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(realName) 
            || string.IsNullOrEmpty(password))
            ShowError("注册资料必须填写完整.");

        password = Botwave.Commons.TripleDESHelper.Encrypt(password);
        UserInfo item = new UserInfo(userName, password);

        item.RealName = realName;
        item.Email = email;
        item.Mobile = mobile;
        item.Tel = phone;
        item.Status = 0;

        if (userService.GetUserByUserName(userName) == null)
        {
            password = Botwave.Commons.TripleDESHelper.Encrypt(password);
            userService.InsertUser(item);
            if (roleService != null)
            {
                IList<Guid> roles = new List<Guid>();
                roles.Add(new Guid("{08154B8C-8A53-4735-A751-2B415B135FAB}"));
                roleService.InsertUserRoles(item.UserId, roles);
            }
            LoginHelper.UserName = userName; // 设置为登录用户.
            ShowSuccess("注册成功.", AppPath + "default.aspx");
        }
        else
        {
            ShowError("注册失败，帐号已经存在.");
        }
    }
}
