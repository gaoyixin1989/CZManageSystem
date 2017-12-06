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
using Botwave.Workflow.Service;
using Botwave.Workflow.Domain;
using Botwave.Commons;

public partial class contrib_dynamicform_pages_config_UC_FormItemIFrames : Botwave.Security.Web.PageBase
{
    #region properties
    private IFormDefinitionService formDefinitionService = (IFormDefinitionService)Ctx.GetObject("formDefinitionService");
    private IGetDataService getDataService = (IGetDataService)Ctx.GetObject("getDataService");
    private IJsLibraryService jsLibraryService = (IJsLibraryService)Ctx.GetObject("jsLibraryService");
    private IActivityDefinitionService activityDefinitionService = (IActivityDefinitionService)Ctx.GetObject("activityDefinitionService");
    private IFormItemIFramesService formItemIFramesService = (IFormItemIFramesService)Ctx.GetObject("formItemIFramesService");
    private IItemDataListSettingService itemDataListSettingService = (IItemDataListSettingService)Ctx.GetObject("itemDataListSettingService");
    private IDataListDefinitionService dataListDefinitionService = (IDataListDefinitionService)Ctx.GetObject("dataListDefinitionService");

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

    public IActivityDefinitionService ActivityDefinitionService
    {
        get { return activityDefinitionService; }
        set { activityDefinitionService = value; }
    }

    public IFormItemIFramesService FormItemIFramesService
    {
        get { return formItemIFramesService; }
        set { formItemIFramesService = value; }
    }

    public IItemDataListSettingService ItemDataListSettingService
    {
        get { return itemDataListSettingService; }
        set { itemDataListSettingService = value; }
    }

