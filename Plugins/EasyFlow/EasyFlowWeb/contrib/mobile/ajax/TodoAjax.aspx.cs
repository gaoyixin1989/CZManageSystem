using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Util;
using Botwave.XQP.Web.Controls;
using System.Data;
using Botwave.Commons;
using Botwave.XQP.Service;
public partial class contrib_mobile_ajax_TodoAjax : Botwave.XQP.Web.Security.PageBase
{
    public DataTable source = new DataTable();
    public int PageIndex = 0;
    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");
    private IWorkflowMobileService workflowMobileService = (IWorkflowMobileService)Ctx.GetObject("workflowMobileService");

    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        set { workflowDefinitionService = value; }
    }

    public IWorkflowMobileService WorkflowMobileService
    {
        set { workflowMobileService = value; }
    }

    private Guid? _workflowId = null;
    private string _workflowName = null;
    private bool _enableSearch = true;

    public Guid? WorkflowId
    {
        get
        {
            if (ViewState["WorkflowId"] != null)
                _workflowId = (Guid)ViewState["WorkflowId"];
            return _workflowId;
        }
        set { ViewState["WorkflowId"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Search(0, 0);
        }
    }

    protected void Search(int recordCount, int pageIndex)
    {
        string workflowName = this.Context.Request["wfname"];
        string keywords = this.Context.Request["key"];
        pageIndex = PageIndex = DbUtils.ToInt32(this.Context.Request["pageIndex"]);
        source = workflowMobileService.GetTaskListByUserName(CurrentUser.UserName, workflowName, keywords, pageIndex, 10, ref recordCount);
    }
}