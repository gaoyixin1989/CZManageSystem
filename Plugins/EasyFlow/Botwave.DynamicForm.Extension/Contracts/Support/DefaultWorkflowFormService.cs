using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Extension.Util;
using Botwave.DynamicForm;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;
using Botwave.DynamicForm.Binders;
using Botwave.Workflow.Extension.Service;
using System.Data.SqlClient;

namespace Botwave.DynamicForm.Extension.Contracts.Support
{
    /// <summary>
    /// 流程表单服务的默认实现类.
    /// </summary>
    public class DefaultWorkflowFormService : IWorkflowFormService
    {
        #region service properties

        private IFormDefinitionService formDefinitionService;
        private IFormInstanceService formInstanceService;
        private IFormItemDataBinder formItemDataBinder;
        //private IFormOptimizeService formOptimizeService;

        /// <summary>
        /// 表单定义服务.
        /// </summary>
        public IFormDefinitionService FormDefinitionService
        {
            get { return formDefinitionService; }
            set { formDefinitionService = value; }
        }

        /// <summary>
        /// 表单实例服务.
        /// </summary>
        public IFormInstanceService FormInstanceService
        {
            get { return formInstanceService; }
            set { formInstanceService = value; }
        }

        /// <summary>
        /// 表单项数据绑定服务.
        /// </summary>
        public IFormItemDataBinder FormItemDataBinder
        {
            get { return formItemDataBinder; }
            set { formItemDataBinder = value; }
        }

        //public IFormOptimizeService FormOptimizeService
        //{
        //    get { return formOptimizeService; }
        //    set { formOptimizeService = value; }
        //}

        #endregion

        #region IWorkflowFormService 成员

        /// <summary>
        /// 获取请求的表单变量字典.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IDictionary<string, object> GetFormVariables(HttpRequest request)
        {
            return new Dictionary<string, object>();
        }

        /// <summary>
        /// 创建表单实例.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="workflowInstanceId"></param>
        /// <param name="creator"></param>
        public void CreateFormInstance(Guid workflowId, Guid workflowInstanceId, string creator)
        {

        }

        /// <summary>
        /// 加载表单数据.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="workflowInstanceId"></param>
        /// <param name="binderVariables"></param>
        /// <returns></returns>
        public string LoadForm(Guid workflowId, Guid workflowInstanceId, IDictionary<string, object> binderVariables)
        {
            return BindForm(workflowId, workflowInstanceId, binderVariables);
        }

        /// <summary>
        /// 保存表单数据.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <param name="actor"></param>
        /// <param name="dataVariables"></param>
        public void SaveForm(Guid workflowInstanceId, string actor, IDictionary<string, object> dataVariables)
        {

        }

        /// <summary>
        /// 保存表单数据.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="workflowInstanceId"></param>
        /// <param name="actor"></param>
        /// <param name="dataVariables"></param>
        /// <param name="binderVariables"></param>
        public void SaveForm(Guid workflowId, Guid workflowInstanceId, string actor, IDictionary<string, object> dataVariables, IDictionary<string, object> binderVariables)
        {

        }

        /// <summary>
        /// 部署表单数据.
        /// </summary>
        /// <param name="oldWorkflow"></param>
        /// <param name="newWorkflow"></param>
        public void DeployForm(WorkflowDefinition oldWorkflow, WorkflowDefinition newWorkflow)
        {
            
        }

        #endregion

        #region methods

        /// <summary>
        /// 绑定表单数据，并返回 html 字符串的结果.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="workflowInstanceId"></param>
        /// <param name="binderVariables"></param>
        /// <returns></returns>
        protected string BindForm(Guid workflowId, Guid workflowInstanceId, IDictionary<string, object> binderVariables)
        {
            // FormDefinition definition = formDefinitionService.GetFormDefinitionByExternalEntity("Form_Workflow", workflowId);
            FormDefinition definition = formDefinitionService.GetFormDefinitionByFormInstanceId(workflowInstanceId);
            if (definition == null)
                definition = formDefinitionService.GetFormDefinitionByExternalEntity("Form_Workflow", workflowId);

            if (null != definition && !String.IsNullOrEmpty(definition.TemplateContent))
            {
                SqlParameter[] pa = new SqlParameter[1];
                pa[0] = new SqlParameter("@formDefinitionId", SqlDbType.UniqueIdentifier);
                pa[0].Value = definition.Id;
                object result = IBatisDbHelper.ExecuteScalar(CommandType.Text, "select WapTemplateContent from bwdf_FormDefinitions where id=@formDefinitionId ", pa);
                string WapTemplateContent = Botwave.Commons.DbUtils.ToString(result);
                if (!String.IsNullOrEmpty(WapTemplateContent))
                    return BindForm(formItemDataBinder, workflowInstanceId, WapTemplateContent, binderVariables);
            }
            return string.Empty;
        }

        /// <summary>
        /// 绑定表单数据.
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="workflowInstanceId"></param>
        /// <param name="templateContent"></param>
        /// <param name="binderVariables"></param>
        /// <returns></returns>
        protected static string BindForm(IFormItemDataBinder binder, Guid workflowInstanceId, string templateContent, IDictionary<string, object> binderVariables)
        {
            if (string.IsNullOrEmpty(templateContent))
                return string.Empty;

            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                binder.Bind(sw, workflowInstanceId, StringUtils.HtmlDecode(templateContent), binderVariables);
                return sw.GetStringBuilder().ToString();
            }
        }

        #endregion
    }
}
