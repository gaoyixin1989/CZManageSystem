using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Web;
using Botwave.DynamicForm;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;
using Botwave.Workflow.Extension.Util;
using Botwave.DynamicForm.Extension.Domain;
using Botwave.DynamicForm.Extension.Implements;
using Botwave.Workflow.Service;
using Botwave.DynamicForm.Renders;
using Botwave.DynamicForm.Extension.Renders;
using Botwave.DynamicForm.Binders;
using System.Data.SqlClient;
using Botwave.Extension.IBatisNet;

public partial class contrib_dynamicform_pages_Create : Botwave.Security.Web.PageBase
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
    public IDataFormLibraryService dataFormLibraryService = (IDataFormLibraryService)Ctx.GetObject("dataFormLibraryService");

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

    public IDataFormLibraryService DataFormLibraryService
    {
        get { return dataFormLibraryService; }
        set { dataFormLibraryService = value; }
    }
    public string ReturnURL = string.Empty;
    #endregion

    public Guid EntityId
    {
        get { return ViewState["EntityId"]==null?Guid.Empty:(Guid)(ViewState["EntityId"]); }
        set { ViewState["EntityId"] = value; }
    }
    public string EntityType
    {
        get { return (string)(ViewState["EntityType"]); }
        set { ViewState["EntityType"] = value; }
    }


    public Guid FormDefinitionId
    {
        get { return ViewState["FormDefinitionId"]==null? Guid.Empty:(Guid)(ViewState["FormDefinitionId"]); }
        set { ViewState["FormDefinitionId"] = value; }
    }
    protected override void OnInit(EventArgs e)
    {
        string wid = Request.QueryString["wid"];
        string entityType = Request.QueryString["EntityType"];
        //if (String.IsNullOrEmpty(wid) || String.IsNullOrEmpty(entityType))
        //{
        //    ShowError(MessageHelper.Message_ArgumentException);
        //}
        //EntityId = new Guid(wid);
        EntityType = entityType;
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string fdid = Request.QueryString["fdid"];
            string wid = Request.QueryString["wid"];
            Guid definitionId = Guid.Empty;
            if (!string.IsNullOrEmpty(fdid))
            {
                FormDefinition temp = formDefinitionService.GetFormDefinitionById(new Guid(fdid),true);
                if (null != temp)
                {
                    definitionId = temp.Id;
                    txtName.Text = temp.Name;
                    txtComment.Text = temp.Comment;
                    FormDefinitionId = definitionId;
                }
            }
        }
    }

    protected void btnSaveFormDefinition_Click(object sender, EventArgs e)
    {
        string userName = CurrentUserName;

        FormDefinition definition = formDefinitionService.GetFormDefinitionById(FormDefinitionId, true);
        if (null == definition)
        {
            definition = new FormDefinition();
        }
        definition.Creator = userName;
        definition.LastModifier = userName;
        definition.Name = txtName.Text.Trim();
        definition.Comment = txtComment.Text.Trim();
        definition.Version = 1;
        
        definition.LastModTime = DateTime.Now;

        definition.IsCurrentVersion = true;

        Guid returnId = dataFormLibraryService.InsertFormDefinition(definition);
        

        if (Guid.Empty != returnId)
        {
            FormDefinitionsInExternals external = new FormDefinitionsInExternals();
            external.FormDefinitionId = returnId;
            external.EntityType = "Form_Template";//修改
            external.EntityId = Guid.Empty;
            formDefinitionService.AssociateFormDefinitionWithExternalEntity(external, true);

            SqlParameter[] pa = new SqlParameter[2];
            pa[0] = new SqlParameter("@formDefinitionId", SqlDbType.UniqueIdentifier);
            pa[0].Value = FormDefinitionId;
            pa[1] = new SqlParameter("@returnId", SqlDbType.UniqueIdentifier);
            pa[1].Value = returnId;
            object result = IBatisDbHelper.ExecuteScalar(CommandType.Text, "update bwdf_FormDefinitions set WapTemplateContent=(select WapTemplateContent from bwdf_FormDefinitions where id=@formDefinitionId) where id=@returnId ", pa);

            IList<FormItemDefinition> itemList = definition.Items == null ? new List<FormItemDefinition>() : definition.Items;
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
            IList<FormItemIFrames> iFrames = formItemIFramesService.SelectByFormDefinitionId(FormDefinitionId);
            foreach (FormItemIFrames item in iFrames)
            {
                if (idList.ContainsKey(item.FormItemDefinitionId.ToString()))
                    item.FormItemDefinitionId = new Guid(idList[item.FormItemDefinitionId.ToString()]);
                //item.FormDefinitionId = returnId;
                formItemIFramesService.Create(item);
            }

            ShowSuccess("加入表单库成功", String.Format(AppPath + "contrib/dynamicform/pages/FormIndex.aspx?EntityType={0}", this.EntityType));
        }
        ShowError("创建模板失败!");
        Response.Redirect("formIndex.aspx?wid=" + EntityId);
    }    
}
