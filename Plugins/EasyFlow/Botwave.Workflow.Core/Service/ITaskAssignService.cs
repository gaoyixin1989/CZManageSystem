using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Botwave.Workflow.Domain;
using Botwave.Entities;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// 流程任务分派服务接口.
    /// </summary>
    public interface ITaskAssignService
    {
        /// <summary>
        /// 指派活动/任务.
        /// </summary>
        /// <param name="assignment"></param>
        void Assign(Assignment assignment);

        /// <summary>
        /// 获取指定流程实例的转交信息列表.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        IList<Assignment> GetAssignments(Guid workflowInstanceId);

        /// <summary>
        /// 获取指定流程活动步骤编号的转交分派设置实例.
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        AllocatorOption GetAssignmentAllocator(Guid activityId);

        /// <summary>
        /// 更新流程活动定义的任务转交分派.
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        int UpdateAssignmentAllocators(AllocatorOption option);

        /// <summary>
        /// 获取可以被指派的用户.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        IList<BasicUser> GetUsers4Assignment(Guid activityInstanceId);

        /// <summary>
        /// 获取指定流程步骤实例的可被转交的用户列表.
        /// </summary>
        /// <param name="workflowInstnaceId"></param>
        /// <param name="activityInstanceId"></param>
        /// <param name="actor"></param>
        /// <returns></returns>
        IDictionary<string, string> GetAssignmentActors(Guid workflowInstnaceId, Guid activityInstanceId, string actor);
        
        /// <summary>
        /// 获取指定用户的转交任务列表.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="workflowName"></param>
        /// <param name="keywords"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetAssignmentTasks(string actor, string workflowName, string keywords,
            string beginTime, string endTime, int pageIndex, int pageSize, ref int recordCount);

        /// <summary>
        /// 获取部门流程步骤的处理人信息.
        /// </summary>
        /// <param name="activityInstanceId">流程步骤实例编号.</param>
        /// <returns></returns>
        IList<BasicUser> GetTodoActors(Guid activityInstanceId);

        /// <summary>
        /// 获取指定流程步骤的下一步待办信息列表(用于发送短信或者邮件提醒).
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        IList<TodoInfo> GetNextTodoInfo(Guid activityInstanceId);

        /// <summary>
        /// 新增待办信息.
        /// </summary>
        /// <param name="item"></param>
        void InsertTodo(TodoInfo item);

        /// <summary>
        /// 删除指定流程步骤实例编号的待办信息.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        int DeleteTodo(Guid activityInstanceId);

        /// <summary>
        /// 删除指定流程步骤实例编号的待办信息.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        int DeleteTodo(Guid activityInstanceId, string userName);
        
        /// <summary>
        /// 更新待办未读或已读.
        /// </summary>
        /// <param name="activityInstanceId">流程步骤实例编号.</param>
        /// <param name="userName">用户名.</param>
        /// <param name="isReaded">未读或者已读.true 为已读，false 为未读.</param>
        /// <returns></returns>
        int UpdateTodoReaded(Guid activityInstanceId, string userName, bool isReaded);

        /// <summary>
        /// 检查指定用户和指定步骤的待办信息是否存在.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool IsExistsTodo(Guid activityInstanceId, string userName);

        /// <summary>
        /// 获取指定步骤和指定用户的待办信息.
        ///     不附加流程实例、活动定义待信息.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        TodoInfo GetTodoInfo(Guid activityInstanceId, string userName);

        /// <summary>
        /// 检查是否从全公司人员中选择转交人.
        /// </summary>
        /// <param name="activityInstanceId">流程活动实例编号.</param>
        /// <returns></returns>
        bool IsAssignFromCompany(Guid activityInstanceId);
    }
}
