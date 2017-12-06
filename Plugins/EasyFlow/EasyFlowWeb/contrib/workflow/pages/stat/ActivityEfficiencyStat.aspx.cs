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

public partial class contrib_workflow_pages_stat_ActivityEfficiencyStat : Botwave.Security.Web.PageBase
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

    protected override void OnInit(EventArgs e)
    {
        string w = Request.QueryString["w"];
        if (String.IsNullOrEmpty(w))
        {
            ShowError(Botwave.GlobalSettings.Instance.ArgumentExceptionMessage);
        }
        WorkflowName = w;

        ltlTitle.Text = WorkflowName + "-";
        base.OnInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            BindData();
    }
    private void BindData()
    {
        IList<Report> list = reportService.GetActivityEfficiency(WorkflowName);
        rptStat.DataSource = list;
        rptStat.DataBind();
    }
}
