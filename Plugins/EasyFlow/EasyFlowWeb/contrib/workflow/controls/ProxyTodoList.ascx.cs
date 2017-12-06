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

public partial class contrib_workflow_controls_ProxyTodoList : Botwave.Security.Web.UserControlBase
{
    #region service properties

    private IActivityService activityService = (IActivityService)Ctx.GetObject("activityService");

    public IActivityService ActivityService
    {
        get { return activityService; }
        set { activityService = value; }
    }
    #endregion

    #region properties

    /// <summary>
    /// 当前用户名.
    /// </summary>
    public string UserName
    {
        get { return (string)ViewState["UserName"]; }
        set { ViewState["UserName"] = value; }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string userName = CurrentUserName;
            this.BindData(userName);
        }
    }


    /// <summary>
    /// 绑定数据.
    /// </summary>
    /// <param name="userName"></param>
    public void BindData(string userName)
    {
        this.UserName = userName;
        this.Search(0, 0);
    }

    protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        string currentUserName = this.UserName;

        //lbTitle
        Label lbTitle = (Label)e.Item.FindControl("lbTitle");
        Literal ltlActivityName = e.Item.FindControl("ltlActivityName") as Literal;
        Literal ltlActivityIcons = e.Item.FindControl("ltlActivityIcons") as Literal;

        DataRowView row = e.Item.DataItem as DataRowView;

        string title = lbTitle.Text;
        int importance = DbUtils.ToInt32(row["Importance"]);
        int urgency = DbUtils.ToInt32(row["Urgency"]);
        int operateType = DbUtils.ToInt32(row["OperateType"]);  // 等于 1 即为退还

        bool isReaded = !TodoInfo.IsUnReaded(DbUtils.ToInt32(row["State"]));     // 是否已读.
        string aliasImageUrl = DbUtils.ToString(row["AliasImage"]); // 别名

        if (!string.IsNullOrEmpty(aliasImageUrl))
        {
            string workflowAlias = DbUtils.ToString(row["WorkflowAlias"]);
            Literal ltlWorkflowAlias = e.Item.FindControl("ltlWorkflowAlias") as Literal;
            ltlWorkflowAlias.Text = string.Format("<img alt=\"{0}\" class=\"groupImage\" src=\"{1}contrib/workflow/res/groups/{2}\" />", workflowAlias, AppPath, aliasImageUrl);
        }

        // 退回

        if (operateType == 1)
            ltlActivityIcons.Text = ltlActivityIcons.Text + "<font color=\"red\">[退回]</font>";

        if (importance > 0)
            ltlActivityIcons.Text = ltlActivityIcons.Text + "<font color=\"red\">[重要]</font>";

        if (urgency > 0)
            ltlActivityIcons.Text = ltlActivityIcons.Text + "[紧急]";

        lbTitle.CssClass = (isReaded ? "readed" : "unread");
    }

    protected void listTodoPager_PageIndexChanged(object sender, PageChangedEventArgs e)
    {
        this.Search(listPagerTodoTask.TotalRecordCount, e.NewPageIndex);
    }

    /// <summary>
    /// 查询数据.
    /// </summary>
    /// <param name="recordCount"></param>
    /// <param name="pageIndex"></param>
    protected void Search(int recordCount, int pageIndex)
    {
        DataTable source = activityService.GetTaskListByProxy(this.UserName, pageIndex, listPagerTodoTask.ItemsPerPage, ref recordCount);
        rptList.DataSource = source;
        rptList.DataBind();

        this.listPagerTodoTask.TotalRecordCount = recordCount;
        this.listPagerTodoTask.DataBind();
    }

    /// <summary>
    /// 生成处理页面的 URL 链接.
    /// </summary>
    /// <param name="activityInstanceId"></param>
    /// <returns></returns>
    public static string BuildProcessUrl(object activityInstanceId)
    {
        return string.Format("{0}contrib/workflow/pages/process.aspx?aiid={1}", AppPath, activityInstanceId);
    }
}
