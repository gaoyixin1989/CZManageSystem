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

namespace Botwave.XQP.Service.Plugins
{
    public class WorkflowFormService : IWorkflowFormService
    {
        #region service properties

        private IFormDefinitionService formDefinitionService;
        private IFormInstanceService formInstanceService;
        private IFormItemDataBinder formItemDataBinder;

        public IFormDefinitionService FormDefinitionService
        {
            get { return formDefinitionService; }
            set { formDefinitionService = value; }
        }

        public IFormInstanceService FormInstanceService
        {
            get { return formInstanceService; }
            set { formInstanceService = value; }
        }

        public IFormItemDataBinder FormItemDataBinder
        {
            get { return formItemDataBinder; }
            set { formItemDataBinder = value; }
        }

        #endregion

        #region IWorkflowFormService 成员

        public IDictionary<string, object> GetFormVariables(HttpRequest request)
        {
            FormContext context = new FormContext(request.Form, request.Files);
            return context.Variables;
        }

        public void CreateFormInstance(Guid workflowId, Guid workflowInstanceId, string creator)
        {
            //创建表单实例
            FormDefinition definition = formDefinitionService.GetFormDefinitionByExternalEntity(WorkflowUtility.EntityType_WorkflowForm, workflowId);
            if (definition != null)
            {
                formInstanceService.CreateFormInstance(workflowInstanceId, definition.Id, creator);
            }
        }

        public string LoadForm(Guid workflowId, Guid workflowInstanceId, IDictionary<string, object> binderVariables)
        {
            return BindForm(workflowId, workflowInstanceId, binderVariables);
        }

        public void SaveForm(Guid workflowInstanceId, string actor, IDictionary<string, object> dataVariables)
        {
            formInstanceService.SaveForm(workflowInstanceId, dataVariables, actor);
        }

        public void SaveForm(Guid workflowId, Guid workflowInstanceId, string actor, IDictionary<string, object> dataVariables, IDictionary<string, object> binderVariables)
        {
            this.SaveForm(workflowInstanceId, actor, dataVariables);
        }

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

        protected string BindForm(Guid workflowId, Guid workflowInstanceId, IDictionary<string, object> binderVariables)
        {
            // FormDefinition definition = formDefinitionService.GetFormDefinitionByExternalEntity("Form_Workflow", workflowId);
            FormDefinition definition = formDefinitionService.GetFormDefinitionByFormInstanceId(workflowInstanceId);
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
