using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Botwave.Security.Domain;
using Botwave.Security.Service;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.XQP.Service;
using Botwave.Web;
using Botwave.Security.Web;

public partial class contrib_workflow_controls_ProcessHistoryEditor : Botwave.XQP.Web.Controls.WorkflowProcessHistoryEditor
{
    

    protected void Page_Load(object sender, EventArgs e)
    { 
    }

    protected override void DataBind(string historyHtml)
    {
        this.ltlHistoryLogs.Text = historyHtml;
    }

   
}
