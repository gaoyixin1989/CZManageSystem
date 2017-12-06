using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;

namespace Botwave.XQP.Service
{
    /// <summary>
    /// 流程运维模块接口
    /// </summary>
    public interface IWorkflowMaintenanceService
    {
        /// <summary>
        /// 更新处理意见
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <param name="request"></param>
        void WorkflowProcessHistoryUpdate(Guid workflowInstanceId, HttpRequest request);

        /// <summary>
        /// 删除处理记录
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <param name="request"></param>
        void WorkflowProcessHistoryDelete(Guid workflowInstanceId, HttpRequest request);

        /// <summary>
        /// 批量删除工单并备份工单
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        string DeleteWorkflowInstance(List<string> list);

        /// <summary>
        /// 查询被转移人的任务列表
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="refcount"></param>
        /// <returns></returns>
        DataTable SearchTasks(Botwave.Workflow.Extension.Service.AdvancedSearchCondition condition, string actors, string processer, int pageIndex, int pageSize, ref int recordCount);

        /// <summary>
        /// 转移待办
        /// </summary>
        /// <param name="activityInstanceIds"></param>
        /// <param name="toUser"></param>
        /// <returns></returns>
        string TransferTodoTask(IList<Guid> activityInstanceIds, string fromUsers, string toUser);

        /// <summary>
        /// 转移已办
        /// </summary>
        /// <param name="workflowInstanceIds"></param>
        /// <param name="toUser"></param>
        /// <returns></returns>
        string TransferDoneTask(IList<Guid> workflowInstanceIds, string fromUsers, string toUser);
    }
}
