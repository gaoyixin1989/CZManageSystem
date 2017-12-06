using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Botwave.Commons;
using Botwave.Web.Controls;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Util;

public partial class contrib_workflow_controls_ReviewList : Botwave.Security.Web.UserControlBase
{
    public string UserName
    {
        get { return (string)ViewState["UserName"]; }
        set { ViewState["UserName"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string userName = CurrentUserName;
            this.UserName = userName;

            this.Search(0, 0, false);
        }
    }

    protected void listPagerToReview_PageIndexChanged(object sender, PageChangedEventArgs e)
    {
        this.Search(listPagerToReview.TotalRecordCount, e.NewPageIndex, false);
    }

    protected void Search(int recordCount, int pageIndex, bool autoHidden)
    {
        string userName = this.UserName;
        string workflowName = Request.QueryString["wfname"];
        workflowName = string.IsNullOrEmpty(workflowName) ? Request.QueryString["name"] : workflowName;
        string keywords = Request.QueryString["key"];

        DataTable resultTable = Botwave.XQP.Domain.ToReview.GetReviewTable(userName, workflowName, keywords, pageIndex, listPagerToReview.ItemsPerPage, ref recordCount);
        if (autoHidden && (resultTable == null || resultTable.Rows.Count == 0))
            this.Visible = false;
        rptList.DataSource = resultTable;
        rptList.DataBind();

        this.listPagerToReview.TotalRecordCount = recordCount;
        this.listPagerToReview.DataBind();
    }

    protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = e.Item.DataItem as DataRowView;
        //string toReviewActors = DbUtils.ToString(row["ToReviewActors"]);

        string aliasImage = DbUtils.ToString(row["AliasImage"]); // 别名图片
        if (!string.IsNullOrEmpty(aliasImage))
        {
            string workflowAlias = DbUtils.ToString(row["WorkflowAlias"]);
            Literal ltlWorkflowAlias = e.Item.FindControl("ltlWorkflowAlias") as Literal;
            ltlWorkflowAlias.Text = string.Format("<img alt=\"{0}\" class=\"groupImage\" src=\"{2}contrib/workflow/res/groups/{1}\" />", workflowAlias, aliasImage, AppPath);
        }
        //Literal ltlToReviewActors = e.Item.FindControl("ltlToReviewActors") as Literal;
        //if (!string.IsNullOrEmpty(toReviewActors))
        //{
        //    ltlToReviewActors.Text = WorkflowUtility.FormatWorkflowActor(toReviewActors, this.UserName);
        //}
    }

    protected void btnReview_Click(object sender, EventArgs e)
    {
        string actor = this.UserName;
        string ids=Request.Form["chk"];
        if (ids.Length > 0)
        {
            foreach (string id in ids.Split(','))
            {
                Guid activityInstanceId = new Guid(id);
                Botwave.XQP.Domain.ToReview.UpdateReview(activityInstanceId, actor);
                Botwave.XQP.Domain.ToReview.DeletePendingMsg(activityInstanceId, actor);
            }
        }
        ShowSuccess("操作成功.", this.Request.RawUrl);
    }
}
