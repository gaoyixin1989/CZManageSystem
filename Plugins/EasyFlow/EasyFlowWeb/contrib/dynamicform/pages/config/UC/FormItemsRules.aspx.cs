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

public partial class contrib_dynamicform_pages_config_UC_FormItemsRules : Botwave.Security.Web.PageBase
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
        if (formItemDefinition != null)
        {
            if (formItemDefinition.ItemType.Equals(FormItemDefinition.FormItemType.File) || formItemDefinition.ItemType.Equals(FormItemDefinition.FormItemType.Html))
            {
                ltlInfo.Text = " - 此控件不支持此功能";
                btnSave.Visible = false;
                btnSaveClose.Visible = false;
            }
            else
            {
                FormItemExtension formItemExtension = getDataService.GetFormItemExtensionById(new Guid(formItemDefinitionId));
                if (formItemExtension != null)
                {
                    radWay_0.Checked = formItemExtension.ItemsRulesType == 0;
                    radWay_1.Checked = formItemExtension.ItemsRulesType == 1;
                    if (radWay_1.Checked) { }
                    hidJson.Value = formItemExtension.ItemsRulesJson;
                }
                hidFName.Value = formItemDefinition.FName;
                ltlName.Text = formItemDefinition.Name;
                ltlFName.Text = formItemDefinition.FName;
                ltlFatherN.Text = "[" + formItemDefinition.Name + "]";
                ltlFatherF.Text = "[" + formItemDefinition.FName + "]";
                IList<FormItemDefinition> formItemDefinitions = formDefinitionService.GetFormItemDefinitionsByFormDefinitionId(new Guid(FormDefinitionId));
                int indtex = 0;
                ddlCName.Items.Clear();
                ddlCName.Dispose();
                ddlCName.Items.Insert(indtex, new ListItem("－ 请选择 －", ""));
                foreach (FormItemDefinition item in formItemDefinitions)
                {
                    if (item.ItemType.Equals(FormItemDefinition.FormItemType.File) || item.ItemType.Equals(FormItemDefinition.FormItemType.Html) || item.ItemType.Equals(FormItemDefinition.FormItemType.Hidden))
                        continue;
                    indtex++;
                    ddlCName.Items.Insert(indtex, new ListItem(item.Name + " - " + item.FName, item.FName+"$"+item.ItemType.ToString()));
                }
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
            if (radWay_1.Checked)
            {
                formItemExtension.ItemsRulesType = 1;
                formItemExtension.ItemsRulesJson = hidJson.Value;
            }
            else if (radWay_0.Checked)
            {
                formItemExtension.ItemsRulesType = 0;
                formItemExtension.ItemsRulesJson = null;
            }
            getDataService.UpdateFormItemExtension(formItemExtension);
        }
        else
        {
            formItemExtension = new FormItemExtension();
            formItemExtension.FormItemDefinitionId = new Guid(FormItemDefinitionId);
            if (radWay_1.Checked)
            {
                formItemExtension.ItemsRulesType = 1;
                formItemExtension.ItemsRulesJson = hidJson.Value;
            }
            else if(radWay_0.Checked)
            {
                formItemExtension.ItemsRulesType = 0;
                formItemExtension.ItemsRulesJson = null;
            }
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
}
