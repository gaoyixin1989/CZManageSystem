using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using Botwave.DynamicForm.Renders;
using Botwave.DynamicForm.Binders;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Util;
using NVelocity;
using NVelocity.App;
using Botwave.DynamicForm.Extension.Implements;
using Botwave.DynamicForm.Extension.Domain;
using Botwave.DynamicForm.Extension.Renders;

public partial class contrib_dynamicform_pages_ItemCreate : Botwave.Security.Web.PageBase
{
    #region service properties

    private IFormDefinitionService formDefinitionService = (IFormDefinitionService)Ctx.GetObject("formDefinitionService");
    private IFormItemDataBinder formItemDataBinder = (IFormItemDataBinder)Ctx.GetObject("formItemDataBinder");
    private IRenderStrategy renderStrategy = (IRenderStrategy)Ctx.GetObject("renderStrategy");
    private IDivRenderStrategy divRenderStrategy = (IDivRenderStrategy)Ctx.GetObject("divRenderStrategy");
    private IActivityDefinitionService activityDefinitionService = (IActivityDefinitionService)Ctx.GetObject("activityDefinitionService");
    private IGetDataService getDataService = (IGetDataService)Ctx.GetObject("getDataService");
    private IFormItemIFramesService formItemIFramesService = (IFormItemIFramesService)Ctx.GetObject("formItemIFramesService");
    private IItemDataListSettingService itemDataListSettingService = (IItemDataListSettingService)Ctx.GetObject("itemDataListSettingService");
    private IDataListDefinitionService dataListDefinitionService = (IDataListDefinitionService)Ctx.GetObject("dataListDefinitionService");

    public IFormDefinitionService FormDefinitionService
    {
        set { this.formDefinitionService = value; }
    }

    public IRenderStrategy RenderStrategy
    {
        set { this.renderStrategy = value; }
    }

    public IDivRenderStrategy DivRenderStrategy
    {
        set { this.divRenderStrategy = value; }
    }

    public IFormItemDataBinder FormItemDataBinder
    {
        set { this.formItemDataBinder = value; }
    }

    public IActivityDefinitionService ActivityDefinitionService
    {
        set { activityDefinitionService = value; }
    }

    public IGetDataService GetDataService
    {
        get { return getDataService; }
        set { getDataService = value; }
    }

    public IFormItemIFramesService FormItemIFramesService
    {
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
    public string ReturnURL = string.Empty;
    #endregion

    #region properties

    private Guid formDefinitionId;
    private Guid workflowId;

    public Guid FormDefinitionId
    {
        get { return formDefinitionId; }
        set { formDefinitionId = value; }
    }

    public Guid WorkflowId
    {
        get { return workflowId; }
        set { workflowId = value; }
    }
    public string EntityType
    {
        get { return Request.QueryString["EntityType"]; }
    }
    #endregion

    protected override void OnInit(EventArgs e)
    {
        string fdid = Request.QueryString["fdid"];
        string wid = Request.QueryString["wid"];
        if (String.IsNullOrEmpty(fdid) && String.IsNullOrEmpty(wid))
        {
            ShowError(MessageHelper.Message_ArgumentException);
        }

        WorkflowId = new Guid(wid);
        Guid definitionId = Guid.Empty;
        if (string.IsNullOrEmpty(fdid))
        {
            FormDefinition temp = formDefinitionService.GetFormDefinitionByExternalEntity(EntityType, WorkflowId);
            if (null != temp)
                definitionId = temp.Id;
        }
        else
        {
            definitionId = new Guid(fdid);
        }

        if (definitionId == Guid.Empty)
        {
            ShowError("未找到相应的表单定义数据.", ReturnURL);
        }

        this.FormDefinitionId = definitionId;
        hdFormDefinitionId.Value = definitionId.ToString();

        if (null != Request.UrlReferrer)
            this.ReturnURL = Request.UrlReferrer.PathAndQuery;

        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PreviewForm();
        }
    }

    #region FormItem Behavior

    protected void btnAddItem_Click(object sender, EventArgs e)
    {
        SaveItemDefinition(Guid.Empty);
        PreviewForm();
    }

    protected void btnUpdateItem_Click(object sender, EventArgs e)
    {
        SaveItemDefinition(new Guid(ddlItems.SelectedValue));
        PreviewForm();
    }

