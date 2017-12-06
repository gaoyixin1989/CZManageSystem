using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;
using Botwave.Web;

public partial class contrib_dynamicform_pages_config_AdvancedConfig : Botwave.Security.Web.PageBase
{
    #region properties
    private IFormDefinitionService formDefinitionService = (IFormDefinitionService)Ctx.GetObject("formDefinitionService");

    public IFormDefinitionService FormDefinitionService
    {
        get { return formDefinitionService; }
        set { formDefinitionService = value; }
    }
    #endregion

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
        System.Web.UI.HtmlControls.HtmlGenericControl child = new System.Web.UI.HtmlControls.HtmlGenericControl("link");
        Page handler = (Page)HttpContext.Current.Handler;
        handler.Header.Controls.Clear();//清理
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/common.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/jquery-latest.pack.js");
        if (!IsPostBack)
        {
            string workflowId = WorkflowId = Request.QueryString["wid"];
            string formDefinitionId = FormDefinitionId = Request.QueryString["fdId"];
            string formItemDefinitionId = FormItemDefinitionId = Request.QueryString["fid"];
            string entityType = EntityType = Request.QueryString["EntityType"];
            FormItemDefinition formItemDefinition = formDefinitionService.GetFormItemDefinitionById(new Guid(formItemDefinitionId));
            if (formItemDefinition != null)
            {

                Page.Title = "为" + "[" + formItemDefinition.Name + "]" + "[" + formItemDefinition.FName + "]" + "设置扩展设置";
            }
        }
    }
}
