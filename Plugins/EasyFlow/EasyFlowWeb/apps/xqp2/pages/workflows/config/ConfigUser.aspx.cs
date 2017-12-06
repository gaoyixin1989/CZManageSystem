using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.Security.Domain;
using Botwave.Security.Service;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Util;
using Botwave.Workflow.Extension.Service;
using Botwave.XQP.Domain;
using Botwave.XQP.Commons;

public partial class apps_xqp2_workflows_config_ConfigUser : Botwave.Security.Web.PageBase
{
    private static string ReturnUrl = AppPath + "apps/xqp2/pages/workflows/config/configUser.aspx";
    private IWorkflowResourceService workflowResourceService = (IWorkflowResourceService)Ctx.GetObject("workflowResourceService");
    /// <summary>
    /// 流程资源服务.
    /// </summary>
    public IWorkflowResourceService WorkflowResourceService
    {
        get { return workflowResourceService; }
        set { workflowResourceService = value; }
    }

    /// <summary>
    /// 用户编号.
    /// </summary>
    public Guid UserId
    {
        get { return (Guid)ViewState["UserId"]; }
        set { ViewState["UserId"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Botwave.Security.LoginUser user = CurrentUser;
            Guid userId = user.UserId;

            this.UserId = userId;
            this.LoadWorkflowRemarks(userId);
            this.LoadWorklfowNotifies(user, userId);
        }
    }

    private void LoadWorkflowRemarks(Guid userId)
    {
        IList<WorkflowRemark> remarks = WorkflowRemark.SelectByUserId(userId);
        gridviewRemarks.DataSource = remarks;
        gridviewRemarks.DataBind();
    }

    private void LoadWorklfowNotifies(Botwave.Security.LoginUser user, Guid userId)
    {
        DataTable dt = WorkflowNotify.GetNotifyTable(userId);
        IDictionary<string, string> userResources = user.Resources;
        IDictionary<string, string> workflowResources = workflowResourceService.GetWorkflowResources(ResourceHelper.Workflow_ResourceId, ResourceHelper.ResourceType_Workflow);
        string workflowName;
        for(int i=0;i<dt.Rows.Count;i++)
        {
            workflowName = dt.Rows[i]["WorkflowName"].ToString().ToLower();
            if (workflowResources.ContainsKey(workflowName))
            {
                string requireResouceId = workflowResources[workflowName] + "0000"; // 取得指定流程公用权限.
                if (!XQPHelper.VerifyAccessResource(userResources, requireResouceId))
                {
                    dt.Rows[i].Delete();
                }
            }
        }
        gviewNotify.DataSource = dt;
        gviewNotify.DataBind();
    }

    #region 审批意见

    protected void gridviewRemarks_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // 设置序号
            Literal ltlNumber = e.Row.FindControl("ltlNumber") as Literal;
            ltlNumber.Text = (e.Row.RowIndex + 1).ToString();
            LinkButton linkbtnDelete = e.Row.FindControl("linkbtnDelete") as LinkButton;

            linkbtnDelete.Attributes.Add("onclick", "return confirm('确定要删除审批意见吗?')");
        }
    }

    protected void gridviewRemarks_RowEditing(object sender, GridViewEditEventArgs e)
    {
        this.gridviewRemarks.EditIndex = e.NewEditIndex;
        this.LoadWorkflowRemarks(this.UserId);
    }

    protected void gridviewRemarks_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        this.gridviewRemarks.EditIndex = -1;
        this.LoadWorkflowRemarks(this.UserId);
    }

    protected void gridviewRemarks_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        TextBox txtRemarkText = this.gridviewRemarks.Rows[e.RowIndex].FindControl("txtRemarkText") as TextBox;
        TextBox txtRemarkValue = this.gridviewRemarks.Rows[e.RowIndex].FindControl("txtRemarkValue") as TextBox;
        if (txtRemarkText != null && txtRemarkValue != null)
        {
            string remarkText = txtRemarkText.Text;
            string remarkValue = txtRemarkValue.Text;

            DataKeyArray keys = this.gridviewRemarks.DataKeys;
            int id = int.Parse(keys[e.RowIndex].Values["Id"].ToString());

            if (WorkflowRemark.Update(id, remarkText, remarkValue) > 0)
                this.ltlRemarkMsg.Text = "<font color=green>更新成功!</font>";
            else
                this.ltlRemarkMsg.Text = "<font color=red>更新失败!</font>";
        }
        else
        {
            this.ltlRemarkMsg.Text = "<font color=red>更新失败!</font>";
        }

        this.gridviewRemarks.EditIndex = -1;
        this.LoadWorkflowRemarks(this.UserId);
    }

    protected void gridviewRemarks_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataKeyArray keys = this.gridviewRemarks.DataKeys;
        int id = int.Parse(keys[e.RowIndex].Values["Id"].ToString());

        if (WorkflowRemark.Delete(id) > 0)
            this.ltlRemarkMsg.Text = "<font color=green>删除成功!</font>";
        else
            this.ltlRemarkMsg.Text = "<font color=red>删除失败!</font>";
        this.LoadWorkflowRemarks(this.UserId);
    }

    #endregion

    protected void gviewNotify_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // 设置序号
            Literal ltlNumber = e.Row.FindControl("ltlNumber") as Literal;
            ltlNumber.Text = (e.Row.RowIndex + 1).ToString();
        }
    }

    /// <summary>
    /// 插入审批意见.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnInsertRemark_Click(object sender, EventArgs e)
    {
        WorkflowRemark remark = new WorkflowRemark();
        remark.DisplayOrder = 1;
        remark.RemarkText = txtRemarkText.Text;
        remark.RemarkValue = txtRemarkValue.Text;
        remark.UserId = CurrentUser.UserId;

        if (remark.IsExists())
            ShowError("对不起，当前审批意见已经存在.", ReturnUrl);

        remark.Insert();
        ShowSuccess("新增审批意见成功！", ReturnUrl);
    }

    /// <summary>
    /// 保存通知设置.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveNotify_Click(object sender, EventArgs e)
    {
        SaveNotify();
    }

    private void SaveNotify()
    {
        GridViewRowCollection rows = this.gviewNotify.Rows;
        DataKeyArray keys = this.gviewNotify.DataKeys;
        int count = rows.Count;
        Guid userId = this.UserId;
        IList<WorkflowNotify> notifyList = new List<WorkflowNotify>();

        for (int i = 0; i < count; i++)
        {
            GridViewRow item = rows[i];
            if (item.RowType == DataControlRowType.DataRow)
            {
                CheckBox checkEmail = item.FindControl("chkemail") as CheckBox;
                CheckBox checkSms = item.FindControl("chksms") as CheckBox; 
                CheckBox checkCSsms = item.FindControl("chkcssms") as CheckBox;
                CheckBox checkCSEmail = item.FindControl("chkcsemail") as CheckBox;
                string workflowName = (string)keys[item.DataItemIndex]["WorkflowName"];
                if (checkEmail == null || checkSms == null || checkCSEmail == null || checkCSsms == null || string.IsNullOrEmpty(workflowName))
                    continue;
                //if (checkEmail.Checked == true && checkSms.Checked == true)
                //    continue;

                string message = string.Empty;

                WorkflowNotify notify = new WorkflowNotify();
                notify.UserId = userId;
                notify.WorkflowName = workflowName;
                notify.NotifyType = GetNotifyType(checkEmail.Checked, checkSms.Checked);
                notify.ReviewType = GetNotifyType(checkCSEmail.Checked, checkCSsms.Checked);

                notifyList.Add(notify);
            }
        }
        if (WorkflowNotify.Insert(userId, notifyList))
            ShowSuccess("修改通知提醒设置成功！", ReturnUrl);
        else
            ShowError("修改出现错误。", ReturnUrl);
    }

    private static short GetNotifyType(bool enbaleEmail, bool enbaleSms)
    {
        if (enbaleEmail && enbaleSms)
            return 1; //都不禁用
        else if (enbaleEmail && !enbaleSms)
            return 2;
        else if (!enbaleEmail && enbaleSms)
            return 3;
        else
            return 0; // 都禁用

    }

    private static short GetReviewType(bool enbaleEmail, bool enbaleSms)
    {
        if (enbaleEmail && enbaleSms)
            return 1; //都不禁用
        else if (enbaleEmail && !enbaleSms)
            return 2;
        else if (!enbaleEmail && enbaleSms)
            return 3;
        else
            return 0; // 都禁用

    }
}
