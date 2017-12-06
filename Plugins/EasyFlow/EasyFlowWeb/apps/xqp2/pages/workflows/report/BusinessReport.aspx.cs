using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class apps_xqp2_pages_workflows_report_BusinessReport : Botwave.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }

    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(apps_xqp2_pages_workflows_controls_WorkflowBusinessStat));

    protected void btnExport_Click(object sender, EventArgs e)
    {
        log.Info(DateTime.Now + "--->Export to Excel");
        workflowBusinessStat1.ExportExcel();
    }

    protected void btnExportAll_Click(object sender, EventArgs e)
    {
        log.Info(DateTime.Now + "--->Export All Excel");
        workflowBusinessStat1.ExportExcelAll();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }
}
