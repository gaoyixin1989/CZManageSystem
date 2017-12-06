using System;
using System.Collections.Generic;
using System.Text;
using Botwave.XQP.Domain;

namespace Botwave.XQP.Service
{
    /// <summary>
    /// 流程提醒扩展服务.
    /// </summary>
    public interface IWorkflowNotifyExtendService
    {
        /// <summary>
        /// 获取指定流程步骤实例的通知提醒人信息.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        IList<WorkflowNotifyActor> GetNotifyActors(Guid activityInstanceId);

        /// <summary>
        /// 获取指定流程步骤实例的下一步骤的通知提醒人信息.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        IList<WorkflowNotifyActor> GetNextNotifyActors(Guid activityInstanceId);
    }
}
