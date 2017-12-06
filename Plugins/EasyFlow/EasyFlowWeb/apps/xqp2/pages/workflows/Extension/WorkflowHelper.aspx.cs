using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Botwave.Extension;
using Botwave.XQP.Web;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Security.Domain;
using Botwave.Security.Service;
using Botwave.Extension.IBatisNet;
using Botwave.Commons;

public partial class Workflow_Extension_WorkflowHelper : Botwave.Security.Web.PageBase
{
    
    #region Service Interface 
    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");
    private IActivityDefinitionService activityDefinitionService = (IActivityDefinitionService)Ctx.GetObject("activityDefinitionService");
    private IRoleService roleService = (IRoleService)Ctx.GetObject("roleService");
    private IWorkflowEngine workflowEngine = (IWorkflowEngine)Ctx.GetObject("workflowEngine");
    private IWorkflowService workflowService = (IWorkflowService)Ctx.GetObject("workflowService");
    private IActivityService activityService = (IActivityService)Ctx.GetObject("activityService");

    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        set { workflowDefinitionService = value; }
    }

    public IActivityDefinitionService ActivityDefinitionService
    {
        set { activityDefinitionService = value; }
    }

    public IRoleService RoleService
    {
        set { roleService = value; }
    }

    public IWorkflowEngine WorkflowEngine
    {
        set { workflowEngine = value; }
    }

    public IWorkflowService WorkflowService
    {
        set { workflowService = value; }
    }

    public IActivityService ActivityService
    {
        set { activityService = value; }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            Botwave.Security.Web.LogWriterFactory.Writer = new Botwave.XQP.Service.Support.DefaultLogWriter();
            //LoadWorkflowData();
    }

    #region 任务改派
    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        string sheetId = txtSheetId.Text.Trim();
        string toUser = txtToUser.Value;
        string selectedActivityId = ddlTransferActivityList.SelectedValue;
        Guid toActivityId = Guid.Empty;
        if (!string.IsNullOrEmpty(selectedActivityId))
            toActivityId = new Guid(selectedActivityId);

        SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@SheetId", SqlDbType.VarChar),
                new SqlParameter("@ToUser", SqlDbType.VarChar),
                new SqlParameter("@ToActivityId", SqlDbType.UniqueIdentifier)
            };
        parameters[0].Value = sheetId;
        parameters[1].Value = toUser;
        parameters[2].Value = toActivityId;

        object returnValue = IBatisDbHelper.ExecuteScalar(CommandType.StoredProcedure, "BWWF_EXT_TransferInstance",parameters);
        if (DbUtils.ToString(returnValue) == "success")
        {
            WriteNomalLog(CurrentUserName, "任务改派", "受理号：" + sheetId + "；改派步骤：" + ddlTransferActivityList.SelectedItem.Text + "；改派用户：" + toUser + " 成功");
            ShowSuccess("任务改派成功!");
        }
        else
        {
            WriteExceptionLog("SQL Server异常", DbUtils.ToString(returnValue));
            ShowError("任务改派失败,请返回重试!");
        }
    }

    //获取工单信息/改派步骤.
    protected void btnGetSheetInfo_Click(object sender, EventArgs e)
    {
        string sheetId = txtSheetId.Text.Trim();
        if (string.IsNullOrEmpty(sheetId)) return;

        string info = GetSheetInfoBySheetId(sheetId);
        if (string.IsNullOrEmpty(info))
            info = "对不起,该工单号不存在,请输入正确的工单号!";
        else
            LoadTransferActivitiesBySheetId(sheetId);

        lblSheetInfo.Text = info;
    }
    #endregion 

    #region Methods
    /// <summary>
    /// 根据工单号获取工单当前信息.
    /// </summary>
    /// <param name="sheetId"></param>
    /// <returns></returns>
    private string GetSheetInfoBySheetId(string sheetId)
    {
        StringBuilder sb = new StringBuilder();
        string where = string.Empty;
        //string sql = string.Format("SELECT ActivityName, ActorName FROM vw_bwwf_Tracking_Todo WHERE SheetId='{0}'", sheetId);
        bool isAdmin = Botwave.XQP.Util.CZWorkflowUtility.HasAdvanceSearch(CurrentUser, "A007");
        if (!isAdmin)//流程管理员
        {
            IList<WorkflowDefinition> workflows = workflowDefinitionService.GetWorkflowDefinitionList();
            IList<WorkflowDefinition> allowWorkflows = new List<WorkflowDefinition>();
            allowWorkflows = Botwave.XQP.Util.CZWorkflowUtility.GetAllowedWorkflows(workflows, CurrentUser.Resources, new string[] { "0005" });
            foreach (WorkflowDefinition item in allowWorkflows)
            {
                sb.Append(item.WorkflowName + ",");
            }
            if (sb.Length > 0)
            {
                sb = sb.Remove(sb.Length - 1, 1);
                where = " and exists(select workflowid from bwwf_workflows bw where tw.workflowid=bw.workflowid and workflowname in ('"+sb.ToString().Replace(",","','")+"'))";
            }
        }
        string sql = string.Format(@"select CASE tw.STATE
WHEN 2 THEN '完成'
WHEN 99 THEN '取消'
ELSE dbo.fn_bwwf_GetCurrentActivityNames(tw.WorkflowInstanceId)
END ActivityName,
(CASE tw.STATE
WHEN 2 THEN ''
WHEN 99 THEN ''
ELSE dbo.fn_bwwf_GetCurrentActors(tw.WorkflowInstanceId)
END) CurrentActors from bwwf_tracking_workflows tw where tw.sheetid = '{0}' {1}", sheetId,where);
        DataTable dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        if (0 != dt.Rows.Count)
        {
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    sb.Append(dt.Rows[i][1].ToString() + ",");
            //}
            //return "当前步骤:" + dt.Rows[0][0].ToString() + ",当前处理人:" + sb.ToString();
            return "当前步骤:" + dt.Rows[0][0].ToString() + ",当前处理人:" + dt.Rows[0][1].ToString();

        }
        return null;
    }

    /// <summary>
    /// 载入改派步骤列表.
    /// </summary>
    /// <param name="sheetId"></param>
    private void LoadTransferActivitiesBySheetId(string sheetId)
    {
        string sql = string.Format(@"SELECT ActivityId,ActivityName FROM bwwf_Activities  a 
                                    LEFT JOIN bwwf_Tracking_Workflows  tw ON a.WorkflowId = tw.WorkflowId
                                    WHERE tw.SheetId='{0}' AND a.State <> 2 
                                    ORDER BY a.SortOrder", sheetId);
        DataSet ds = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql);

        ddlTransferActivityList.DataSource = ds;
        ddlTransferActivityList.DataTextField = "ActivityName";
        ddlTransferActivityList.DataValueField = "ActivityId";
        ddlTransferActivityList.DataBind();

        ddlTransferActivityList.Items.Insert(0, new ListItem("-请选择改派步骤-", ""));
    }

    /// <summary>
    /// 呈现被转移用户拥有权限列表.
    /// </summary>
    /// <param name="roles"></param>
    /// <returns></returns>
    private static string RenderTargetUserRole(IList<RoleInfo> roles)
    {
        if (roles == null || roles.Count == 0)
            return string.Empty;

        int repeateColumns = 4;
        int count = roles.Count;

        StringBuilder builder = new StringBuilder();
        builder.Append("<table style=\"width:100%\" class=\"tblGrayClass\" cellpadding=\"4\" cellspacing=\"1\">");

        for (int index = 0; index < count; index++)
        {
            int columnIndex = index % repeateColumns;
            if (columnIndex == 0)
                builder.AppendLine("<tr>");
            RoleInfo item = roles[index];
            string text = item.RoleName;
            builder.AppendFormat("<td>{0}</td>", text);

            if (columnIndex == repeateColumns - 1)
                builder.AppendLine("</tr>");
        }
        int modValue = count % repeateColumns;
        if (modValue > 0)
        {
            for (; modValue < repeateColumns; modValue++)
            {
                builder.Append("<td></td>");
            }
            builder.AppendLine("</tr>");
        }

        builder.Append("</table>");
        return builder.ToString();
    }

    #endregion

    protected void btn_CloseFlow_Click(object sender, EventArgs e)
    {
        string completedActId = string.Empty;
        string sheetid = string.Empty;
        string workflowInstanceId = Botwave.Commons.DbUtils.ToString(IBatisDbHelper.ExecuteScalar(CommandType.Text, string.Format("select workflowinstanceid from bwwf_tracking_workflows where sheetid = '{0}' and state = 1",txtSheetId.Text)));
        if (string.IsNullOrEmpty(workflowInstanceId))
        {
            Response.Write("<script>alert('该工单号不存在或者已经结束！');</script>");
            return;
        }
        WorkflowInstance workflowInstance = workflowService.GetWorkflowInstance(new Guid(workflowInstanceId));

        if (workflowInstance == null)
        {
            Response.Write("<script>alert('该工单号不存在或者已经结束！');</script>");
            return;
        }
        IList<ActivityDefinition> activityDefinitions = activityDefinitionService.GetActivitiesByWorkflowId(workflowInstance.WorkflowId);
        foreach (ActivityDefinition activityDefinition in activityDefinitions)
        {
            if (activityDefinition.State == 2)
            {
                completedActId = activityDefinition.ActivityId.ToString();
                break;
            }
        }
        IList<ActivityDefinition> prevs = activityDefinitionService.GetPrevActivityDefinitions(new Guid(completedActId));//获取完成步骤的上一个步骤
        foreach (ActivityDefinition prev in prevs)//先将工单改派到完成步骤的上一步
        {
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@SheetId", SqlDbType.VarChar),
                new SqlParameter("@ToUser", SqlDbType.VarChar),
                new SqlParameter("@ToActivityId", SqlDbType.UniqueIdentifier)
            };
            parameters[0].Value = workflowInstance.SheetId;
            parameters[1].Value = CurrentUserName;
            parameters[2].Value = prev.ActivityId;

            object returnValue = IBatisDbHelper.ExecuteScalar(CommandType.StoredProcedure, "BWWF_EXT_TransferInstance", parameters);
            if (DbUtils.ToString(returnValue) == "success")
            {
                ActivityInstance activityInstance = activityService.GetCurrentActivity(workflowInstance.WorkflowInstanceId);
                ActivityExecutionContext context = new ActivityExecutionContext();
                context.ActivityInstanceId = activityInstance.ActivityInstanceId;
                context.Actor = CurrentUserName;
                context.Command = ActivityCommands.Approve;
                context.Reason = "管理员结束工单";

                context.Variables["Secrecy"] = workflowInstance.Secrecy;
                context.Variables["Urgency"] = workflowInstance.Urgency;
                context.Variables["Importance"] = workflowInstance.Importance;

                string entityId = workflowInstance.ExternalEntityId;
                if (!String.IsNullOrEmpty(entityId))
                    context.ExternalEntityId = entityId;
                context.Variables["CurrentUser"] = CurrentUser; //添加当前用户为流程变量
                IDictionary<string, string> dict = new Dictionary<string, string>();
                context.ActivityAllocatees = new Dictionary<Guid, IDictionary<string, string>>();
                dict.Add(CurrentUserName, null);
                context.ActivityAllocatees.Add(new Guid(completedActId), dict);
                workflowEngine.ExecuteActivity(context);
                WriteNomalLog(CurrentUserName, "任务改派", "受理号：" + workflowInstance.SheetId + "结束工单 成功");
                ShowSuccess("成功结束工单！");
            }
            else
            {
                WriteExceptionLog("SQL Server异常", DbUtils.ToString(returnValue));
                ShowError("结束失败,请返回重试!");
            }
        }
    }

    protected void btn_CannalFlow_Click(object sender, EventArgs e)
    {
        string completedActId = string.Empty;
        string sheetid = string.Empty;
        string workflowInstanceId = Botwave.Commons.DbUtils.ToString(IBatisDbHelper.ExecuteScalar(CommandType.Text, string.Format("select workflowinstanceid from bwwf_tracking_workflows where sheetid = '{0}' and state = 1", txtSheetId.Text)));
        if (string.IsNullOrEmpty(workflowInstanceId))
        {
            Response.Write("<script>alert('该工单号不存在或者已经结束！');</script>");
            return;
        }
        WorkflowInstance workflowInstance = workflowService.GetWorkflowInstance(new Guid(workflowInstanceId));

        if (workflowInstance == null)
        {
            Response.Write("<script>alert('该工单号不存在或者已经结束！');</script>");
            return;
        }

        ActivityInstance activityInstance = activityService.GetCurrentActivity(workflowInstance.WorkflowInstanceId);
        ActivityExecutionContext context = new ActivityExecutionContext();
        context.ActivityInstanceId = activityInstance.ActivityInstanceId;
        context.Actor = CurrentUserName;
        context.Command = ActivityCommands.Cancel;
        context.Reason = "管理员作废工单";

        context.Variables["Secrecy"] = workflowInstance.Secrecy;
        context.Variables["Urgency"] = workflowInstance.Urgency;
        context.Variables["Importance"] = workflowInstance.Importance;

        string entityId = workflowInstance.ExternalEntityId;
        if (!String.IsNullOrEmpty(entityId))
            context.ExternalEntityId = entityId;
        workflowEngine.ExecuteActivity(context);
        WriteNomalLog(CurrentUserName, "任务改派", "受理号：" + workflowInstance.SheetId + "作废工单 成功");
        ShowSuccess("成功作废工单！");
    }
}
