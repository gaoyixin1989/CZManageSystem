using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave;
using Botwave.Commons;
using Botwave.DynamicForm;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;

public partial class contrib_dynamicform_pages_List : Botwave.Web.PageBase
{
    protected int RowIndex = 1;
    protected Guid WorkflowId = Guid.Empty;

    private IFormDefinitionService formDefinitionService = Spring.Context.Support.WebApplicationContext.Current["FormDefinitionService"] as IFormDefinitionService;

    public IFormDefinitionService FormDefinitionService
    {
        set { this.formDefinitionService = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();
        }
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        if (!IsPostBack)
        {
            Guid? workflowId = DbUtils.ToGuid(Request["wid"]);
            if (!workflowId.HasValue)
            {
                ShowError(GlobalSettings.Instance.ArgumentExceptionMessage);
            }
            this.WorkflowId = workflowId.Value;
            this.hiddenWorkflowId.Value = workflowId.ToString();
        }
    }

    private void Bind()
    {
        Guid workflowId = DbUtils.ToGuid(hiddenWorkflowId.Value).Value;
        FormDefinition definition = formDefinitionService.GetFormDefinitionByExternalEntity("Form_Workflow", workflowId);
        if (null == definition)
            return;

        string formName = definition.Name;
        if (formName.EndsWith("表单"))
            formName = formName.Substring(0, formName.Length - 2);
        if (!string.IsNullOrEmpty(formName))
            this.ltlTitle.Text = formName + " - ";

        IList<FormDefinition> formList = Botwave.XQP.ImportExport.WorkflowFormHelper.GetFormDefinitions(definition.Name);
        rptForms.DataSource = formList;
        rptForms.DataBind();
    }

    protected string FormatBoolean(object value)
    {
        bool isChecked = DbUtils.ToBoolean(value);
        return string.Format("<input type=\"checkbox\" disabled=\"disabled\"{0} />", isChecked ? " checked=\"checked\"" : string.Empty);
    }

    protected void rptForms_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        FormDefinition item = e.Item.DataItem as FormDefinition;
        if (item == null || !item.IsCurrentVersion)
            return;

        LinkButton linkDelete = e.Item.FindControl("linkDelete") as LinkButton;
        LinkButton linkSetCurent = e.Item.FindControl("linkSetCurrent") as LinkButton;
        if (linkDelete != null)
            linkDelete.Visible = false;
        if (linkSetCurent != null)
            linkSetCurent.Visible = false;
    }

    protected void rptForms_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        Guid? formId = DbUtils.ToGuid(e.CommandArgument);
        if (!formId.HasValue)
            return;
        Guid workflowId = DbUtils.ToGuid(hiddenWorkflowId.Value).Value;
        string actor = Botwave.Security.LoginHelper.UserName;
        if (e.CommandName == "Export")
        {
            Botwave.XQP.ImportExport.WorkflowFormHelper.Export(this.Response, formId.Value);
        }
        else if (e.CommandName == "Delete")
        {
            if (Botwave.XQP.ImportExport.WorkflowFormHelper.Delete(formId.Value, actor))
            {
                ShowSuccess("删除表单成功。");
            }
        }
        else if (e.CommandName == "SetCurrentVersion")
        {
            if (Botwave.XQP.ImportExport.WorkflowFormHelper.SetCurrentVersion(workflowId, formId.Value, actor))
            {
                ShowSuccess("设置为当前版本成功。");
            }
        }
    }
}
