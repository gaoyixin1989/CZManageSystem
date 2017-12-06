using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.Commons;
using Botwave.XQP.Domain;

public partial class apps_czmcc_pages_WorkflowAttentionControl : Botwave.Web.UserControlBase
{
    public Guid WorkflowInstanceId
    {
        get { return ViewState["WorkflowInstanceId"] == null ? Guid.Empty : (Guid)ViewState["WorkflowInstanceId"]; }
        set { ViewState["WorkflowInstanceId"] = value; }
    }

    public int AttentionType
    {
        get { return ViewState["AttentionType"] == null ? 0 : (int)ViewState["AttentionType"]; }
        set { ViewState["AttentionType"] = value; }
    }

    public string Actor
    {
        get { return ViewState["Actor"] == null ? string.Empty : (string)ViewState["Actor"]; }
        set { ViewState["Actor"] = value; }
    }

    public bool IsExists
    {
        get { return ViewState["IsExists"] == null ? false : (bool)ViewState["IsExists"]; }
        set { ViewState["IsExists"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    public void Bind(Guid workflowInstanceId, int type, string creator)
    {
        this.WorkflowInstanceId = workflowInstanceId;
        this.AttentionType = type;
        this.Actor = creator;

        this.IsExists = CZWorkflowAttention.Exists(workflowInstanceId, creator) == 1;
    }
}
