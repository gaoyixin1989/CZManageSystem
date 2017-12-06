using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Web;
using Botwave.Security;
using Botwave.Security.Domain;
using Botwave.Security.Service;

public partial class contrib_security_pages_EditUser : Botwave.Security.Web.PageBase
{
    public string CommandText = "新增";
    private IUserService userService = (IUserService)Ctx.GetObject("userService");
    private IDepartmentService departmentService = (IDepartmentService)Ctx.GetObject("departmentService");
    private IRoleService roleService = (IRoleService)Ctx.GetObject("roleService");

    public IUserService UserService
    {
        set { userService = value; }
    }

    public IDepartmentService DepartmentService
    {
        set { departmentService = value; }
    }

    public IRoleService RoleService
    {
        set { roleService = value; }
    }

    public Guid? UserId
    {
        get
        {
            if (ViewState["UserId"] != null)
                return new Guid(ViewState["UserId"].ToString());
            return null;
        }
        set
        {
            if (value.HasValue)
                ViewState["UserId"] = value.Value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtUserName.Attributes["onkeyup"] = "autoSetEmail(this)"; // 自动设置电子邮箱的客户端脚本
            if (Request.QueryString["UserId"] != null)
            {
                UserId = new Guid(Request.QueryString["UserId"]);
                this.CommandText = "编辑";
                this.btnEdit.CssClass = "btn_sav";
                this.btnEdit.Text = "保存";
                this.ltlTitle.Text = "编辑";
                this.LoadData();
            }
            //if (string.IsNullOrEmpty(this.txtEmployeeId.Text))
            //{
            //    this.txtEmployeeId.Text = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
            //}
        }
    }

    protected void LoadData()
    {
        if (UserId.HasValue)
        {
            UserInfo item = userService.GetUserByUserId(UserId.Value);
            this.trPassword.Visible = false;

            string department = item.DpFullName;

            this.txtUserName.Text = item.UserName;
            this.txtRealName.Text = item.RealName;
            this.txtEmployeeId.Text = item.EmployeeId;
            this.txtDepartment.Text = (string.IsNullOrEmpty(department) ? item.DpId : department);
            this.txtTel.Text = item.Tel;
            this.txtMobile.Text = item.Mobile;
            this.txtEmail.Text = item.Email;
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        LoginUser currentUser = LoginHelper.User;
        string creator = currentUser.UserName;
        UserInfo item = new UserInfo();
        string userName = txtUserName.Text.Trim().ToLower();
        item.UserName = userName;

        string depart = txtDepartment.Text.Trim();
        if (!string.IsNullOrEmpty(depart))
        {
            Department dp = departmentService.GetDepartmentByFullName(depart);
            if (dp != null)
                depart = dp.DpId;
        }

        item.DpId = depart;
        item.RealName = txtRealName.Text.Trim();
        item.EmployeeId = txtEmployeeId.Text.Trim();
        item.Tel = txtTel.Text.Trim();
        item.Mobile = txtMobile.Text.Trim();
        item.Email = txtEmail.Text.Trim();
        item.Creator = creator;
        item.LastModifier = creator;
        item.Status = 0;
        item.Type = 1;

        if (UserId == null)
        {
            item.UserId = Guid.NewGuid();
            string password = txtPassword.Text;
            password = Botwave.Commons.TripleDESHelper.Encrypt(password);
            item.Password = password;
            // 新增
            if (userService.UserIsExists(userName))
            {
                ShowError("用户已经存在，请重新填写.");
            }

            userService.InsertUser(item);

            //给新增的用户分配默认角色（2009-04-27）
            RoleInfo roleInfo = roleService.GetRoleByName("外部用户");
            if (null != roleInfo)
            {
                Guid roleId = roleInfo.RoleId;
                IList<Guid> userIds = new List<Guid>();
                userIds.Add(item.UserId);
                roleService.InsertUserRoles(userIds, roleId);
            }

            //WriteNomalLog(creator, "编辑用户", string.Format("新增用户 {0} 成功.", userName));
        }
        else
        {
            item.UserId = UserId.Value;
            // 修改用户
            userService.UpdateUser(item);
            //WriteNomalLog(creator, "编辑用户", string.Format("编辑用户 {0} 成功.", userName));
        }
        ShowSuccess(MessageHelper.Message_Success, AppPath + "contrib/security/pages/users.aspx");
    }
}
