using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.GMCCServiceHelpers;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;

namespace Botwave.XQP.Service.Plugins
{
    /// <summary>
    /// 流程待办任务/待办信息辅助类.
    /// </summary>
    public class WorkflowPostHelper
    {
        #region 外部待办待阅 URL

        /// <summary>
        /// 获取流程步骤实例的外部待办 URL.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        public static string TransformUrlByActivityInstanceId(string activityInstanceId)
        {
            return string.Format("{0}ssoproxy.ashx?contrib/workflow/pages/process.aspx?aiid={1}", GlobalSettings.Instance.Address, activityInstanceId);
        }

        /// <summary>
        /// 获取流程步骤实例的外部待阅 URL
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        public static string TransformViewUrlByActivityInstanceId(string activityInstanceId)
        {
            return string.Format("{0}ssoproxy.ashx?contrib/workflow/pages/workflowview.aspx?aiid={1}", GlobalSettings.Instance.Address, activityInstanceId);
        }
        #endregion

        /// <summary>
        /// 删除流程实例对应的待办工作.
        /// </summary>
        /// <param name="activityService"></param>
        /// <param name="context"></param>
        public static void DeletePendingJob(IActivityService activityService, ActivityExecutionContext context)
        {
            IList<ActivityInstance> list = activityService.GetActivitiesInSameWorkflow(context.ActivityInstanceId);
            foreach (ActivityInstance item in list)
            {
                //如果活动没关闭，则删除相关的待办工作
                if (!item.IsCompleted)
                {
                    AsynExtendedPendingJobHelper.DeletePendingJobByEntity(ActivityInstance.EntityType, item.ActivityInstanceId.ToString());
                }
            }
        }

        /// <summary>
        /// 取消流程实例的后续处理.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        public static void PostCancelWorkflowInstance(Guid activityInstanceId)
        {
            IDbDataParameter param = IBatisDbHelper.CreateParameter();
            param.ParameterName = "@ActivityInstanceId";
            param.DbType = DbType.Guid;
            param.Value = activityInstanceId;
            IBatisDbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "xqp_wf_PostCancelWorkflowInstance", param);
        }

        /// <summary>
        /// 关闭流程步骤实例的后续处理.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        public static void PostCloseActivityInstance(Guid activityInstanceId)
        {
            IDbDataParameter param = IBatisDbHelper.CreateParameter();
            param.ParameterName = "@ActivityInstanceId";
            param.DbType = DbType.Guid;
            param.Value = activityInstanceId;
            IBatisDbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "xqp_wf_PostCloseActivityInstance", param);
        }
    }
}
