using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.EnterpriseServices;
using System.Transactions;
using System.Text;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Service;

namespace Botwave.Workflow.Extension.Util
{
    /// <summary>
    /// 流程数据处理辅助类.
    /// </summary>
    public class WorkflowDataHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(WorkflowDataHelper));
        private static object syncLocker = new object();

        #region service interfaces

        private static IWorkflowEngine workflowEngine;
        private static IWorkflowService workflowService;
        private static IWorkflowFormService workflowFormService;
        private static IWorkflowAttachmentService workflowAttachmentService;

        /// <summary>
        /// 流程引擎.
        /// </summary>
        public static IWorkflowEngine WorkflowEngine
        {
            get { return workflowEngine; }
            set { workflowEngine = value; }
        }

        /// <summary>
        /// 流程实例服务.
        /// </summary>
        public static IWorkflowService WorkflowService
        {
            get { return workflowService; }
            set { workflowService = value; }
        }

        /// <summary>
        /// 流程表单服务.
        /// </summary>
        public static IWorkflowFormService WorkflowFormService
        {
            get { return workflowFormService; }
            set { workflowFormService = value; }
        }

        /// <summary>
        /// 流程附件服务.
        /// </summary>
        public static IWorkflowAttachmentService WorkflowAttachmentService
        {
            get { return workflowAttachmentService; }
            set { workflowAttachmentService = value; }
        }
        #endregion

        #region start workflow

        /// <summary>
        /// 使用事务的方式启动流程.
        /// </summary>
        /// <param name="instance">流程实例信息.</param>
        /// <param name="formVariables">流程表单参数字典.</param>
        /// <param name="executionContext">流程步骤执行上下文.</param>
        /// <param name="actor">当前处理人.</param>
        /// <param name="isUpdate">是否属于更新流程信息.</param>
        /// <returns>返回流程初始步骤实例 ID.</returns>
        public static Guid StartWorkflowByTransaction(WorkflowInstance instance, IDictionary<string, object> formVariables, ActivityExecutionContext executionContext, string actor, bool isUpdate)
        {
            Guid activityInstanceId = Guid.Empty;
            try
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    activityInstanceId = StartWorkflow(instance, formVariables, executionContext, actor, isUpdate);
                    trans.Complete();
                }
            }
            catch (TransactionException te)
            {
                log.Error(te);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            return activityInstanceId;
        }


        /// <summary>
        /// 使用 LOCK 的方式来启动流程.
        /// </summary>
        /// <param name="instance">流程实例信息.</param>
        /// <param name="formVariables">流程表单参数字典.</param>
        /// <param name="executionContext">流程步骤执行上下文.</param>
        /// <param name="actor">当前处理人.</param>
        /// <param name="isUpdate">是否属于更新流程信息.</param>
        /// <returns>返回流程初始步骤实例 ID.</returns>
        public static Guid StartWorkflowByLock(WorkflowInstance instance, IDictionary<string, object> formVariables, ActivityExecutionContext executionContext, string actor, bool isUpdate)
        {
            Guid activityInstanceId = Guid.Empty;
            lock (syncLocker)
            {
                activityInstanceId = StartWorkflow(instance, formVariables, executionContext, actor, isUpdate);
            }
            return activityInstanceId;
        }

        /// <summary>
        /// 普通方式的启动流程.
        /// </summary>
        /// <param name="instance">流程实例信息.</param>
        /// <param name="formVariables">流程表单参数字典.</param>
        /// <param name="executionContext">流程步骤执行上下文.</param>
        /// <param name="actor">当前处理人.</param>
        /// <param name="isUpdate">是否属于更新流程信息.</param>
        /// <returns>返回流程初始步骤实例 ID.</returns>
        public static Guid StartWorkflow(WorkflowInstance instance, IDictionary<string, object> formVariables, ActivityExecutionContext executionContext, string actor, bool isUpdate)
        {
            Guid activityInstanceId = Guid.Empty;
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
                    workflowFormService.CreateFormInstance(workflowId, workflowInstanceId, actor);

                    //workflowAttachmentService.UpdateWorkflowAttachmentEntities(workflowId, workflowInstanceId);
                }
                workflowFormService.SaveForm(workflowInstanceId, actor, formVariables);

                // 执行下一步骤
                executionContext.ActivityInstanceId = activityInstanceId;
                workflowEngine.ExecuteActivity(executionContext);

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return activityInstanceId;
        }

        #endregion
    }
}
