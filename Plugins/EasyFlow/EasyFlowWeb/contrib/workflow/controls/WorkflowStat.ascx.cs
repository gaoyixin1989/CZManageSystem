using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;

public partial class contrib_workflow_controls_WorkflowStat : Botwave.Security.Web.UserControlBase//System.Web.UI.UserControl
{
    private IReportService reportService = (IReportService)Ctx.GetObject("reportService");
    public IReportService ReportService
    {
        set { reportService = value; }
    }

    public string WorkflowName
    {
        get { return (string)(ViewState["WorkflowName"]); }
        set { ViewState["WorkflowName"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void LoadData(string workflowName)
    {
        this.WorkflowName = workflowName;
        string startDT = DateTime.Now.AddYears(-100).ToString("yyyy-MM-dd HH:mm:ss");
        string endDT = DateTime.Now.AddYears(100).ToString("yyyy-MM-dd HH:mm:ss");

        IList<Report> list = reportService.GetActivityStat(workflowName, startDT, endDT);
        rptWorkflowStat.DataSource = list;
        rptWorkflowStat.DataBind();
    }
}
