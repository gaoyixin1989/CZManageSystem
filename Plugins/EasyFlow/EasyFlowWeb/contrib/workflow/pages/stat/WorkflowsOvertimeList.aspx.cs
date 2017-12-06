using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Botwave.Workflow.Service;

public partial class contrib_workflow_pages_stat_WorkflowsOvertimeList : Botwave.Security.Web.PageBase
{
    private IReportService reportService = (IReportService)Ctx.GetObject("reportService");

    public IReportService ReportService
    {
        get { return reportService; }
        set { reportService = value; }
    }

    public string WorkflowName
    {
        get { return (string)ViewState["WorkflowName"]; }
        set { ViewState["WorkflowName"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string w = Request.QueryString["w"];
            if (!string.IsNullOrEmpty(w))
            {
                WorkflowName = w.ToString();
                ltlTitle.Text = WorkflowName + "-";
            }

            InitializeData(0, 0);
        }
    }

    private void InitializeData(int recordCount, int pageIndex)
    {
        DataTable dtResults = reportService.GetWorkflowsOvertimeList(WorkflowName, pageIndex, listPager.ItemsPerPage, ref recordCount);
        if (null != dtResults && dtResults.Rows.Count > 0)
        {
            listResults.DataSource = dtResults;
            listResults.DataBind();
            listPager.TotalRecordCount = recordCount;
            listPager.DataBind();
        }
    }

    protected void listPager_PageIndexChanged(object sender, Botwave.Web.Controls.PageChangedEventArgs e)
    {
        this.InitializeData(listPager.TotalRecordCount, e.NewPageIndex);
    }
}
