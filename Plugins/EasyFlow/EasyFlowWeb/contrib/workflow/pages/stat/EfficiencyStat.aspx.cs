using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;

public partial class contrib_workflow_pages_stat_EfficiencyStat : Botwave.Security.Web.PageBase
{
    private IReportService reportService = (IReportService)Ctx.GetObject("reportService");
    public IReportService ReportService
    {
        set { reportService = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            BindData();
    }
    private void BindData()
    {
        IList<Report> list = reportService.GetWorkflowEfficiency();
        rptStat.DataSource = list;
        rptStat.DataBind();
    }
}
