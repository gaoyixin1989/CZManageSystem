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

public partial class contrib_mobile_ajax_myTaskAjax : Botwave.XQP.Web.Security.PageBase
{
    public DataTable source = new DataTable();
    public int PageIndex = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Search(0, 0);
        }
    }


    protected void Search(int recordCount, int pageIndex)
    {
        string keywords = Context.Request["key"], workflowName = Context.Request["wfname"], sheetID = Context.Request["sheetid"];
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
        string creator = CurrentUser.UserName;
        Botwave.Workflow.Extension.Service.AdvancedSearchCondition condition = new Botwave.Workflow.Extension.Service.AdvancedSearchCondition();
        condition.BeginTime = startdt;
        condition.EndTime = enddt;
        condition.CreatorName = creator;
        condition.ProcessorName = creator;
        condition.WorkflowName = workflowName;
        condition.Title = keywords;
        condition.SheetId = sheetID;
        //this.rptList.DataSource = workflowSearcher.Search(condition, pageIndex, listPager.ItemsPerPage, ref recordCount);

        source = CZWorkflowSearcher.GetMpDoingTask(condition, pageIndex, 10, ref recordCount);
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

    public string GetCurrentActors(string workflowInstanceId, int state)
    {
        if (state == 2)
            return string.Empty;
        if (state == 99)
            return string.Empty;
        else
        {
            SqlParameter[] pa = new SqlParameter[1];
            pa[0] = new SqlParameter("@WorkflowInstanceId", SqlDbType.UniqueIdentifier);
            pa[0].Value = new Guid(workflowInstanceId);
            object result = IBatisDbHelper.ExecuteScalar(CommandType.Text, "select dbo.fn_bwwf_GetCurrentActors(@WorkflowInstanceId)", pa);
            if (result != null)
            {
                string str = Botwave.Commons.DbUtils.ToString(result);
                str = Botwave.Workflow.Extension.Util.WorkflowUtility.FormatWorkflowActor(str);
                return str;
            }
            return string.Empty;
        }
    }
}