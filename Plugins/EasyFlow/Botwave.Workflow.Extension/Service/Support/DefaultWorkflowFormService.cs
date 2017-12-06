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

namespace Botwave.Workflow.Extension.Service.Support
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
            FormContext context = new FormContext(request.Form, request.Files);
            return context.Variables;
        }

        /// <summary>
        /// 创建表单实例.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="workflowInstanceId"></param>
        /// <param name="creator"></param>
        public void CreateFormInstance(Guid workflowId, Guid workflowInstanceId, string creator)
        {
            //创建表单实例
            FormDefinition definition = formDefinitionService.GetFormDefinitionByExternalEntity(WorkflowUtility.EntityType_WorkflowForm, workflowId);
            if (definition != null)
            {
                formInstanceService.CreateFormInstance(workflowInstanceId, definition.Id, creator);
            }
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
            //string content = formOptimizeService.GetFormContent(workflowInstanceId);
            //if (string.IsNullOrEmpty(content))
            //{
            //    return BindForm(workflowId, workflowInstanceId, binderVariables);
            //}
            //return content;
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
            formInstanceService.SaveForm(workflowInstanceId, dataVariables, actor);
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
            this.SaveForm(workflowInstanceId, actor, dataVariables);

            // 当工单完成或者取消时，将表单以字符串形式保存.
            //int state = GetWorkflowInstanceState(workflowInstanceId);
            //if (state == 2 || state == 99)
            //{
            //    if (formOptimizeService != null)
            //    {
            //        string content = this.BindForm(workflowId, workflowInstanceId, binderVariables);
            //        formOptimizeService.SaveFormContent(workflowInstanceId, content);
            //    }
            //}
        }

        /// <summary>
        /// 部署表单数据.
        /// </summary>
        /// <param name="oldWorkflow"></param>
        /// <param name="newWorkflow"></param>
        public void DeployForm(WorkflowDefinition oldWorkflow, WorkflowDefinition newWorkflow)
        {
            //流程配置完毕后续处理：关联表单与流程定义
            FormDefinitionsInExternals external = new FormDefinitionsInExternals();
            external.EntityType = "Form_Workflow";
            external.EntityId = newWorkflow.WorkflowId;

            FormDefinition formDefinition;
            if (oldWorkflow == null)
            {
                //新增流程时，创建表单定义及关联表单流程.
                string creator = newWorkflow.Creator;
                formDefinition = new FormDefinition();
                formDefinition.Name = newWorkflow.WorkflowName + "表单";
                formDefinition.Creator = creator;
                formDefinition.LastModifier = creator;
                formDefinition.Version = 1;
                Guid definitionId = formDefinitionService.SaveFormDefinition(formDefinition);

                if (Guid.Empty != definitionId)
                {
                    external.FormDefinitionId = definitionId;
                    formDefinitionService.AssociateFormDefinitionWithExternalEntity(external, true);
                }
            }
            else
            {
                //修改流程时，新增一条与已有表单定义关联的表单流程关联.
                formDefinition = formDefinitionService.GetFormDefinitionByExternalEntity(external.EntityType, oldWorkflow.WorkflowId);
                external.FormDefinitionId = formDefinition.Id;
                formDefinitionService.AssociateFormDefinitionWithExternalEntity(external, true);
            }
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
                return BindForm(formItemDataBinder, workflowInstanceId, definition.TemplateContent, binderVariables);
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

        /// <summary>
        /// 获取指定流程实例的执行状态.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        protected static int GetWorkflowInstanceState(Guid workflowInstanceId)
        {
            return IBatisMapper.Mapper.QueryForObject<int>("bwwf_WorkflowInstances_Select_State_ByWorkflowInstanceId", workflowInstanceId);
        }

        #endregion
    }
}