    protected void btnDeleteItem_Click(object sender, EventArgs e)
    {
        if (ddlItems.SelectedIndex == 0) return;
        Guid itemId = new Guid(ddlItems.SelectedValue);
        FormItemDefinition item = new FormItemDefinition();
        item.Id = itemId;
        formDefinitionService.RemoveItemFromForm(item);
        getDataService.DeleteFormItemExtensionById(item.Id);

        PreviewForm();
    }

    //生成模板.
    protected void btnCreateTemplate_Click(object sender, EventArgs e)
    {

        FormDefinition definition = formDefinitionService.GetFormDefinitionById(FormDefinitionId, true);
        if (null == definition)
        {
            ShowError("未找到相应的表单定义数据.", ReturnURL);
        }

        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        
        renderStrategy.Render(htw, definition);

        StringWriter msw = new StringWriter();
        HtmlTextWriter mhtw = new HtmlTextWriter(msw);
        divRenderStrategy.Render(mhtw,definition);

        definition.TemplateContent = AppendScript(FormDefinitionId, sw.GetStringBuilder());
        definition.LastModifier = CurrentUserName;
        definition.LastModTime = DateTime.Now;

        definition.IsCurrentVersion = true;

        string WapTemplateContent = AppendMobileScript(FormDefinitionId, msw.GetStringBuilder());

        Guid returnId = formDefinitionService.SaveFormDefinition(definition);
        if (Guid.Empty != returnId)
        {
            dataListDefinitionService.WapTemplateContentUpdate(returnId,WapTemplateContent);
            
            FormDefinitionsInExternals external = new FormDefinitionsInExternals();
            external.FormDefinitionId = returnId;
            external.EntityType = EntityType;//修改
            external.EntityId = WorkflowId;
            formDefinitionService.AssociateFormDefinitionWithExternalEntity(external, false);

            IList<FormItemDefinition> itemList = definition.Items;
            IDictionary<string, string> idList = new Dictionary<string, string>();
            foreach (FormItemDefinition item in itemList)
            {
                string oldId = item.Id.ToString();
                item.Id = Guid.NewGuid();
                item.FormDefinitionId = returnId;
                idList.Add(oldId, item.Id.ToString());
            }
            formDefinitionService.AppendItemsToForm(itemList);

            //插入datalist设置
            IDictionary<string, Guid> datalistDict = new Dictionary<string, Guid>();
            IList<DataListSetting> settings = itemDataListSettingService.GetDataListSettingByFormDefinitionId(FormDefinitionId);
            foreach (DataListSetting setting in settings)
            {
                IList<DataListItemDefinition> itemDefinitions = dataListDefinitionService.GetDataListItemDefinitionsByFormItemDefinitionId(setting.FormItemDefinitionId);
                foreach (DataListItemDefinition itemDefinition in itemDefinitions)
                {
                    if (idList.ContainsKey(itemDefinition.FormItemDefinitionId.ToString()))
                        itemDefinition.FormItemDefinitionId = new Guid(idList[itemDefinition.FormItemDefinitionId.ToString()]);
                    itemDefinition.Id = Guid.NewGuid();
                }
                if (idList.ContainsKey(setting.FormItemDefinitionId.ToString()))
                    setting.FormItemDefinitionId = new Guid(idList[setting.FormItemDefinitionId.ToString()]);
                itemDataListSettingService.DataListSettingInsert(setting);
                dataListDefinitionService.AppendItemsToForm(itemDefinitions, datalistDict);
            }

            //插入扩展项设置
            IList<FormItemExtension> extensionlist = getDataService.GetFormItemExtensionSettingsByFormdefinitionId(FormDefinitionId);
            foreach (FormItemExtension item in extensionlist)
            {
                if (idList.ContainsKey(item.FormItemDefinitionId.ToString()))
                    item.FormItemDefinitionId = new Guid(idList[item.FormItemDefinitionId.ToString()]);
                //item.FormDefinitionId = returnId;
                getDataService.InserFormItemExtension(item);
            }

            //插入iframe设置
            IList<FormItemIFrames> iFrames = formItemIFramesService.SelectByFormDefinitionId(formDefinitionId);
            foreach (FormItemIFrames item in iFrames)
            {
                if (idList.ContainsKey(item.FormItemDefinitionId.ToString()))
                    item.FormItemDefinitionId = new Guid(idList[item.FormItemDefinitionId.ToString()]);
                //item.FormDefinitionId = returnId;
                formItemIFramesService.Create(item);
            }

            ShowSuccess("创建模板成功", String.Format(AppPath + "contrib/dynamicform/pages/ItemCreate.aspx?fdid={0}&wid={1}&EntityType={2}", returnId, WorkflowId, this.EntityType));
        }
        ShowError("创建模板失败!");
    }
    #endregion

