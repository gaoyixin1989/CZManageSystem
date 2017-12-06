using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Botwave.DynamicForm.Binders;
using Botwave.DynamicForm.Services;
using Botwave.DynamicForm.IBatisNet;
using Botwave.DynamicForm.Domain;
using System.Collections.Generic;
using Botwave.Commons;
using System.Collections.Specialized;

/// <summary>
///主要提供动态表单的操作
/// </summary>

public class DynamicFormHelper
{
    private static IFormItemDataBinder formItemDataBinder;
    private static IFormDefinitionService formDefinitionService;
    private static IFormInstanceService formInstanceService;

    public static IFormItemDataBinder FormItemDataBinder
    {
        set { formItemDataBinder = value; }
    }
    public  IFormDefinitionService FormDefinitionService
    {
        set { formDefinitionService = value; }
    }
    public  IFormInstanceService FormInstanceService
    {        
        set { formInstanceService = value; }
    }
    static DynamicFormHelper()
    {
        formItemDataBinder = Spring.Context.Support.WebApplicationContext.Current["formItemDataBinder"] as IFormItemDataBinder;
        formDefinitionService = Spring.Context.Support.WebApplicationContext.Current["formDefinitionService"] as IFormDefinitionService;
        formInstanceService = Spring.Context.Support.WebApplicationContext.Current["formInstanceService"] as IFormInstanceService;

    }
    
    /// <summary>
    /// 绑定表单
    /// </summary>
    /// <param name="EntityId">实体的ID</param>
    /// <param name="formInstanceId">表单实例Id</param>
    /// <returns>表单内容字符串</returns>
    public static string LoadFrom(Guid EntityId, Guid formInstanceId)
    {

        FormDefinition definition = formDefinitionService.GetFormDefinitionByExternalEntity("Form_IIS", EntityId);
        if (null != definition && !String.IsNullOrEmpty(definition.TemplateContent))
        {
            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                formItemDataBinder.Bind(sw, formInstanceId, StringUtils.HtmlDecode(definition.TemplateContent), null);
                return sw.GetStringBuilder().ToString();
            }
        }
        return string.Empty;
    }

    /// <summary>
    /// 创建表单实例
    /// </summary>
    /// <param name="EntityId">实体ID</param>
    /// <param name="formInstanceId">表单实例ID</param>
    /// <param name="creator">创建人的UserName</param>
    public static void CreateFormInstance(Guid EntityId, Guid formInstanceId, string creator)
    {
        //创建表单实例
        FormDefinition definition = formDefinitionService.GetFormDefinitionByExternalEntity("Form_IIS", EntityId);
        if (definition != null)
        {
            formInstanceService.CreateFormInstance(formInstanceId, definition.Id, creator);
        }
    }
    /// <summary>
    /// 更新表单内容
    /// </summary>
    /// <param name="formInstanceId">表单实例ID</param>
    /// <param name="creator">修改人UserName</param>
    public static void UpdateFormInstance(Guid formInstanceId, String creator)
    {
        formInstanceService.UpdateFormInstance(formInstanceId,creator);
    }
    /// <summary>
    /// 辅助方法获获取键值对的字典
    /// </summary>
    /// <param name="formVariables">键值对的集合</param>
    /// <returns>键值对字典</returns>
    public static IDictionary<string, object> GetFormVariables(NameValueCollection formVariables,HttpRequest request)
    {
        Botwave.DynamicForm.FormContext context = new Botwave.DynamicForm.FormContext(formVariables, request.Files);
        return context.Variables;
    }
    /// <summary>
    /// 保存表单的内容
    /// </summary>
    /// <param name="formInstanceId">表单实例ID</param>
    /// <param name="actor">当前操作人的UserName</param>
    /// <param name="dataVariables">键值对字典</param>
    public static void SaveForm(Guid formInstanceId, string actor, IDictionary<string, object> dataVariables)
    {
        formInstanceService.SaveForm(formInstanceId, dataVariables, actor);
    }    
}

