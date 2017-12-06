using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.Commons;
using Botwave.Web;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Service;
using Botwave.XQP.Service;
using System.Data;
using System.Data.SqlClient;
using Botwave.Extension.IBatisNet;

public partial class contrib_mobile_ajax_doneTaskAjax : Botwave.XQP.Web.Security.PageBase
{
    public DataTable source = new DataTable();
    public int PageIndex = 0;

    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");
    private IWorkflowMobileService workflowMobileService = (IWorkflowMobileService)Ctx.GetObject("workflowMobileService");

    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        set { workflowDefinitionService = value; }
    }

    public IWorkflowMobileService WorkflowMobileService
    {
        set { this.workflowMobileService = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Search(0, 0);
        }
    }


    protected void Search(int recordCount, int pageIndex)
    {
        string keywords = string.IsNullOrEmpty(Context.Request["key"]) ? "" : Context.Request["key"]
            , workflowName = string.IsNullOrEmpty(Context.Request["wfname"]) ? "" : Context.Request["wfname"]
            , creater = string.IsNullOrEmpty(Context.Request["creator"]) ? "" : Context.Request["creator"]
            , sheetID = string.IsNullOrEmpty(Context.Request["sheetid"]) ? "" : Context.Request["sheetid"];
        DateTime now = DateTime.Now;
        string startdt = now.AddYears(-100).ToString("yyyy-MM-dd"); ;
        string enddt = now.AddYears(100).ToString("yyyy-MM-dd");
        pageIndex = PageIndex = DbUtils.ToInt32(this.Context.Request["pageIndex"]);
        if (!string.IsNullOrEmpty(Context.Request["st"])) // 最小流程处理时间.
        {
            startdt = Context.Request["st"];
        }
        if (!string.IsNullOrEmpty(Context.Request["et"])) // 最大流程处理时间
        {
            enddt = Context.Request["et"];
        }
        string actor = CurrentUser.UserName;

        source = workflowMobileService.GetDoneTaskPager(workflowName, actor, keywords, sheetID, creater, startdt, enddt, false, null, pageIndex, 10, ref recordCount);
    }

    public string GetCurrentActNames(string workflowInstanceId, int state)
    {
        if (state == 2)
            return "完成";
        if (state == 99)
            return "取消";
        else
        {
            SqlParameter[] pa = new SqlParameter[1];
            pa[0] = new SqlParameter("@WorkflowInstanceId", SqlDbType.UniqueIdentifier);
            pa[0].Value = new Guid(workflowInstanceId);
            object result = IBatisDbHelper.ExecuteScalar(CommandType.Text, "select dbo.fn_bwwf_GetCurrentActivityNames(@WorkflowInstanceId)", pa);
            return Botwave.Commons.DbUtils.ToString(result);
        }
    }
}