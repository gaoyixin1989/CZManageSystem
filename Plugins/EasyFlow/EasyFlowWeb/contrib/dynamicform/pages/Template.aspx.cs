using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Commons;
using Botwave.Web;
using Botwave.DynamicForm;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;
using NVelocity;
using NVelocity.App;

public partial class contrib_dynamicform_pages_Template : Botwave.Web.PageBase
{
    private IFormDefinitionService formDefinitionService = (IFormDefinitionService)Ctx.GetObject("formDefinitionService");

    public IFormDefinitionService FormDefinitionService
    {
        set { this.formDefinitionService = value; }
    }

    public Guid FormDefinitionId
    {
        get { return (Guid)(ViewState["FormDefinitionId"]); }
        set { ViewState["FormDefinitionId"] = value; }
    }

    public string EntityType
    {
        get { return Request.QueryString["EntityType"]; }
    }

    protected override void OnInit(EventArgs e)
    {
        string fdid = Request.QueryString["fdid"];
        if (String.IsNullOrEmpty(fdid))
        {
            ShowError(MessageHelper.Message_ArgumentException);
        }
        Guid formDefinitionId = new Guid(fdid);
        FormDefinitionId = formDefinitionId;
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/Base64.js");
        if (!IsPostBack)
            BindTemplate();
    }

    protected void BindTemplate()
    {
        FormDefinition definition = formDefinitionService.GetFormDefinitionById(FormDefinitionId);
        if (null == definition)
        {
            ShowError("未找到相应的表单定义数据.");
        }
        string formName = definition.Name;
        if (formName.EndsWith("表单"))
            formName = formName.Substring(0, formName.Length - 2);
        if (!string.IsNullOrEmpty(formName))
            this.ltlTitle.Text = formName + " - ";

        this.FCKContent.Value = definition.TemplateContent;
    }

    protected void btnSaveTemplate_Click(object sender, EventArgs e)
    {
        string template = Botwave.XQP.Commons.XQPHelper.DecodeBase64("utf-8", FCKContent.Value);
        template = Server.HtmlDecode(template);
        formDefinitionService.UpdateFormDefinitionTemplate(FormDefinitionId, template);

        ShowSuccess("保存模板成功！");
    }

    protected void btnReturn_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["wfid"].Equals(Guid.Empty.ToString()))
            Response.Redirect(string.Format("TemplateCreate.aspx?fdid={0}", FormDefinitionId));
        else
            Response.Redirect(string.Format("ItemCreate.aspx?fdid={0}&wid={1}&EntityType={2}", FormDefinitionId, Request.QueryString["wfid"], this.EntityType));
    }

    protected void btnPreview_Click(object sender, EventArgs e)
    {
        string content = Botwave.XQP.Commons.XQPHelper.DecodeBase64("utf-8",FCKContent.Value);
        VelocityEngine engine = VelocityEngineFactory.GetVelocityEngine();

        System.IO.StringWriter sw = new System.IO.StringWriter();
        VelocityContext vc = new VelocityContext();

        engine.Evaluate(vc, sw, "form log tag", StringUtils.HtmlDecode(content));
        FCKContent.Value = Server.HtmlDecode(content);
        lblPreview.Text = sw.GetStringBuilder().ToString();
    }
}
