using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.Web;
using Botwave.DynamicForm;
using Botwave.DynamicForm.Services;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Extension.Implements;
using Botwave.DynamicForm.Extension.Domain;

public partial class contrib_dynamicform_pages_config_UC_ValidateDataType : Botwave.Security.Web.PageBase
{
    #region properties
    private IFormDefinitionService formDefinitionService = (IFormDefinitionService)Ctx.GetObject("formDefinitionService");
    private IGetDataService getDataService = (IGetDataService)Ctx.GetObject("getDataService");
    private IJsLibraryService jsLibraryService = (IJsLibraryService)Ctx.GetObject("jsLibraryService");

    public IFormDefinitionService FormDefinitionService
    {
        get { return formDefinitionService; }
        set { formDefinitionService = value; }
    }

    public IGetDataService GetDataService
    {
        get { return getDataService; }
        set { getDataService = value; }
    }

    public IJsLibraryService JsLibraryService
    {
        get { return jsLibraryService; }
        set { jsLibraryService = value; }
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
            BindData(formItemDefinitionId);
        }
    }

    private void BindData(string formItemDefinitionId)
    {
        FormItemDefinition formItemDefinition = formDefinitionService.GetFormItemDefinitionById(new Guid(formItemDefinitionId));
        lbtnDelete.Visible = true;
        if (formItemDefinition != null)
        {
            if (!formItemDefinition.ItemType.Equals(FormItemDefinition.FormItemType.Text) && !formItemDefinition.ItemType.Equals(FormItemDefinition.FormItemType.TextArea))
            {
                ltlInfo.Text = " - 控件不支持此功能";
                ltlInfo1.Text = " - 控件不支持此功能";
                rptUsers.Visible = false;
                btnSave.Visible = false;
                btnSaveClose.Visible = false;
                lbtnDelete.Visible = false;
                lbtnDelete1.Visible = false;
            }
            else
            {
                IList<JsLibrary> jsLibraryList = jsLibraryService.GetLibraryList();
                rptUsers.DataSource = jsLibraryList;
                rptUsers.DataBind();
                FormItemExtension formItemExtension = getDataService.GetFormItemExtensionById(new Guid(formItemDefinitionId));
                if (formItemExtension != null)
                {
                    if (formItemExtension.ValidateType != 0)
                    {
                        JsLibrary jsLibrary = jsLibraryService.GetLibraryById(formItemExtension.ValidateType);
                        if (jsLibrary != null)
                        {
                            //hidId.Value = jsLibrary.Id.ToString();
                            ltlType.Text = jsLibrary.Events;
                            ltlDescript.Text = jsLibrary.Title;
                            ltlFunction.Text = jsLibrary.Function + "(#" + formItemDefinition.FName + "#)";
                            hidFunction.Value = jsLibrary.Function;
                        }
                        else
                            lbtnDelete.Visible = false;
                    }
                    else
                        lbtnDelete.Visible = false;

                    ltlFullName.Text = "[" + formItemDefinition.Name + "]" + "[" + formItemDefinition.FName + "]";
                    if (!string.IsNullOrEmpty(formItemExtension.DataEncode))
                    {
                        ltlStart.Text = formItemExtension.DataEncode.Split(':')[0];
                        ltlEnd.Text = formItemExtension.DataEncode.Split(':')[1];
                    }
                }
                else
                    lbtnDelete.Visible = false;
            }
        }
        
    }


    private bool CheckDataPrams(IList<FormItemDefinition> definitions, string prams)
    {
        IDictionary<string, string> dict = new Dictionary<string, string>();
        dict.Add("#WorkflowName#",string.Empty);
        dict.Add("#Wiid#", string.Empty);
        dict.Add("#Title#", string.Empty);
        dict.Add("#SheetId#", string.Empty);
        dict.Add("#CurrentUser#", string.Empty);
        dict.Add("#DpId#", string.Empty);
        dict.Add("#Aiid#", string.Empty);
        dict.Add("#ActivityName#", string.Empty);
        foreach (FormItemDefinition temp in definitions)
        {
            if (!dict.ContainsKey("#" + temp.FName + "#"))
                dict.Add("#" + temp.FName + "#", string.Empty);
        }
        foreach (KeyValuePair<string, string> temp in dict)
        {
            prams = prams.Replace(temp.Key, temp.Value);
        }
        foreach (KeyValuePair<string, string> temp in dict)
        {
            if(prams.Contains(temp.Key))
            return false;
        }
        return true;
    }

    private void SaveData()
    {
        FormItemExtension formItemExtension = getDataService.GetFormItemExtensionById(new Guid(FormItemDefinitionId));
        if (formItemExtension != null)
        {
            formItemExtension.ValidateType = Botwave.Commons.DbUtils.ToInt32(hidId.Value);
            formItemExtension.ValidateFunction = hidFunction.Value;
            formItemExtension.ValidateDescription = hidDescription.Value;
            if (Botwave.Commons.DbUtils.ToInt32(ltlStart.Text) > 0 && Botwave.Commons.DbUtils.ToInt32(ltlEnd.Text) > 0)
                formItemExtension.DataEncode = ltlStart.Text + ":" + ltlEnd.Text;
            getDataService.UpdateFormItemExtension(formItemExtension);
        }
        else
        {
            formItemExtension = new FormItemExtension();
            formItemExtension.FormItemDefinitionId = new Guid(FormItemDefinitionId);
            formItemExtension.ValidateType = Botwave.Commons.DbUtils.ToInt32(hidId.Value);
            formItemExtension.ValidateFunction = hidFunction.Value;
            formItemExtension.ValidateDescription = hidDescription.Value;
            if (Botwave.Commons.DbUtils.ToInt32(ltlStart.Text) > 0 && Botwave.Commons.DbUtils.ToInt32(ltlEnd.Text) > 0)
                formItemExtension.DataEncode = ltlStart.Text + ":" + ltlEnd.Text;
            getDataService.InserFormItemExtension(formItemExtension);
        }
        Response.Write("<script>alert('保存成功。');</script>");
        BindData(FormItemDefinitionId);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveData();
    }

    protected void btnSaveClose_Click(object sender, EventArgs e)
    {
        SaveData();
        Response.Write("<script>window.parent.close();</script>");
    }

    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        FormItemExtension formItemExtension = getDataService.GetFormItemExtensionById(new Guid(FormItemDefinitionId));
        if (formItemExtension != null)
        {
            formItemExtension.ValidateType = 0;
            formItemExtension.ValidateFunction = null;
            formItemExtension.ValidateDescription = null;
            getDataService.UpdateFormItemExtension(formItemExtension);
            BindData(formItemExtension.FormItemDefinitionId.ToString());
        }
    }

    protected void lbtnDelete1_Click(object sender, EventArgs e)
    {
        FormItemExtension formItemExtension = getDataService.GetFormItemExtensionById(new Guid(FormItemDefinitionId));
        if (formItemExtension != null)
        {
            formItemExtension.DataEncode = null;
            getDataService.UpdateFormItemExtension(formItemExtension);
            BindData(formItemExtension.FormItemDefinitionId.ToString());
        }
        BindData(FormItemDefinitionId);
    }
}
