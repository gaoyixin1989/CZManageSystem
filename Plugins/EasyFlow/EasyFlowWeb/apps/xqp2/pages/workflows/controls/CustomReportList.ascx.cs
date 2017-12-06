using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Report;
using Botwave.Report.Common;

public partial class apps_xqp2_pages_workflows_controls_CustomReportList : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();
        }
    }

    private void BindData()
    {
        IList<ReportEntity> reports = ReportDAL.GetReportList();
        this.rptReports.DataSource = reports;
        this.rptReports.DataBind();
    }
}
