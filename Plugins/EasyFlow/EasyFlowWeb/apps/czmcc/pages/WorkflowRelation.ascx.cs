using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.XQP.Domain;
using Botwave.XQP.Commons;
using System.Data;

public partial class apps_czmcc_pages_WorkflowRelation : System.Web.UI.UserControl
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

    public Guid WorkflowId
    {
        get { return ViewState["WorkflowId"]==null?Guid.Empty:(Guid)ViewState["WorkflowId"]; }
        set { ViewState["WorkflowId"] = value; }
    }

    public string RelationWorkflowName
    {
        get { return (string)ViewState["RelationWorkflowName"]; }
        set { ViewState["RelationWorkflowName"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void Bind(Guid? workflowInstanceId)
    {
        Bind(workflowInstanceId, Guid.NewGuid());
    }

    public void BindProcess(Guid? workflowInstanceId, Guid activityId)
    {
        Bind(workflowInstanceId,activityId, Guid.NewGuid());
    }

    private void Bind(Guid? workflowInstanceId, Guid relationID)
    {
        this.RelationID = workflowInstanceId.HasValue ? workflowInstanceId.Value :  relationID;
        this.Starting = !workflowInstanceId.HasValue;
    }

    private void Bind(Guid? workflowInstanceId, Guid? activityId, Guid relationID)
    {
        this.RelationID = workflowInstanceId.HasValue ? workflowInstanceId.Value : relationID;
        this.Starting = !workflowInstanceId.HasValue;
        CZWorkflowRelationSetting setting = CZWorkflowRelationSetting.GetRelationSetting(activityId.Value);
        if (setting != null)
        {
            RelationWorkflowName = setting.RelationWorkflowName;
            DataTable dt = APIServiceSQLHelper.QueryForDataSet("API_Select_Workflow_Start_WorkflowId", RelationWorkflowName);
            if (dt != null && dt.Rows.Count > 0)
            {
                WorkflowId = new Guid(dt.Rows[0][0].ToString());//流程定义ID
            }
            if (setting.SettingType > 0)
            {
                int relationCnt = CZWorkflowRelationSetting.GetRelationsCnt(workflowInstanceId.Value);
                if ((setting.SettingType == 1 && relationCnt < 1) || (setting.SettingType == 2))
                    Editable = true;
            }
        }
    }

    public void UpdateWorkflowInstanceId(Guid workflowInstanceId, string creator)
    {
        if (this.RelationID == workflowInstanceId)
            return;
        Botwave.XQP.Domain.CZWorkflowRelationSetting.SetWorkflowInstanceRelation(this.RelationID, creator, workflowInstanceId);
    }
}
