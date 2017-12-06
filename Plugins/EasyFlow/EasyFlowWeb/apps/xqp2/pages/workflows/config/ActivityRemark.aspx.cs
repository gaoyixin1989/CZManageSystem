using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.XQP.Domain;
using Botwave.Workflow.Service;

public partial class apps_xqp2_pages_workflows_config_ActivityRemark : Botwave.Security.Web.PageBase
{
    /// <summary>
    /// 步骤编号.
    /// </summary>
    public Guid ActivityId
    {
        get { return (Guid)ViewState["ActivityId"]; }
        set { ViewState["ActivityId"] = value; }
    }

    /// <summary>
    /// 步骤名称.
    /// </summary>
    public string ActivityName
    {
        get { return (string)ViewState["ActivityName"]; }
        set { ViewState["ActivityName"] = value; }
    }

    /// <summary>
    /// 流程名称.
    /// </summary>
    public string WorkflowName
    {
        get { return (string)ViewState["WorkflowName"]; }
        set { ViewState["WorkflowName"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string aid=Request.QueryString["aid"];
            string wname=Request.QueryString["wname"];
            string act=Request.QueryString["act"];
            WorkflowName = wname;
            ActivityName = act;
            ActivityId = new Guid(aid);
            this.LoadWorkflowRemarks(new Guid(aid));
        }

    }

    private void LoadWorkflowRemarks(Guid activityid)
    {
        IList<ActivityRemark> remarks = ActivityRemark.SelectByActivityId(activityid);
        gridviewRemarks.DataSource = remarks;
        gridviewRemarks.DataBind();
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
        this.LoadWorkflowRemarks(this.ActivityId);
    }

    protected void gridviewRemarks_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        this.gridviewRemarks.EditIndex = -1;
        this.LoadWorkflowRemarks(this.ActivityId);
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

            if (ActivityRemark.Update(id, remarkText, remarkValue) > 0)
                this.ltlRemarkMsg.Text = "<font color=green>更新成功!</font>";
            else
                this.ltlRemarkMsg.Text = "<font color=red>更新失败!</font>";
        }
        else
        {
            this.ltlRemarkMsg.Text = "<font color=red>更新失败!</font>";
        }

        this.gridviewRemarks.EditIndex = -1;
        this.LoadWorkflowRemarks(this.ActivityId);
    }

    protected void gridviewRemarks_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataKeyArray keys = this.gridviewRemarks.DataKeys;
        int id = int.Parse(keys[e.RowIndex].Values["Id"].ToString());

        if (ActivityRemark.Delete(id) > 0)
            this.ltlRemarkMsg.Text = "<font color=green>删除成功!</font>";
        else
            this.ltlRemarkMsg.Text = "<font color=red>删除失败!</font>";
        this.LoadWorkflowRemarks(this.ActivityId);
    }

    /// <summary>
    /// 插入审批意见.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnInsertRemark_Click(object sender, EventArgs e)
    {
        ActivityRemark remark = new ActivityRemark();
        remark.DisplayOrder = 1;
        remark.RemarkText = txtRemarkText.Text;
        remark.RemarkValue = txtRemarkValue.Text;
        remark.ActivityName = ActivityName;
        remark.WorkflowName = WorkflowName;

        if (remark.IsExists())
        {
            Response.Write("<script>alert('对不起，当前审批意见已经存在.');</script>");
            return;
        }

        remark.Insert();
        this.LoadWorkflowRemarks(ActivityId);
    }
    #endregion
}