    #region Methods

    //保存表单项.
    protected void SaveItemDefinition(Guid itemID)
    {
        Guid formDefinitionId = FormDefinitionId;
        string fName = txtFName.Text.Trim();
        if (Botwave.XQP.Domain.FormField.ExistField(formDefinitionId, fName, itemID))
            ShowError(string.Format("表单字段\"{0}\"已经存在，请重新填写字段名称。", fName));
        FormItemDefinition item = new FormItemDefinition();
        item.Id = itemID;
        item.FormDefinitionId = formDefinitionId;
        item.FName = fName;
        item.Name = txtName.Text.Trim();
        item.Comment = txtComment.Text.Trim();
        item.ItemDataType = (FormItemDefinition.DataType)DbUtils.ToInt32(rbItemDataType.SelectedValue);
        item.ItemType = (FormItemDefinition.FormItemType)DbUtils.ToInt32(rbItemType.SelectedValue);
        item.DataSource = txtDataSource.Text.Trim();
        item.DefaultValue = txtDefaultValue.Text.Trim();
        item.Left = DbUtils.ToInt32(txtLeft.Text.Trim());
        item.Top = DbUtils.ToInt32(txtTop.Text.Trim());
        item.Width = DbUtils.ToInt32(txtWidth.Text.Trim());
        item.Height = DbUtils.ToInt32(txtHeight.Text.Trim());
        item.RowExclusive = chkRowExclusive.Checked;
        item.Require = chkRequire.Checked;
        item.ValidateType = ddlValidateType.SelectedValue;
        item.MaxVal = txtMaxVal.Text.Trim();
        item.MinVal = txtMinVal.Text.Trim();
        item.Op = Request.Form[ddlOp.UniqueID];
        item.OpTarget = txtOpTarget.Text.Trim();
        item.ErrorMessage = txtErrorMessage.Text.Trim();

        //高级设置信息.
        string showSet = string.Empty;
        foreach (ListItem li in cblActivities4Show.Items)
        {
            if (li.Selected)
                showSet += li.Text + "|";
        }
        item.ShowSet = (showSet.Length > 0) ? showSet : null;
        string readSet = string.Empty;
        foreach (ListItem li in cblActivities4Read.Items)
        {
            if (li.Selected)
                readSet += li.Text + "|";
        }
        item.ReadonlySet = (readSet.Length > 0) ? readSet : null;

        if (itemID.Equals(Guid.Empty))
        {
            item.Id = Guid.NewGuid();
            formDefinitionService.AppendItemToForm(item);
        }
        else
        {
            formDefinitionService.UpdateItem(item);
        }
    }

    //预览表单.
    protected void PreviewForm()
    {
        BindItems();
        ResetForms();
        InitFormItems();
        BindActivitiesList();
    }

    //初始化表单项.
    private void InitFormItems()
    {
        FormDefinition definition = formDefinitionService.GetFormDefinitionById(FormDefinitionId, true);
        if (null == definition) return;

        string formName = definition.Name;
        if (formName.EndsWith("表单"))
            formName = formName.Substring(0, formName.Length - 2);
        if (!string.IsNullOrEmpty(formName))
            this.ltlTitle.Text = formName + " - ";

        StringWriter swDefinition = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(swDefinition);
        StringWriter mswDefinition = new StringWriter();
        HtmlTextWriter mhtw = new HtmlTextWriter(mswDefinition);

        definition.Items = ResortItemsByCreatedTime(definition.Items);

        try
        {
            TableRender.Render(htw, definition, true);

            DivRender.Render(mhtw, definition, true);
            
        }
        catch (FormItemRenderException ex)
        {
            Botwave.Commons.ExceptionLogger.Log(CurrentUserName);
            string message = ex.Message;
            if (ex.FormItem != null)
                message = string.Format("表单生成预览出错，出错字段：<br />字段名称：<b>{0}</b><br />字段意义：<b>{1}</b><br />请重新设置字段的{2}属性。", ex.FormItem.FName, ex.FormItem.Name, ex.IsUnknowException ? string.Empty : " <b>行列与高宽</b> ");

            lblPreview.Text = string.Format("<div style=\"color:red;font-size:15px\">{0}</div>", message);
        }
        if (null != swDefinition & !String.IsNullOrEmpty(swDefinition.GetStringBuilder().ToString()))
        {
            StringWriter sw = new StringWriter();
            formItemDataBinder.Bind(sw, Guid.Empty, StringUtils.HtmlDecode(swDefinition.GetStringBuilder()), null);
            lblPreview.Text = sw.GetStringBuilder().ToString();

            StringWriter msw = new StringWriter();
            formItemDataBinder.Bind(msw, Guid.Empty, StringUtils.HtmlDecode(mswDefinition.GetStringBuilder()), null);
            lblWapReview.Text = msw.GetStringBuilder().ToString();
            ltlIframe.Text = "<iframe id=\"Iframe\"  style=\"border: solid 1px #DFDFE8;\" src=\"preview.aspx\" frameborder=\"0\" width=\"360\" scrolling=\"auto\" height=\"640\"></iframe>";
        }
    }

