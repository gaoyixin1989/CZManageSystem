using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Commons;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.XQP.Domain;

public partial class apps_xqp2_pages_workflows_config_ConfigCreation : Botwave.Web.PageBase
{
    /// <summary>
    /// 流程名称.
    /// </summary>
    public string WorkflowName
    {
        get { return (string)ViewState["WorkflowName"]; }
        set { ViewState["WorkflowName"] = value; }
    }

    public Guid WorkflowId
    {
        get { return (Guid)ViewState["WorkflowId"]; }
        set { ViewState["WorkflowId"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //string workflowName = Request.QueryString["name"];
            string workflowName = Server.HtmlEncode(Request.QueryString["name"]);
            if (string.IsNullOrEmpty(workflowName))
            {
                ShowError("查询字符串参数错误.");
            }

            IList<WorkflowCreationControl> creationControls = WorkflowCreationControl.GetWorkflowCreationControls(workflowName);
            if (creationControls.Count > 0)
            {
                this.LoadWorkflowCreationControls(creationControls);
            }
            this.WorkflowName = workflowName;
        }
    }

    /// <summary>
    /// 加载流程发单控制列表.
    /// </summary>
    /// <param name="items"></param>
    private void LoadWorkflowCreationControls(IList<WorkflowCreationControl> items)
    {
        foreach (WorkflowCreationControl item in items)
        {
            int urgency = item.Urgency;
            if (urgency == 0) // 一般
            {
                wcc1.BindData(item);
            }
            else if (urgency == 1) // 紧急
            {
                wcc2.BindData(item);
            }
            else if (urgency == 2) // 最紧急
            {
                wcc3.BindData(item);
            }
        }
    }

    // 保存
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string workflowName = this.WorkflowName;
        // 一般
        WorkflowCreationControl item = wcc1.GetData(workflowName, 0);
        item.Insert();

        // 紧急
        item = wcc2.GetData(workflowName, 1);
        item.Insert();

        // 最紧急
        item = wcc3.GetData(workflowName, 2);
        item.Insert();

        ShowSuccess("成功保存流程发单控制设置.");
    }
}
