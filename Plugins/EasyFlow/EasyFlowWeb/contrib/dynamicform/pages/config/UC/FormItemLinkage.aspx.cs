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

public partial class contrib_dynamicform_pages_config_UC_FormItemLinkage : Botwave.Security.Web.PageBase
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
            //if (formItemDefinition.ItemType.Equals(FormItemDefinition.FormItemType.DropDownList))
            if ((int)formItemDefinition.ItemType < 7 || formItemDefinition.ItemType.Equals(FormItemDefinition.FormItemType.Html))
            {
                FormItemExtension formItemExtension = getDataService.GetFormItemExtensionById(new Guid(formItemDefinitionId));
                if (formItemExtension != null)
                {
                    radWay_0.Checked = formItemExtension.ItemsLinkageType == 0;
                    radWay_1.Checked = formItemExtension.ItemsLinkageType == 1;
                    radWay_2.Checked = formItemExtension.ItemsLinkageType == 2;
                    if (radWay_1.Checked)
                        hidJson.Value = formItemExtension.ItemsLinkageJson;
                    else if (radWay_2.Checked)
                    {
                        txtSQL.Value = formItemExtension.ItemsLinkageJson;
                        txtSqlConnection.Text = formItemExtension.ItemsLinkageSource;
                    }
                }
                ltlName.Text = formItemDefinition.Name;
                ltlFName.Text = formItemDefinition.FName;
                ltlFatherN.Text = "[" + formItemDefinition.Name + "]";
                ltlFatherF.Text = "[" + formItemDefinition.FName + "]";
                IList<FormItemDefinition> formItemDefinitions = formDefinitionService.GetFormItemDefinitionsByFormDefinitionId(new Guid(FormDefinitionId));
                int indtex = 0;
                ddlFName.Items.Clear();
                ddlFName.Dispose();
                ddlFName.Items.Insert(indtex, new ListItem("－ 请选择 －", ""));
                foreach (FormItemDefinition item in formItemDefinitions)
                {
                    //if (!item.ItemType.Equals(FormItemDefinition.FormItemType.DropDownList))
                    if ((int)item.ItemType > 6 && !item.ItemType.Equals(FormItemDefinition.FormItemType.Html))
                    {
                        continue;
                    }
                    indtex++;
                    ddlFName.Items.Insert(indtex, new ListItem(item.Name + " - " + item.FName, item.Id.ToString()));
                }
                indtex = 0;
                ddlFather.Items.Clear();
                ddlFather.Dispose();
                foreach (string str in formItemDefinition.DataSource.Split(','))
                {
                    ddlFather.Items.Insert(indtex, new ListItem(str, formItemDefinition.FName));
                    indtex++;
                }
            }
            else
            {
                ltlInfo.Text = " - 控件不支持此功能。";
                btnSave.Visible = false;
                btnSaveClose.Visible = false;
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
                formItemExtension.ItemsLinkageType = 1;
                formItemExtension.ItemsLinkageJson = hidJson.Value;
                formItemExtension.ItemsLinkageSource = null;
            }
            else if (radWay_2.Checked)
            {
                formItemExtension.ItemsLinkageType = 2;
                formItemExtension.ItemsLinkageSource = txtSqlConnection.Text;
                formItemExtension.ItemsLinkageJson = txtSQL.Value;
            }
            else if (radWay_0.Checked)
            {
                formItemExtension.ItemsLinkageType = 0;
                formItemExtension.ItemsLinkageSource = null;
                formItemExtension.ItemsLinkageJson = null;
            }
            getDataService.UpdateFormItemExtension(formItemExtension);
        }
        else
        {
            formItemExtension = new FormItemExtension();
            formItemExtension.FormItemDefinitionId = new Guid(FormItemDefinitionId);
            if (radWay_1.Checked)
            {
                formItemExtension.ItemsLinkageType = 1;
                formItemExtension.ItemsLinkageJson = hidJson.Value;
                formItemExtension.ItemsLinkageSource = null;
            }
            else if (radWay_2.Checked)
            {
                formItemExtension.ItemsLinkageType = 2;
                formItemExtension.ItemsLinkageSource = txtSqlConnection.Text;
                formItemExtension.ItemsLinkageJson = txtSQL.Value;
            }
            else if(radWay_0.Checked)
            {
                formItemExtension.ItemsLinkageType = 0;
                formItemExtension.ItemsLinkageSource = null;
                formItemExtension.ItemsLinkageJson = null;
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

    protected void ddlFName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFName.SelectedIndex > 0)
        {
            FormItemDefinition formItemDefinition = formDefinitionService.GetFormItemDefinitionById(new Guid(ddlFName.SelectedValue));
            ddlChildren.Items.Clear();
            ddlChildren.Dispose();
            ltlChildrenF.Text = null;
            ltlChildrenN.Text = null;
            if (formItemDefinition != null)
            {
                int indtex = 0;
                ltlChildrenF.Text = "[" + formItemDefinition.FName + "]";
                ltlChildrenN.Text = "[" + formItemDefinition.Name + "]";
                txtChildren.ReadOnly = false;
                if (!formItemDefinition.ItemType.Equals(FormItemDefinition.FormItemType.Html))
                {
                    foreach (string str in formItemDefinition.DataSource.Split(','))
                    {
                        ddlChildren.Items.Insert(indtex, new ListItem(str, formItemDefinition.FName));
                        indtex++;
                    }
                }
                else
                {
                    txtChildren.Text = "{GetOuterData}";
                    txtChildren.ReadOnly = true;
                }
                ddlChildren.Items.Insert(indtex, new ListItem("{GetOuterData}", formItemDefinition.FName));
                
            }
        }
    }
}
