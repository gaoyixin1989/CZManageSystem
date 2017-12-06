using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class contrib_dynamicform_pages_config_UC_Menu : Botwave.Security.Web.UserControlBase
{
    public string WorkflowId
    {
        get { return (string)ViewState["WorkflowId"]; }
        set { ViewState["WorkflowId"] = value; }
    }

    public string FormDefinitionId
    {
        get { return (string)ViewState["FormDefinitionId"]; }
        set { ViewState["FormDefinitionId"] = value; }
    }

    public string FormItemDefinitionId
    {
        get { return (string)ViewState["FormItemDefinitionId"]; }
        set { ViewState["FormItemDefinitionId"] = value; }
    }

    public string EntityType
    {
        get { return (string)ViewState["EntityType"]; }
        set { ViewState["EntityType"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!IsPostBack)
        {
            string workflowId =WorkflowId= Request.QueryString["wfid"];
            string formDefinitionId =FormDefinitionId= Request.QueryString["fdId"];
            string formItemDefinitionId =FormItemDefinitionId= Request.QueryString["fid"];
            string entityType =EntityType= Request.QueryString["EntityType"];
        }
    }
}
