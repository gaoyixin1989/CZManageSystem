using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class apps_czmcc_pages_WorkflowHistoryRelation : System.Web.UI.UserControl
{
    public bool Editable
    {
        get { return ViewState["Editable"] == null ? false : (bool)ViewState["Editable"]; }
        set { ViewState["Editable"] = value; }
    }

    public bool Starting
    {
        get { return ViewState["Starting"] == null ? false : (bool)ViewState["Starting"]; }
        set { ViewState["Starting"] = value; }
    }

    public string Actor
    {
        get { return (string)ViewState["Actor"]; }
        set { ViewState["Actor"] = value; }
    }

    public Guid RelationID
    {
        get { return string.IsNullOrEmpty(this.hiddenRelationID.Value) ? Guid.Empty : Botwave.Commons.DbUtils.ToGuid(this.hiddenRelationID.Value).Value; }
        set { this.hiddenRelationID.Value = value.ToString(); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void Bind(Guid? workflowInstanceId)
    {
        Bind(workflowInstanceId, Guid.NewGuid());
    }

    public void Bind(Guid? workflowInstanceId, Guid relationID)
    {
        this.RelationID = workflowInstanceId.HasValue ? workflowInstanceId.Value :  relationID;
        this.Starting = !workflowInstanceId.HasValue;
    }

    public void UpdateWorkflowInstanceId(Guid workflowInstanceId, string creator)
    {
        if (this.RelationID == workflowInstanceId)
            return;
        Botwave.XQP.Domain.CZWorkflowRelation.UpdateWorkflowInstanceId(this.RelationID, creator, workflowInstanceId);
    }
}
