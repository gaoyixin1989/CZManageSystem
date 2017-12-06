using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.DynamicForm.Extension.Implements;
using Botwave.DynamicForm.Extension.Domain;
using System.Text;
using Botwave.DynamicForm.Services;
using Botwave.DynamicForm.Domain;

public partial class apps_xqp2_pages_workflows_report_DataListInfoDetail : Botwave.Security.Web.PageBase
{
    #region Interfaces
    private IItemDataListSettingService itemDataListSettingService = (IItemDataListSettingService)Ctx.GetObject("itemDataListSettingService");
    private IDataListDefinitionService dataListDefinitionService = (IDataListDefinitionService)Ctx.GetObject("dataListDefinitionService");
    private IDataListInstanceService dataListInstanceService = (IDataListInstanceService)Ctx.GetObject("dataListInstanceService");
    private IFormDefinitionService formDefinitionService = (IFormDefinitionService)Ctx.GetObject("formDefinitionService");

    public IItemDataListSettingService ItemDataListSettingService
    {
        set { this.itemDataListSettingService = value; }
    }

    public IDataListDefinitionService DataListDefinitionService
    {
        set { this.dataListDefinitionService = value; }
    }

    public IDataListInstanceService DataListInstanceService
    {
        set { this.dataListInstanceService = value; }
    }

    public IFormDefinitionService FormDefinitionService
    {
        get { return formDefinitionService; }
        set { formDefinitionService = value; }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string wiid=Request.QueryString["wiid"];
            string fdid=Request.QueryString["fdid"];
            this.litTable.Text = GetData(wiid, fdid);
        }
    }
    private string GetData(string workflowinstanceid, string formItemDefinitionId)
    {
        DataListSetting setting = itemDataListSettingService.GetDataListSetting(new Guid(formItemDefinitionId));
        FormItemDefinition item = formDefinitionService.GetFormItemDefinitionById(new Guid(formItemDefinitionId));
        IList<DataListItemDefinition> itemDefinitions = dataListDefinitionService.GetDataListItemDefinitionsByFormItemDefinitionId(new Guid(formItemDefinitionId));
        IList<DataListItemInstance> itemInstances = dataListInstanceService.GetDataListItemInstancesByFormInstanceIdAndFormItemDefinitionId(new Guid(workflowinstanceid), new Guid(formItemDefinitionId), true);
        IDictionary<int, DataListItemDefinition> dict = new Dictionary<int, DataListItemDefinition>();
        IDictionary<string, string> itemInstanceDict = new Dictionary<string, string>();
        StringBuilder tableStr = new StringBuilder("<table id=\"TB_FORM_DATA_LIST_" + item.FName + "\" class=\"tblGrayClass grayBackTable\" style=\"word-break: break-all; text-align:center\" cellspacing=\"1\" cellpadding=\"4\" border=\"0\"><tbody>");
        Page.Title = "[" + item.FName + "]" + item.Name + " 的详细信息";
        foreach (DataListItemDefinition itemDefinition in itemDefinitions)
        {
            if (itemDefinition.ColumnNumber < setting.Columns)
                dict.Add(itemDefinition.ColumnNumber, itemDefinition);
            
        }
        if (null != itemInstances && itemInstances.Count > 0)
        {
            foreach (DataListItemInstance itemInstance in itemInstances)
            {
                string val = String.IsNullOrEmpty(itemInstance.Value) ? "" : itemInstance.Value;
                if (!itemInstanceDict.ContainsKey(itemInstance.Definition.FName + "_r" + itemInstance.RowNumber.ToString()))
                    itemInstanceDict.Add(itemInstance.Definition.FName + "_r" + itemInstance.RowNumber.ToString(), val);
            }
        }
        tableStr.AppendLine("<tr>");
        for (int j = 0; j < setting.Columns; j++)
        {
            if (dict.ContainsKey(j))
            {
                tableStr.AppendLine("<th style=\"width:150px\"><b>" + dict[j].Name + "</b></th>");
            }
        }
        tableStr.AppendLine("<tr>");
        for (int i = 0; i < setting.Rows; i++)
        {
            tableStr.AppendLine("<tr>");
            for (int j = 0; j < setting.Columns; j++)
            {
                if (dict.ContainsKey(j))
                {
                    switch (dict[j].ItemType.ToString())
                    {
                        case "Text":
                            string val = !itemInstanceDict.ContainsKey(dict[j].FName + "_r" + i.ToString()) ? "" : itemInstanceDict[dict[j].FName + "_r" + i.ToString()];
                            tableStr.AppendLine("<td>" + val + "</td>");
                            break;
                        case "TextArea":
                            break;
                        case "Date":
                            break;
                        case "CheckBoxList":
                            break;
                        case "DropDownList":
                            break;
                        case "RadioButtonList":
                            break;
                    }
                }
            }
            tableStr.AppendLine("</tr>");
        }
        tableStr.AppendLine("</tbody></table>");
        return tableStr.ToString();
    }
}