    //重置表单项.
    protected void ResetForms()
    {
        txtFName.Text = String.Empty;
        txtName.Text = String.Empty;
        txtComment.Text = String.Empty;
        rbItemDataType.SelectedIndex = -1;
        rbItemType.SelectedIndex = -1;
        txtDataSource.Text = String.Empty;
        txtDefaultValue.Text = String.Empty;
        txtLeft.Text = String.Empty;
        txtTop.Text = String.Empty;
        txtWidth.Text = String.Empty;
        txtHeight.Text = String.Empty;
        chkRowExclusive.Checked = false;
        chkRequire.Checked = false;
        ddlValidateType.SelectedIndex = 0;
        txtMaxVal.Text = String.Empty;
        txtMinVal.Text = String.Empty;
        ddlOp.SelectedIndex = 0;
        txtOpTarget.Text = String.Empty;
        txtErrorMessage.Text = String.Empty;

        cblActivities4Read.SelectedIndex = -1;
        cblActivities4Show.SelectedIndex = -1;
    }

    #region bind

    //绑定表单项列表.
    protected void BindItems()
    {
        if (Guid.Empty == FormDefinitionId) return;

        ddlItems.DataSource = formDefinitionService.GetFormItemDefinitionsByFormDefinitionId(FormDefinitionId);
        ddlItems.DataBind();
        ddlItems.Items.Insert(0, new ListItem("", ""));
    }

    //绑定活动多选框
    protected void BindActivitiesList()
    {
        IList<ActivityDefinition> activityList = activityDefinitionService.GetActivitiesByWorkflowId(WorkflowId);

        cblActivities4Show.DataSource = activityList;
        cblActivities4Show.DataBind();
        //可编辑属性设置去掉活动步骤1
        IList<ActivityDefinition> activityList2 = new List<ActivityDefinition>();
        foreach (ActivityDefinition definition in activityList)
        {
            if (definition.State == 1)
                activityList2.Add(definition);
        }
        cblActivities4Read.DataSource = activityList2;
        cblActivities4Read.DataBind();

    }

