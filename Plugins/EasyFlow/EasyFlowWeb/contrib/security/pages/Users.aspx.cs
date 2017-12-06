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

using Botwave.Web;
using Botwave.Security;
using Botwave.Security.Domain;
using Botwave.Security.Service;
using System.Data.SqlClient;
using System.Text;
using Botwave.Commons;

public partial class contrib_security_pages_Users : Botwave.Security.Web.PageBase
{
    protected string keywords;
    protected Guid? roleId;

    private IUserService userService = (IUserService)Ctx.GetObject("userService");
    private IRoleService roleService = (IRoleService)Ctx.GetObject("roleService");
    private IDepartmentService departmentService = (IDepartmentService)Ctx.GetObject("departmentService");

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
    public IDepartmentService DepartmentService
    {
        set { departmentService = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["q"] != null)
                keywords = Request.QueryString["q"];
            if (Request.QueryString["RoleId"] != null)
                roleId = new Guid(Request.QueryString["RoleId"]);
            if (!string.IsNullOrEmpty(Request.QueryString["Pid"]))
                btnBack.Visible = true;
            else
                btnBack.Visible = false;
            this.ControlsInitialize();
            this.SearchUsers2(0, 0);
        }
    }

    private void ControlsInitialize()
    {
        IList<RoleInfo> roles = roleService.GetRolesByEnabled();
        roles.Insert(0, new RoleInfo("全部"));
        droplistRoles.DataSource = roles;
        droplistRoles.DataTextField = "RoleName";
        droplistRoles.DataValueField = "RoleId";
        droplistRoles.DataBind();

        // 设置下拉列表被选择的索引.
        if (roleId != null)
        {
            int roleCount = roles.Count;
            for (int i = 0; i < roleCount; i++)
            {
                if (roles[i].RoleId == roleId.Value)
                    droplistRoles.SelectedIndex = i;
            }
        }
    }

    protected void listPager_PageIndexChanged(object sender, Botwave.Web.Controls.PageChangedEventArgs e)
    {
        this.SearchUsers2(listPager.TotalRecordCount, e.NewPageIndex);
    }

    private void SearchUsers2(int recordCount, int pageIndex)
    {
        keywords = string.Empty;
        roleId = null;
        if (Request.QueryString["q"] != null)
            keywords = Request.QueryString["q"];
        if (Request.QueryString["RoleId"] != null)
            roleId = new Guid(Request.QueryString["RoleId"]);

        DataTable results; //userService.GetUsersByPager(keywords, roleId, pageIndex, listPager.ItemsPerPage, ref recordCount);
        results = Botwave.Workflow.Practices.CZMCC.Support.UserService.Current.GetUsersByPager(keywords, roleId, pageIndex, listPager.ItemsPerPage, ref recordCount);
        this.usersRepeater.DataSource = results;
        this.usersRepeater.DataBind();
        listPager.TotalRecordCount = recordCount;
        listPager.DataBind();
    }

    public string SqlBulkCopy(DataTable dt)
    {
        int errorResult = 0;
        int affect = 0;
        string insertReault = string.Empty;
        StringBuilder sError=new StringBuilder();
        try
        {
            int index=1;
            foreach (DataRow row in dt.Rows)
            {
                string creator = CurrentUserName;
                UserInfo item = new UserInfo();
                string userName = DbUtils.ToString(row["用户名"]);
                item.UserName = userName;

                string depart = DbUtils.ToString(row["所属部门"]);
                if (!string.IsNullOrEmpty(depart))
                {
                    depart = "潮州分公司>" + depart;
                    Department dp = departmentService.GetDepartmentByFullName(depart);
                    if (dp != null)
                        depart = dp.DpId;
                }

                item.DpId = depart;
                item.RealName = DbUtils.ToString(row["真实姓名"]);
                item.EmployeeId = DbUtils.ToString(row["工号"]);
                item.Tel = DbUtils.ToString(row["固定电话"]);
                item.Mobile = DbUtils.ToString(row["手机"]);
                item.Email = DbUtils.ToString(row["电子邮箱"]);
                item.Creator = creator;
                item.LastModifier = creator;
                item.Status = 0;
                item.Type = 1;
                // 新增
                if (userService.UserIsExists(userName))
                {
                    errorResult++;
                    sError.AppendFormat("第{0}行：用户名已存在。</br>",index);
                    index++;
                    continue;
                }

                item.UserId = Guid.NewGuid();
                string password = DbUtils.ToString(row["密码"]);
                password = Botwave.Commons.TripleDESHelper.Encrypt(password);
                item.Password = password;


                userService.InsertUser(item);
                Guid newRowid = Guid.Empty;
                string rroleId = DbUtils.ToString(row["角色ID"]);
                try
                {
                    newRowid = new Guid(rroleId);
                }
                catch {
                    sError.AppendFormat("第{0}行：角色ID应为36位的GUID。</br>",index);
                    index++;
                    continue;
                }
                RoleInfo myRoleInfo = roleService.GetRoleById(newRowid);
                if(roleService.GetRoleById(newRowid)==null)
                {
                    sError.AppendFormat("第{0}行：角色ID对应的角色不存在。</br>", index);
                    index++;
                    continue;
                }
                if (myRoleInfo.RoleName == "内部用户")
                {
                    sError.AppendFormat("第{0}行：角色ID对应的角色【内部用户】不允许分配。</br>", index);
                    index++;
                    continue;
                }
                //给新增的用户分配默认角色（2009-04-27）

                RoleInfo roleInfo = roleService.GetRoleByName("外部用户");
                if (null != roleInfo)
                {
                    Guid roleId = roleInfo.RoleId;
                    IList<Guid> roleIds = new List<Guid>();
                    roleIds.Add(roleId);
                    if(roleId!=newRowid)
                    roleIds.Add(newRowid);
                    roleService.InsertUserRoles(item.UserId, roleIds);
                }

                WriteNomalLog(creator, "编辑用户", string.Format("导入新增用户 {0} 成功.", userName));
            }
        }
        catch (Exception ex)
        {
            WriteExceptionLog(CurrentUserName, "异常操作", "导入用户信息异常：" + ex.ToString());
            throw ex;
        }
        return "更新成功！" + affect + "条" + "，失败" + errorResult + "条 " + "</br>" + insertReault + "</br>" + sError.ToString();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        keywords = Request.Form["txtKeyword"];
        string query = string.Empty;
        if (!string.IsNullOrEmpty(keywords))
            query = string.Format("q={0}", HttpUtility.UrlEncode(keywords));
        if (droplistRoles.SelectedIndex != 0)
        {
            if (query.Length > 0)
                query += "&";
            query += string.Format("RoleId={0}", droplistRoles.SelectedValue);
        }
        if (query.Length > 0)
            Response.Redirect("users.aspx?" + query);
        else
            Response.Redirect("users.aspx");
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect(AppPath + "contrib/security/pages/roles.aspx?Pid=" + Request.QueryString["Pid"]);
    }

    protected void usersRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            // 删除
            Guid userId = new Guid(e.CommandArgument.ToString());
            if (userService.DeleteByUserId(userId) > 0)
            {
                ShowSuccess("删除用户成功！");
            }
            else
            {
                ShowError("删除用户失败。");
                //throw new Exception("删除用户失败。");
            }
        }
        else if (e.CommandName == "Disabled")
        {
            // 删除
            Guid userId = new Guid(e.CommandArgument.ToString());
            UserInfo uinfo = userService.GetUserByUserId(userId);
            if (uinfo != null)
            {
                uinfo.Status = -1;
                SqlParameter[] param = { new SqlParameter("@userid",uinfo.UserId),
                                       new SqlParameter("@LastModifier","admin")};
                Botwave.Extension.IBatisNet.IBatisDbHelper.ExecuteNonQuery(CommandType.Text, "update bw_users set status=-1,LastModifier=@LastModifier,lastmodtime=getdate() where userid=@userid",param);
                 ShowSuccess("禁用用户成功！");
            }
            else
            {
                ShowError("禁用用户失败。");
                //throw new Exception("删除用户失败。");
            }
        }
        else if (e.CommandName == "Enabled")
        {
            // 删除
            Guid userId = new Guid(e.CommandArgument.ToString());
            UserInfo uinfo = userService.GetUserByUserId(userId);
            if (uinfo != null)
            {
                uinfo.Status = 0;
                SqlParameter[] param = { new SqlParameter("@userid",uinfo.UserId),
                                       new SqlParameter("@LastModifier","admin")};
                Botwave.Extension.IBatisNet.IBatisDbHelper.ExecuteNonQuery(CommandType.Text, "update bw_users set status=0,LastModifier=@LastModifier,lastmodtime=getdate() where userid=@userid",param);
                    ShowSuccess("启用用户成功！");
            }
            else
            {
                ShowError("启用用户失败。");
                //throw new Exception("删除用户失败。");
            }
        }
        else if (e.CommandName == "Login")
        {
            string newUser = e.CommandArgument.ToString();
            Botwave.XQP.Commons.LogWriter.Write(LoginHelper.UserName, "模拟用户登录", newUser);
            LoginHelper.UserName = newUser;
            WebUtils.RedirectParent(this.Response, "default.aspx");
        }
    }

    // 收回选择用户的当前角色

    protected void btnRecycle_Click(object sender, EventArgs e)
    {
        string checkboxs = Request.Form["chkbox_user"];
        if (string.IsNullOrEmpty(checkboxs))
        {
            ShowError("请选择要收回角色的用户。");
            //throw new Exception("请选择要收回角色的用户。");
        }

        Guid roleId = new Guid(droplistRoles.SelectedValue);
        if (roleId == Guid.Empty || Request.QueryString["roleId"] == null)
        {
            ShowError("不能删除全部用户的角色，请选择角色查询并在结果中选择用户。");
            //throw new Exception("不能删除全部用户的角色，请选择角色查询并在结果中选择用户。");
        }


        string[] userArray = checkboxs.Replace(" ", "").Split(',', '，');
        IList<Guid> users = new List<Guid>();
        foreach (string item in userArray)
        {
            users.Add(new Guid(item));
        }

        roleService.RecycleRoles(roleId, users);
        ShowSuccess("成功收回角色！");
    }
    protected void usersRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemIndex < 0)
            return;
        DataRowView item = e.Item.DataItem as DataRowView;
        LinkButton btnEnabled = e.Item.FindControl("btnEnabled") as LinkButton;
        LinkButton btnDisabied = e.Item.FindControl("btnDisabied") as LinkButton;
        int status = Convert.ToInt32(item["status"]);
        if (status == -1)
            btnDisabied.Visible = false;
        else
            btnEnabled.Visible = false;
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        if (this.file.PostedFile != null)
        {
            string type = file.PostedFile.FileName.Substring(file.PostedFile.FileName.LastIndexOf("."));//获取类型
            string ExcelUrl = "~/App_Data/Temp/" + Guid.NewGuid().ToString() + type;
            file.PostedFile.SaveAs(Server.MapPath(ExcelUrl));
            DataTable dtReault = Botwave.XQP.Commons.ExcelHelper.ReadExcel(Server.MapPath(ExcelUrl));
            DataSet dsResult = new DataSet();
            dsResult.Tables.Add(dtReault);
            StringBuilder sbError = new StringBuilder();
            string resultStr = string.Empty;
            if (dsResult == null)
            {
                ShowError("没有数据");
            }
            else
            {
                try
                {
                    resultStr = SqlBulkCopy(dsResult.Tables[0]);
                    WriteNomalLog(CurrentUserName, "编辑用户", "导入用户信息：" + resultStr);
                    
                }
                catch (Exception ex)
                {
                    WriteExceptionLog(CurrentUserName, "异常操作", "导入用户信息异常：" + ex.ToString());
                    ShowError("导入失败！请联系管理员");
                }
            }
        }
        ShowSuccess("导入成功，详细信息请查看操作日志");
    }
}
