using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Security.Domain;
using Botwave.Security.Service;

public partial class contrib_security_pages_DeleteRole : Botwave.Security.Web.PageBase
{
    private IRoleService roleService = (IRoleService)Ctx.GetObject("roleService");

    public IRoleService RoleService
    {
        set { roleService = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["RoleId"] == null)
        {
            ShowError("当前角色不存在或者参数错误！", AppPath + "contrib/security/pages/roles.aspx?Pid=" + Request.QueryString["Pid"]);
            //throw new Exception("当前角色不存在或者参数错误！");
        }
        Guid roleId = new Guid(Request.QueryString["RoleId"]);
        int count = roleService.GetRoleCountByParentId(roleId);
        if (count > 0)
        {
            ShowError("不能删除角色，因为该角色还存在其他子角色。");
            //throw new Exception("不能删除角色，因为该角色还存在其他子角色。");
        }
        Botwave.XQP.Service.IWorkflowRoleService workflowRoleService = Spring.Context.Support.WebApplicationContext.Current["workflowRoleService"] as Botwave.XQP.Service.IWorkflowRoleService;
        //roleService.DeleteById(roleId);
        string result= workflowRoleService.DeleteRoleByRoleId(roleId);
        WriteNomalLog(CurrentUserName,"删除角色",result);
        ShowSuccess("删除角色成功！", AppPath + "contrib/security/pages/roles.aspx?Pid=" + Request.QueryString["Pid"]);
    }
}
