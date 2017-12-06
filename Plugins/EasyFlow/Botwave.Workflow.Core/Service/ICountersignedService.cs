using System;
using System.Collections.Generic;
using System.Text;

using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// 流程活动(步骤)会签服务接口.
    /// </summary>
    public interface ICountersignedService
    {
        /// <summary>
        /// 会签.
        /// </summary>
        /// <param name="countersigned"></param>
        void Sign(Countersigned countersigned);

        /// <summary>
        /// 获取指定活动实例的待办列表.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        IList<TodoInfo> GetTodoList(Guid activityInstanceId);

        /// <summary>
        /// 获取指定活动实例的会签记录列表.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        IList<Countersigned> GetCountersignedList(Guid activityInstanceId);
    }
}
