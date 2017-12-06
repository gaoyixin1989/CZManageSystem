using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class apps_xqp2_pages_workflows_report_CommentReport : Botwave.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        workflowCommentStat1.ExportExcel();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }
}
