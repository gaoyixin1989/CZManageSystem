using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Transactions;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.DynamicForm;
using Botwave.DynamicForm.Services;
using Botwave.DynamicForm.Domain;

namespace Botwave.XQP.Util
{
    /// <summary>
    /// 流程事务类.
    /// </summary>
    public class WorkflowTransactionHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(WorkflowTransactionHelper));

        private static object syncLocker = new object();

        #region interfaces

        private static IWorkflowEngine workflowEngine = Spring.Context.Support.WebApplicationContext.Current["workflowEngine"] as IWorkflowEngine;
        private static IWorkflowService workflowService = Spring.Context.Support.WebApplicationContext.Current["workflowService"] as IWorkflowService;
        private static IFormDefinitionService formDefinitionService = Spring.Context.Support.WebApplicationContext.Current["formDefinitionService"] as IFormDefinitionService;
        private static IFormInstanceService formInstanceService = Spring.Context.Support.WebApplicationContext.Current["formInstanceService"] as IFormInstanceService;

        public static IWorkflowEngine WorkflowEngine
        {
            get { return workflowEngine; }
            set { workflowEngine = value; }
        }

        public static IWorkflowService WorkflowService
        {
            get { return workflowService; }
            set { workflowService = value; }
        }

        public static IFormDefinitionService FormDefinitionService
        {
            get { return formDefinitionService; }
            set { formDefinitionService = value; }
        }

        public static IFormInstanceService FormInstanceService
        {
            get { return WorkflowTransactionHelper.formInstanceService; }
            set { WorkflowTransactionHelper.formInstanceService = value; }
        }

        #endregion

        /// <summary>
        /// 事务启动流程.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="formContext"></param>
        /// <param name="executionContext"></param>
        /// <param name="actor"></param>
        /// <param name="isUpdate"></param>
        /// <returns></returns>
        public static Guid TransactionStartWorkflow(WorkflowInstance instance, FormContext formContext, ActivityExecutionContext executionContext, string actor, bool isUpdate)
        {
            Guid activityInstanceId = Guid.Empty;
            using (TransactionScope scope = new TransactionScope())
            {
                Guid workflowId = instance.WorkflowId;
                Guid workflowInstanceId = instance.WorkflowInstanceId;
                log.Warn("worklfowId:" + workflowId + "  # workflowInstanceId:" + workflowInstanceId);
                try
                {
                    // 插入或更新流程实例数据.
                    if (isUpdate)
                    {
                        workflowService.UpdateWorkflowInstanceForStart(instance);// 更新实例
                    }
                    else
                    {
                        workflowService.InsertWorkflowInstance(instance); // 创建实例
                    }

                    // 启动流程
                    activityInstanceId = workflowEngine.StartWorkflow(instance);
                    log.Warn("ActivityInstanceId:" + activityInstanceId);
                    if (activityInstanceId == Guid.Empty)
                        throw new WorkflowException("流程初始步骤实例为 Empty.");
                    // 插入或更新流程表单数据
                    if (!isUpdate)
                    {
                        //创建表单实例
                        FormDefinition definition = formDefinitionService.GetFormDefinitionByExternalEntity("Form_Workflow", workflowId);
                        formInstanceService.CreateFormInstance(workflowInstanceId, definition.Id, actor);
                    }
                    //Guid userId = Botwave.Security.AuthenticateHelper.User.UserId;
                    UpdateAttachmentEntities(workflowId, workflowInstanceId);

                    formInstanceService.SaveForm(workflowInstanceId, formContext.Variables, actor);

                    // 执行下一步骤
                    executionContext.ActivityInstanceId = activityInstanceId;
                    workflowEngine.ExecuteActivity(executionContext);
                    
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                scope.Complete();
            }
            return activityInstanceId;
        }

        /// <summary>
        /// 启动流程.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="formContext"></param>
        /// <param name="executionContext"></param>
        /// <param name="actor"></param>
        /// <param name="isUpdate"></param>
        /// <returns></returns>
        public static Guid StartWorkflow(WorkflowInstance instance, Botwave.XQP.Util.FormContext formContext, ActivityExecutionContext executionContext, Botwave.Security.LoginUser actor, bool isUpdate)
        {
            Guid activityInstanceId = Guid.Empty;

            lock (syncLocker)
            {
                Guid workflowId = instance.WorkflowId;
                Guid workflowInstanceId = instance.WorkflowInstanceId;
                //if (log.IsInfoEnabled)
                //{
                //    log.Info("worklfowId:" + workflowId + "  # workflowInstanceId:" + workflowInstanceId);
                //}
                
                //try
                //{
                    // 插入或更新流程实例数据.
                    if (isUpdate)
                    {
                        workflowService.UpdateWorkflowInstanceForStart(instance);// 更新实例
                    }
                    else
                    {
                        workflowService.InsertWorkflowInstance(instance); // 创建实例
                    }

                    // 启动流程
                    activityInstanceId = workflowEngine.StartWorkflow(instance);
                    //if (log.IsInfoEnabled)
                    //{
                    //    log.Info("ActivityInstanceId:" + activityInstanceId);
                    //}
                    if (activityInstanceId == Guid.Empty)
                        throw new WorkflowException("流程初始步骤实例为 Empty.");
                    // 插入或更新流程表单数据
                    if (!isUpdate)
                    {
                        //创建表单实例
                        FormDefinition definition = formDefinitionService.GetFormDefinitionByExternalEntity("Form_Workflow", workflowId);
                        formInstanceService.CreateFormInstance(workflowInstanceId, definition.Id, actor.UserName);

                        UpdateAttachmentEntities(actor.UserId, workflowInstanceId);
                    }
                    //Botwave.DynamicForm.FormContext fc = new Botwave.DynamicForm.FormContext();
                    formInstanceService.SaveForm(workflowInstanceId, formContext.Variables, actor.UserName);

                    // 执行下一步骤
                    executionContext.ActivityInstanceId = activityInstanceId;
                    workflowEngine.ExecuteActivity(executionContext);

                //}
                //catch (Exception ex)
                //{
                //    log.Error(ex);
                //}
            }
            return activityInstanceId;
        }


        /// <summary>
        /// 更新附件信息.  2011-04-20 Dain
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="workflowInstanceId"></param>
        public static void UpdateAttachmentEntities(Guid workflowId, Guid workflowInstanceId)
        {
            IList<Botwave.FileManagement.Attachment> listAtt = Botwave.FileManagement.AttachmentEntity.Select(workflowId);
            foreach (Botwave.FileManagement.Attachment entity in listAtt)
            {
                try
                {
                    Botwave.FileManagement.AttachmentEntity.Update(entity.Id, workflowInstanceId);
                    log.InfoFormat("成功更新附件关系,附件ID:{0},流程实例ID:{1},临时ID:{2}", entity.Id, workflowInstanceId, workflowId);
                }
                catch (Exception ex)
                {
                    string sql = string.Format(@"INSERT INTO [xqp_Attachment_Entity_UpdateFail]
           ([AttachmentTempEntityId] ,[WorkflowInstanceId]
           ,[AttachmentCreator]
           ,[AttachmentId]
           ,[AttachmentTitle]
           ,[AttachmentCreatedTime],ExceptionContent)
     VALUES
           ('{0}' ,'{1}','{2}' ,'{3}','{4}','{5}','{6}')", workflowId, workflowInstanceId, entity.Creator, entity.Id, entity.Title, entity.CreatedTime, ex.ToString());
                    IBatisDbHelper.ExecuteNonQuery(CommandType.Text, sql);
                }

            }
        }
    }
}
