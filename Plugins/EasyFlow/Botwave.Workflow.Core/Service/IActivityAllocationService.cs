using System;
using System.Collections.Generic;
using System.Text;

using Botwave.Workflow.Domain;
using Botwave.Workflow.Allocator;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// 流程活动(步骤)任务分派服务接口.
    /// </summary>
    public interface IActivityAllocationService
    {
        /// <summary>
        /// 获取目标用户.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <param name="options"></param>
        /// <param name="actor"></param>
        /// <param name="withComment">是否带有备注/真实姓名等信息.</param>
        /// <returns></returns>
        IDictionary<string, string> GetTargetUsers(Guid workflowInstanceId, AllocatorOption options, string actor, bool withComment);

        /// <summary>
        /// 获取目标用户.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <param name="options"></param>
        /// <param name="actor"></param>
        /// <param name="variables">变量属性字典.</param>
        /// <param name="withComment">是否带有备注/真实姓名等信息.</param>
        /// <returns></returns>
        IDictionary<string, string> GetTargetUsers(Guid workflowInstanceId, AllocatorOption options, string actor, IDictionary<string, object> variables, bool withComment);
    }
}
