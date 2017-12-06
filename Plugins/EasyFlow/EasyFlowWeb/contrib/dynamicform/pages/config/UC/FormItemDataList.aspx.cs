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
using System.Text;
using System.Collections.Specialized;
using System.Linq;
//using Newtonsoft.Json.Linq;
//using Newtonsoft.Json;

public partial class contrib_dynamicform_pages_config_UC_FormItemDataList : Botwave.Security.Web.PageBase
{
    #region properties
    private IFormDefinitionService formDefinitionService = (IFormDefinitionService)Ctx.GetObject("formDefinitionService");
    private IGetDataService getDataService = (IGetDataService)Ctx.GetObject("getDataService");
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
                DataListSetting setting = itemDataListSettingService.GetDataListSetting(new Guid(formItemDefinitionId));
                if (setting != null)
                {
                    txtPubH.Text = setting.Rows.ToString();
                    txtPubW.Text = setting.Columns.ToString();
                    IList<DataListItemDefinition> definitions = dataListDefinitionService.GetDataListItemDefinitionsByFormItemDefinitionId(new Guid(formItemDefinitionId));
                    chkNoLimit.Checked = setting.Type == 1;
                    if (definitions.Count > 0)
                    {
                        StringBuilder table = new StringBuilder();
                        for (int i = 0; i < setting.Columns; i++)
                        {
                            table.AppendLine("<tr><td>"+(i+1)+"</td><td><input type='text' class='expand_input' name='bwdf_title_" + definitions[i].Id + "' value='" + definitions[i].Name + "'/></td>");
                            table.AppendLine("<td><select class='expand_input' name='bwdf_list_" + definitions[i].Id + "' id='bwdf_list_" + definitions[i].Id + "'>");
                            table.AppendLine("<option value='0' " + (definitions[i].ItemType.Equals((DataListItemDefinition.FormItemType)0) ? " selected" : "") + ">单行输入</option>");
                            table.AppendLine("<option value='1' " + (definitions[i].ItemType.Equals((DataListItemDefinition.FormItemType)1) ? " selected" : "") + ">多行输入</option>");
                            table.AppendLine("<option value='6' " + (definitions[i].ItemType.Equals((DataListItemDefinition.FormItemType)6) ? " selected" : "") + ">日期</option>");
                            table.AppendLine("<option value='2' " + (definitions[i].ItemType.Equals((DataListItemDefinition.FormItemType)2) ? " selected" : "") + ">下拉框</option>");
                            table.AppendLine("<option value='4' " + (definitions[i].ItemType.Equals((DataListItemDefinition.FormItemType)4) ? " selected" : "") + ">复选框</option>");
                            table.AppendLine("<option value='5' " + (definitions[i].ItemType.Equals((DataListItemDefinition.FormItemType)5) ? " selected" : "") + ">单选框</option>");
                            table.AppendLine("</select></td>");
                            table.AppendLine("<td><input type='text' name='source_" + definitions[i].Id + "'  value='" + definitions[i].DataSource + "'/></td>");
                            table.AppendLine("<td><input type='checkbox' name='chkRequire_" + definitions[i].Id + "' " + (definitions[i].Require?"checked='checked'":"") + " value='true'/></tb>");
                            //table.AppendLine("<td><select class=\"expand_input\" name=\"ddlValidateType_" + definitions[i].Id + "\"><option value=\"\">不需要</option><option " + (definitions[i].ValidateType == "require" ? "selected" : "") + " value=\"requie\">非空</option><option " + (definitions[i].ValidateType == "group" ? "selected" : "") + " value=\"group\">必选</option></select></td>");
                            table.AppendLine("<td><input type='text' name='errmsg_" + definitions[i].Id + "'  value='" + definitions[i].ErrorMessage + "'/></td>");
                            table.AppendLine("<td><input type='hidden' class='expand_input' value='"+definitions[i].Id+"'/><a action=\"up\" onclick=\"moveUp(this)\" href=\"javascript:void(0)\">上移</a>&nbsp;<a action=\"down\"  onclick=\"moveDown(this)\" href=\"javascript:void(0)\">下移</a>&nbsp;<a href='#' onclick='del(this)' a_del='1'>删除</a></td></td>");
                            table.AppendLine("</tr>");
                        }
                        ltlTr.Text = table.ToString();
                    }
                    
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
        //删除Iframe设置，防止冲突
        formItemIFramesService.Delete(new Guid(FormItemDefinitionId));
        bool canSave = true;
        if (!string.IsNullOrEmpty(hidIntroduceJson.Value))
        {
            IDictionary<string, Guid> dict = new Dictionary<string, Guid>();
            FormItemDefinition formItemDefinition = formDefinitionService.GetFormItemDefinitionById(new Guid(FormItemDefinitionId));
            int columnNum = Convert.ToInt32(txtPubW.Text);
            IList<DataListItemDefinition> itemDefinitions = dataListDefinitionService.GetDataListItemDefinitionsByFormItemDefinitionId(new Guid(FormItemDefinitionId));
            DataListSetting setting = itemDataListSettingService.GetDataListSetting(new Guid(FormItemDefinitionId));

            IList<DataListItemDefinition> newItemDefinitions = new List<DataListItemDefinition>();
            foreach (DataListItemDefinition itemDefinition in itemDefinitions)
            {
                dict.Add(itemDefinition.FName, itemDefinition.Id);
            }
            if (setting != null)
            {
                //setting.FormItemDefinitionId = new Guid(FormItemDefinitionId);
                setting.Columns = columnNum;
                setting.Rows = Convert.ToInt32(txtPubH.Text);
                setting.Type = chkNoLimit.Checked ? 1 : 0;
                itemDataListSettingService.DataListSettingUpdate(setting);
            }
            else
            {
                setting = new DataListSetting();
                setting.FormItemDefinitionId = new Guid(FormItemDefinitionId);
                setting.Columns = columnNum;
                setting.Rows = Convert.ToInt32(txtPubH.Text);
                setting.Type = chkNoLimit.Checked ? 1 : 0;
                itemDataListSettingService.DataListSettingInsert(setting);
            }
            //JArray jr = (JArray)JsonConvert.DeserializeObject(hidIntroduceJson.Value);
            LitJson.JsonData jr = LitJson.JsonMapper.ToObject(hidIntroduceJson.Value);
            //foreach (JObject jo in jr)
            foreach (LitJson.JsonData jo in jr)
            {
                //string id = jo["id"].Value<string>();
                //string title = jo["title"].Value<string>();
                //string sort = jo["sort"].Value<string>();
                //string itemtype = jo["itemtype"].Value<string>();
                //string datasource = jo["datasource"].Value<string>();
                //string require = jo["require"].Value<string>();
                //string validatetype = jo["validatetype"].Value<string>();
                //string errmsg = jo["errmsg"].Value<string>();
                string id = (string)jo["id"];
                string title = (string)jo["title"];
                string sort = (string)jo["sort"];
                string itemtype = (string)jo["itemtype"];
                string datasource = (string)jo["datasource"];
                string require = (string)jo["require"];
                string validatetype = (string)jo["validatetype"];
                string errmsg = (string)jo["errmsg"];
                string type = itemtype;
                if (string.IsNullOrEmpty(title))
                {
                    //Response.Write("<script>alert('第[" + (i + 1) + "]列字段意义不能为空。');</script>");
                    //canSave = false;
                    //break;
                    continue;
                }

                DataListItemDefinition item = null;
                if (!id.Equals("add") && itemDefinitions.Where(d => d.Id == new Guid(id)).Count() > 0)
                {
                    item = itemDefinitions.Where(d => d.Id == new Guid(id)).First();
                    //ids.Add(item.Id);
                }
                else
                {
                    item = new DataListItemDefinition();
                    item.Id = Guid.NewGuid();
                }
                int itemType = DbUtils.ToInt32(type);
                item.ItemType = (DataListItemDefinition.FormItemType)itemType;

                item.FormItemDefinitionId = new Guid(FormItemDefinitionId);
                item.FName = formItemDefinition.FName + "_c" + (DbUtils.ToInt32(sort) - 1);
                item.Name = title;
                item.Require = DbUtils.ToBoolean(require);
                if (item.Require)
                {
                    if (item.ItemType.Equals(DataListItemDefinition.FormItemType.RadioButtonList) && item.ItemType.Equals(DataListItemDefinition.FormItemType.CheckBoxList))
                        item.ValidateType = "group";
                    else
                        item.ValidateType = "require";
                }
                
                item.DataSource = datasource;
                item.ColumnNumber = DbUtils.ToInt32(sort) - 1;
                item.ErrorMessage = errmsg;
                //item.ItemType = (DataListItemDefinition.FormItemType)0;
                if (item.ItemType.Equals(1))
                    item.ItemDataType = (DataListItemDefinition.DataType)2;

                //switch (item.ItemType)
                //{
                //    case DataListItemDefinition.FormItemType.Text:
                //        //item.Comment = txtComment.Text.Trim();
                //        item.ItemDataType = (DataListItemDefinition.DataType)0;
                //        break;
                //    case DataListItemDefinition.FormItemType.TextArea:

                //        break;
                //    case DataListItemDefinition.FormItemType.Date:
                //        break;
                //    case DataListItemDefinition.FormItemType.CheckBoxList:
                //        break;
                //    case DataListItemDefinition.FormItemType.DropDownList:
                //        break;
                //    case DataListItemDefinition.FormItemType.RadioButtonList:
                //        break;
                //}
                newItemDefinitions.Add(item);
                //else
                //    itemDefinitions.Where(d => d.Id == new Guid(id)).First() = item;
            }
            if (canSave)
            {
                dataListDefinitionService.AppendItemsToForm(newItemDefinitions, dict);
                Response.Write("<script>alert('保存成功。');window.location='"+Request.RawUrl+"'</script>");
                //BindData(FormItemDefinitionId, WorkflowId);
            }
        }
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
        IList<DataListItemDefinition> items = dataListDefinitionService.GetDataListItemDefinitionsByFormItemDefinitionId(new Guid(FormItemDefinitionId));
        Botwave.Extension.IBatisNet.IBatisMapper.Mapper.BeginTransaction();
        try
        {
            itemDataListSettingService.DataListSettingDelete(new Guid(FormItemDefinitionId));
            dataListDefinitionService.RemoveItemsFromForm(items);
            Botwave.Extension.IBatisNet.IBatisMapper.Mapper.CommitTransaction();
            Response.Write("<script>alert('删除成功。');window.location='" + Request.RawUrl + "'</script>");
        }
        catch (Exception ex){
            Botwave.Extension.IBatisNet.IBatisMapper.Mapper.RollBackTransaction();
            ShowError("删除失败！原因为："+ex.ToString());
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        int index=0;
        
    }
}
