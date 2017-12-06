using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class contrib_workflow_pages_stat_WorkflowStat : Botwave.Web.PageBase
{
    public string WorkflowName
    {
        get { return (string)(ViewState["WorkflowName"]); }
        set { ViewState["WorkflowName"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["wname"] == null)
                ShowError(Botwave.Web.MessageHelper.Message_ArgumentException);
            string workflowName = Request.QueryString["wname"];

            this.WorkflowName = workflowName;
            this.workflowStat1.LoadData(workflowName);
        }
    }
}