    public IDataListDefinitionService DataListDefinitionService
    {
        get { return dataListDefinitionService; }
        set { dataListDefinitionService = value; }
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
            string workflowId = WorkflowId = Request.QueryString["wfid"];
            string formDefinitionId = FormDefinitionId = Request.QueryString["fdId"];
            string formItemDefinitionId = FormItemDefinitionId = Request.QueryString["fid"];
            string entityType = EntityType = Request.QueryString["EntityType"];
            BindData(formItemDefinitionId, workflowId);
        }
    }

    private void BindData(string formItemDefinitionId,string workflowId)
    {
        FormItemDefinition formItemDefinition = formDefinitionService.GetFormItemDefinitionById(new Guid(formItemDefinitionId));
        if (formItemDefinition != null)
        {
            if (!formItemDefinition.ItemType.Equals(FormItemDefinition.FormItemType.Html))
            {
                ltlInfo.Text = " - 此控件不支持此功能";
                btnSave.Visible = false;
                btnSaveClose.Visible = false;
            }
            else
            {
                IList<ActivityDefinition> activityDefinitions = activityDefinitionService.GetActivitiesByWorkflowId(new Guid(workflowId));
                IList<FormItemIFrames> formItemIFrames = formItemIFramesService.SelectByFormItemDefinitionId(new Guid(formItemDefinitionId));
                int index = 0;
                ddlActivityName.Items.Clear();
                ddlActivityName.Dispose();
                foreach (ActivityDefinition item in activityDefinitions)
                {
                    ddlActivityName.Items.Insert(index, new ListItem(item.ActivityName, item.ActivityName));
                    index++;
                }
                ddlActivityName.Items.Insert(0, new ListItem("- 请选择 -", ""));
                    
                index=0;
                lblLibrary.Items.Clear();
                lblLibrary.Dispose();
                foreach (FormItemIFrames item in formItemIFrames)
                {
                    lblLibrary.Items.Insert(index, new ListItem(item.SettingType == 0 ? "公共Iframe：" + item.AccessUrl : item.ActivityName + "：" + item.AccessUrl, item.SettingType == 0 ? "0$" + item.Height.ToString() + "$" + item.Width.ToString() : item.ActivityName + "$" + item.Height.ToString() + "$" + item.Width.ToString()));
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
        formItemIFramesService.Delete(new Guid(FormItemDefinitionId));
        //删除DataList设置，防止冲突
        IList<DataListItemDefinition> items = dataListDefinitionService.GetDataListItemDefinitionsByFormItemDefinitionId(new Guid(FormItemDefinitionId));
        Botwave.Extension.IBatisNet.IBatisMapper.Mapper.BeginTransaction();
        try
        {
            itemDataListSettingService.DataListSettingDelete(new Guid(FormItemDefinitionId));
            dataListDefinitionService.RemoveItemsFromForm(items);
            Botwave.Extension.IBatisNet.IBatisMapper.Mapper.CommitTransaction();
        }
        catch (Exception ex)
        {
            Botwave.Extension.IBatisNet.IBatisMapper.Mapper.RollBackTransaction();
            ShowError("删除失败！原因为：" + ex.ToString());
        }

        for (int i = 0; i < lblLibrary.Items.Count; i++)
        {
            if (formItemIFramesService.IsExists(new Guid(FormItemDefinitionId), lblLibrary.Items[i].Value.Split('$')[0], lblLibrary.Items[i].Value.Split('$')[0] == "0" ? 0 : 1))
            {
                FormItemIFrames item = null;
                if (lblLibrary.Items[i].Value.Split('$')[0] == "0")
                    item = formItemIFramesService.LoadByFormItemDefinitionIdAndType(new Guid(FormItemDefinitionId), 0);
                else
                    item = formItemIFramesService.LoadByFormItemDefinitionIdAndName(new Guid(FormItemDefinitionId), lblLibrary.Items[i].Value.Split('$')[0]);
                item.AccessUrl = lblLibrary.Items[i].Text.Split('：')[1];
                item.Height = Convert.ToInt32(lblLibrary.Items[i].Value.Split('$')[1]);
                item.Width = Convert.ToInt32(lblLibrary.Items[i].Value.Split('$')[2]);
                item.LastModifier = CurrentUserName;
                item.Enabled = 1;
                formItemIFramesService.Update(item);
            }
            else
            {
                FormItemIFrames item = new FormItemIFrames();
                item.FormItemDefinitionId = new Guid(FormItemDefinitionId);
                item.ActivityName = lblLibrary.Items[i].Value.Split('$')[0] == "0" ? "0" : lblLibrary.Items[i].Value.Split('$')[0];
                item.AccessUrl = lblLibrary.Items[i].Text.Split('：')[1];
                item.Height = Convert.ToInt32(lblLibrary.Items[i].Value.Split('$')[1]);
                item.Width = Convert.ToInt32(lblLibrary.Items[i].Value.Split('$')[2]);
                item.SettingType = lblLibrary.Items[i].Value.Split('$')[0] == "0" ? 0 : 1;
                item.Enabled = 1;
                item.Creator = CurrentUserName;
                item.LastModifier = CurrentUserName;
                formItemIFramesService.Create(item);
            }
        }
        Response.Write("<script>alert('保存成功。');</script>");
        BindData(FormItemDefinitionId, WorkflowId);
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

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        foreach (ListItem item in lblLibrary.Items)
        {
            if (item.Selected)
            {
                lblLibrary.Items.Remove(item);
                break;
            }
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        int index=0;
        if (lblLibrary.Items.Count == 0)
        {
            if (!string.IsNullOrEmpty(txtPubUrl.Text))
            {
                lblLibrary.Items.Insert(index, new ListItem("公共Iframe：" + txtPubUrl.Text, "0$" + DbUtils.ToInt32(txtPubH.Text) + "$" + DbUtils.ToInt32(txtPubW.Text)));
                index++;
            }
            if (!string.IsNullOrEmpty(txtActivityName.Text) && ddlActivityName.SelectedIndex > 0)
            {
                lblLibrary.Items.Insert(index, new ListItem(ddlActivityName.SelectedValue + "：" + txtActivityName.Text, ddlActivityName.SelectedValue + "$" + DbUtils.ToInt32(txtActH.Text) + "$" + DbUtils.ToInt32(txtActW.Text)));
                index++;
            }
        }
        else
        {
            bool canpub = true;
            bool canact = true;
            foreach (ListItem item in lblLibrary.Items)
            {
                if (item.Value.Split('$')[0] == "0" && !string.IsNullOrEmpty(txtPubUrl.Text))
                {
                    item.Text = "公共Iframe：" + txtPubUrl.Text;
                    item.Value = "0$" + DbUtils.ToInt32(txtPubH.Text) + "$" + DbUtils.ToInt32(txtPubW.Text);
                    canpub = false;
                }
                if (item.Value.Split('$')[0] == ddlActivityName.SelectedValue && !string.IsNullOrEmpty(txtActivityName.Text))
                {
                    item.Text = ddlActivityName.SelectedValue + "：" + txtActivityName.Text;
                    item.Value = ddlActivityName.SelectedValue + "$" + DbUtils.ToInt32(txtActH.Text) + "$" + DbUtils.ToInt32(txtActW.Text);
                    canact = false;
                }
            }
            if (canpub && !string.IsNullOrEmpty(txtPubUrl.Text))
            {
                lblLibrary.Items.Insert(index, new ListItem("公共Iframe：" + txtPubUrl.Text, "0$" + DbUtils.ToInt32(txtPubH.Text) + "$" + DbUtils.ToInt32(txtPubW.Text)));
                index++;
            }
            if (canact && !string.IsNullOrEmpty(txtActivityName.Text) && ddlActivityName.SelectedIndex > 0)
            {
                lblLibrary.Items.Insert(index, new ListItem(ddlActivityName.SelectedValue + "：" + txtActivityName.Text, ddlActivityName.SelectedValue + "$" + DbUtils.ToInt32(txtActH.Text) + "$" + DbUtils.ToInt32(txtActW.Text)));
                index++;
            }
        }
    }
}