    //附加高级设置/数据绑定脚本
    protected string AppendScript(Guid formDefinitionId, StringBuilder sbTemplate)
    {
        bool isShowSet = false;     //是否可见属性

        bool isReadonlySet = false;     //是否可编辑属性

        bool hasSelection = false;   //是否有数据选择
        StringBuilder sbReadonly = new StringBuilder();
        StringBuilder sbSelection = new StringBuilder();
        IList<FormItemDefinition> itemDefinitionList = formDefinitionService.GetFormItemDefinitionsByFormDefinitionId(formDefinitionId);
        foreach (FormItemDefinition item in itemDefinitionList)
        {
            string itemName = ControlCreator.CONTROL_PREFIX;
            switch (item.ItemType)
            {
                case FormItemDefinition.FormItemType.Text: itemName += FormItemDefinition.FormItemPrefix.Text; break;
                case FormItemDefinition.FormItemType.TextArea: itemName += FormItemDefinition.FormItemPrefix.TextArea; break;
                case FormItemDefinition.FormItemType.CheckBoxList: itemName += FormItemDefinition.FormItemPrefix.CheckBoxList; break;
                case FormItemDefinition.FormItemType.Date: itemName += FormItemDefinition.FormItemPrefix.Date; break;
                case FormItemDefinition.FormItemType.DropDownList: itemName += FormItemDefinition.FormItemPrefix.DropDownList; break;
                case FormItemDefinition.FormItemType.RadioButtonList: itemName += FormItemDefinition.FormItemPrefix.RadioButtonList; break;
                case FormItemDefinition.FormItemType.File: itemName += FormItemDefinition.FormItemPrefix.Text; break;
                default: break;
            }
            itemName += item.FName;

            if (!String.IsNullOrEmpty(item.ShowSet))
                isShowSet = true;
            if (!String.IsNullOrEmpty(item.ReadonlySet))
            {
                isReadonlySet = true;
                sbReadonly.AppendFormat("#set($data{0} = \"{1}\")\n", item.FName, item.ReadonlySet);
                sbReadonly.AppendFormat("#if($data{0}.IndexOf($!dc.get_Item(\"ActivityName\")) != -1)\n", item.FName);
                if (item.ItemType == FormItemDefinition.FormItemType.CheckBoxList || item.ItemType == FormItemDefinition.FormItemType.RadioButtonList)
                {
                    sbReadonly.AppendFormat(String.Format("$(\"input[name='{0}']\").each(function()", itemName));
                    sbReadonly.AppendLine("{");
                    sbReadonly.AppendLine(" $(this).attr(\"contentEditable\",\"true\");");
                    sbReadonly.AppendLine("});");
                }
                else
                {
                    sbReadonly.AppendFormat("$(\"#{0}\").attr(\"contentEditable\",\"true\");\n", itemName);
                }
                sbReadonly.Append("#end");
            }
            //数据选择绑定脚本
            if (item.ItemType == FormItemDefinition.FormItemType.DropDownList || item.ItemType == FormItemDefinition.FormItemType.CheckBoxList
                || item.ItemType == FormItemDefinition.FormItemType.RadioButtonList)
            {
                hasSelection = true;
                sbSelection.AppendFormat("pushSelection('{0}', '$!tc.GetValue(&quot;{1}&quot;)');\n", itemName, item.FName);
            }
        }

        if (isShowSet || isReadonlySet || hasSelection)
        {
            sbTemplate.AppendLine("<script language=\"javascript\">");
            if (isShowSet || isReadonlySet)
            {
                //sbTemplate.AppendLine("$(function(){");
                if (isShowSet)
                {
                    sbTemplate.AppendLine("var activityName = \"$!dc.get_Item(\"ActivityName\")\";");
                    sbTemplate.AppendLine("var rowCount = document.all.TB_FORM_DATA.rows.length;");
                    sbTemplate.AppendLine("for(var i=0; i< rowCount;i++){");
                    sbTemplate.AppendLine(" var trid = document.all.TB_FORM_DATA.rows[i].id;");
                    sbTemplate.AppendLine(" if(trid){");
                    sbTemplate.AppendLine("     if(trid.indexOf(activityName) != -1)");
                    sbTemplate.AppendLine("     {");
                    sbTemplate.AppendLine("         document.all.TB_FORM_DATA.rows[i].style.display = \"none\";");
                    sbTemplate.AppendLine("     }");
                    sbTemplate.AppendLine(" }");
                    sbTemplate.AppendLine("}");
                }
                if (isReadonlySet)
                {
                    sbTemplate.AppendLine("#if($!dc.get_Item(\"ActivityName\")){");
                    sbTemplate.AppendLine(sbReadonly.ToString());
                    sbTemplate.AppendLine("}");
                    sbTemplate.AppendLine("#end");
                }
                //sbTemplate.AppendLine("});");
            }

            if (hasSelection)
                sbTemplate.AppendLine(sbSelection.ToString());

            sbTemplate.Append("</script>");
        }

        return sbTemplate.ToString();
    }

