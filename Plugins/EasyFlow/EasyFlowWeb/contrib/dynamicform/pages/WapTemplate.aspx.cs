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
using System.Data.SqlClient;
using Botwave.Extension.IBatisNet;

public partial class contrib_dynamicform_pages_WapTemplate : Botwave.Web.PageBase
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
        SqlParameter[] pa = new SqlParameter[1];
        pa[0] = new SqlParameter("@formDefinitionId", SqlDbType.UniqueIdentifier);
        pa[0].Value = FormDefinitionId;
        object result = IBatisDbHelper.ExecuteScalar(CommandType.Text, "select WapTemplateContent from bwdf_FormDefinitions where id=@formDefinitionId ", pa);
        this.FCKContent.Value = Botwave.Commons.DbUtils.ToString(result);
    }

    protected void btnSaveTemplate_Click(object sender, EventArgs e)
    {
        string template = Botwave.XQP.Commons.XQPHelper.DecodeBase64("utf-8", FCKContent.Value);
        template = Server.HtmlDecode(template);

        //formDefinitionService.UpdateFormDefinitionTemplate(FormDefinitionId, template);
        SqlParameter[] pa = new SqlParameter[2];
        pa[0] = new SqlParameter("@formDefinitionId", SqlDbType.UniqueIdentifier);
        pa[0].Value = FormDefinitionId;
        pa[1] = new SqlParameter("@TemplateContent", SqlDbType.NText);
        pa[1].Value = template;
        object result = IBatisDbHelper.ExecuteScalar(CommandType.Text, "update bwdf_FormDefinitions set WapTemplateContent=@TemplateContent where id=@formDefinitionId ", pa);

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
        string content = Botwave.XQP.Commons.XQPHelper.DecodeBase64("utf-8", FCKContent.Value);
        VelocityEngine engine = VelocityEngineFactory.GetVelocityEngine();

        System.IO.StringWriter sw = new System.IO.StringWriter();
        VelocityContext vc = new VelocityContext();

        engine.Evaluate(vc, sw, "form log tag", StringUtils.HtmlDecode(content));

        FCKContent.Value = Server.HtmlDecode(content);

        lblPreview.Text = sw.GetStringBuilder().ToString();

        ltlIframe.Text = "<iframe id=\"Iframe\"  style=\"border: solid 1px #DFDFE8;\" src=\"preview.aspx\" frameborder=\"0\" width=\"" + txtWidth.Text + "\" scrolling=\"auto\" height=\"" + txtHeight.Text + "\"></iframe>";
    }
}
