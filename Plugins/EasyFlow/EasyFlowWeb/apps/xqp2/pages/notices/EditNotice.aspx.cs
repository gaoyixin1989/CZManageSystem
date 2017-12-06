using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.XQP.Commons;

public partial class xqp2_contrib_cms_pages_notice_EditNotice : Botwave.Web.PageBase
{
    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");

    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        set { workflowDefinitionService = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            noticeEditor1.EntityType = XQPHelper.EntityType_WorkflowNotice;
            noticeEditor1.ReturnUrl = AppPath + "apps/xqp2/pages/notices/notices.aspx";
            noticeEditor1.EntityDataBind += delegate(DropDownList container, EventArgs arg)
            {
                this.BindWorkflowData(container);
            };
        }
    }


    private void BindWorkflowData(DropDownList container)
    {
        IList<WorkflowDefinition> list = workflowDefinitionService.GetWorkflowDefinitionList();
        container.DataSource = list;
        container.DataTextField = "WorkflowName";
        container.DataValueField = "WorkflowName";
        container.DataBind();
        container.Items.Insert(0, new ListItem("---全部---", Guid.Empty.ToString()));
    }

}
