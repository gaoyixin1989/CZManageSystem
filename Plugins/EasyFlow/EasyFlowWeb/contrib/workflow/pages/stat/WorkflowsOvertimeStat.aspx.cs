using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Botwave.Workflow.Service;
using Botwave.Workflow.Domain;

public partial class contrib_workflow_pages_stat_WorkflowsOvertimeStat : Botwave.Security.Web.PageBase
{
    private IReportService reportService = (IReportService)Ctx.GetObject("reportService");

    public IReportService ReportService
    {
        get { return reportService; }
        set { reportService = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            InitializeData();
    }

    private void InitializeData()
    {
        IList<Report> resultList = reportService.GetWorkflowsOvertimeStat();
        if (null != resultList && resultList.Count > 0)
        {
            rptStat.DataSource = resultList;
            rptStat.DataBind();
        }
    }
}