    //附加高级设置/数据绑定脚本
    protected string AppendMobileScript(Guid formDefinitionId, StringBuilder sbTemplate)
    {
        bool isShowSet = false;     //是否可见属性

        bool isReadonlySet = false;     //是否可编辑属性

        bool hasSelection = false;   //是否有数据选择
        StringBuilder sbReadonly = new StringBuilder();
        StringBuilder sbSelection = new StringBuilder();
        IList<FormItemDefinition> itemDefinitionList = formDefinitionService.GetFormItemDefinitionsByFormDefinitionId(formDefinitionId);
        foreach (FormItemDefinition item in itemDefinitionList)
        {
            string itemName = ControlCreator.CONTROL_PREFIX;
            switch (item.ItemType)
            {
                case FormItemDefinition.FormItemType.Text: itemName += FormItemDefinition.FormItemPrefix.Text; break;
                case FormItemDefinition.FormItemType.TextArea: itemName += FormItemDefinition.FormItemPrefix.TextArea; break;
                case FormItemDefinition.FormItemType.CheckBoxList: itemName += FormItemDefinition.FormItemPrefix.CheckBoxList; break;
                case FormItemDefinition.FormItemType.Date: itemName += FormItemDefinition.FormItemPrefix.Date; break;
                case FormItemDefinition.FormItemType.DropDownList: itemName += FormItemDefinition.FormItemPrefix.DropDownList; break;
                case FormItemDefinition.FormItemType.RadioButtonList: itemName += FormItemDefinition.FormItemPrefix.RadioButtonList; break;
                case FormItemDefinition.FormItemType.File: itemName += FormItemDefinition.FormItemPrefix.Text; break;
                default: break;
            }
            itemName += item.FName;

            if (!String.IsNullOrEmpty(item.ShowSet))
                isShowSet = true;
            if (!String.IsNullOrEmpty(item.ReadonlySet))
            {
                isReadonlySet = true;
                sbReadonly.AppendFormat("#set($data{0} = \"{1}\")\n", item.FName, item.ReadonlySet);
                sbReadonly.AppendFormat("#if($data{0}.IndexOf($!dc.get_Item(\"ActivityName\")) != -1)\n", item.FName);
                if (item.ItemType == FormItemDefinition.FormItemType.CheckBoxList || item.ItemType == FormItemDefinition.FormItemType.RadioButtonList)
                {
                    sbReadonly.AppendFormat(String.Format("$(\"input[name='{0}']\").each(function()", itemName));
                    sbReadonly.AppendLine("{");
                    sbReadonly.AppendLine(" $(this).attr(\"contentEditable\",\"true\");");
                    sbReadonly.AppendLine("});");
                }
                else
                {
                    sbReadonly.AppendFormat("$(\"#{0}\").attr(\"contentEditable\",\"true\");\n", itemName);
                }
                sbReadonly.Append("#end");
            }
            //数据选择绑定脚本
            if (item.ItemType == FormItemDefinition.FormItemType.DropDownList || item.ItemType == FormItemDefinition.FormItemType.CheckBoxList
                || item.ItemType == FormItemDefinition.FormItemType.RadioButtonList)
            {
                hasSelection = true;
                sbSelection.AppendFormat("pushSelection('{0}', '$!tc.GetValue(&quot;{1}&quot;)');\n", itemName, item.FName);
            }
        }

        if (isShowSet || isReadonlySet || hasSelection)
        {
            sbTemplate.AppendLine("<script language=\"javascript\">");
            if (isShowSet || isReadonlySet)
            {
                //sbTemplate.AppendLine("$(function(){");
                if (isShowSet)
                {
                    sbTemplate.AppendLine("var activityName = \"$!dc.get_Item(\"ActivityName\")\";");
                    sbTemplate.AppendLine("var rowCount = $(\"#TB_FORM_DATA .row\").length");
                    sbTemplate.AppendLine("for(var i=0; i< rowCount;i++){");
                    sbTemplate.AppendLine(" var $row= $(\"#TB_FORM_DATA .row\")[i];");
                    sbTemplate.AppendLine(" var trid = $row.id;");
                    sbTemplate.AppendLine(" if(trid){");
                    sbTemplate.AppendLine("     if(trid.indexOf(activityName) != -1)");
                    sbTemplate.AppendLine("     {");
                    sbTemplate.AppendLine("         $row.style.display=\"none\";");
                    sbTemplate.AppendLine("     }");
                    sbTemplate.AppendLine(" }");
                    sbTemplate.AppendLine("}");
                }
                if (isReadonlySet)
                {
                    sbTemplate.AppendLine("#if($!dc.get_Item(\"ActivityName\")){");
                    sbTemplate.AppendLine(sbReadonly.ToString());
                    sbTemplate.AppendLine("}");
                    sbTemplate.AppendLine("#end");
                }
                //sbTemplate.AppendLine("});");
            }

            if (hasSelection)
                sbTemplate.AppendLine(sbSelection.ToString());

            sbTemplate.Append("</script>");
        }

        return sbTemplate.ToString();
    }

    /// <summary>
    /// 按创建时间对表单项重新排序.
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    protected static IList<FormItemDefinition> ResortItemsByCreatedTime(IList<FormItemDefinition> items)
    {
        if (items == null || items.Count <= 1)
            return items;
        int count = items.Count;
        for (int i = 0; i < count - 1; i++)
        {
            FormItemDefinition current = items[i];
            FormItemDefinition next = items[i + 1];
            if (current.CreatedTime > next.CreatedTime)
            {
                FormItemDefinition definition = current;
                items[i] = next;
                items[i + 1] = definition;
            }
        }
        return items;
    }

    #endregion

    #endregion
}
