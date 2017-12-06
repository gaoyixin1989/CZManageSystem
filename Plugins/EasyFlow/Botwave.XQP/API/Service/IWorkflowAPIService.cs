using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using Botwave.XQP.API.Entity;

namespace Botwave.XQP.API.Service
{
    /// <summary>
    /// webservice接口
    /// </summary>
    public interface IWorkflowAPIService
    {
        /// <summary>
        /// 获取指定用户的待办列表
        /// </summary>
        /// <param name="WorkflowName">指定获取数据的的流程名(多个以,号隔开)</param>
        /// <param name="UserName">用户名</param>
        /// <param name="Activities">指定获取数据的的流程步骤(多个以,号隔开)</param>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="isOnlyStart">是否本人的数据</param>
        /// <returns></returns>
        DataTable GetTodoList(string WorkflowName, string UserName, string Activities, string BeginTime, string EndTime, bool isOnlyStart);

        /// <summary>
        /// 获取指定用户的已办列表
        /// </summary>
        /// <param name="WorkflowName">当前流程名(多个以,号隔开)</param>
        /// <param name="Activities">当前步骤名(多个以,号隔开)</param>
        /// <param name="UserName">用户名</param>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="isOnlyStart">是否本人的数据</param>
        /// <returns></returns>
        DataTable GetDoneList(string WorkflowName, string Activities, string UserName, string BeginTime, string EndTime, bool isOnlyStart);

        /// <summary>
        /// 获取指定用户的“我的工单”列表信息
        /// </summary>
        /// <param name="WorkflowName">当前流程名(多个以,号隔开)</param>
        /// <param name="Activities">当前步骤名(多个以,号隔开)</param>
        /// <param name="UserName">用户名</param>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="State">工单状态</param>
        /// <returns></returns>
        DataTable GetMyTasksList(string WorkflowName, string Activities, string UserName, string BeginTime, string EndTime, string State);

        /// <summary>
        ///  获取指定工单标识的需求单明细信息
        /// </summary>
        /// <param name="workflowInstanceId">需求单工单标识流程实例ID或工单号sheetId</param>
        /// <returns></returns>
        WorkflowDetail GetWorkflowDetailList(string workflowInstanceId);

        /// <summary>
        /// 获取指定需求单工单的当前步骤以及处理状态（正在处理，已完成，已取消等）
        /// </summary>
        /// <param name="workflowInstanceId">需求单工单标识流程实例ID或工单号sheetId</param>
        /// <returns></returns>
        DataTable GetWorkflowStateList(string workflowInstanceId);

        /// <summary>
        /// 获取表单详细信息
        /// </summary>
        /// <param name="WorkflowAlias">；流程类别</param>
        /// <returns></returns>
        FiledInfo[] GetFieldInfo(string WorkflowAlias);

        /// <summary>
        /// 获取流程提单的处理人
        /// </summary>
        /// <param name="WorkflowAlias">流程类别</param>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        Activity[] GetActivityInfo(string WorkflowAlias, string username);

        /// <summary>
        /// 获取下一步处理人
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        Activity[] GetNextActivityInfo(Guid activityInstanceId, string username);

        /// <summary>
        /// 处理数据
        /// </summary>
        /// <param name="username">指定用户的Portal 登录账号</param>
        /// <param name="workflowId">流程定义ID（标识）</param>
        /// <param name="workflowTitle">需求单工单标题</param>
        /// <param name="startTime">工单发起时间</param>
        /// <param name="workflowProperties">需求单工单处理属性</param>
        /// <returns></returns>
        WorkflowExecutionResult StartWorkflow(string username, string workflowId, string workflowTitle, string startTime, string workflowProperties);

        /// <summary>
        /// 传入表单数据以处理需求单
        /// </summary>
        /// <param name="username">指定用户的Portal 登录账号</param>
        /// <param name="activityInstanceId">需求单工单标识(活动实例ID)</param>
        /// <param name="command">工单处理命令。approve：通过审核。reject：退回工单。cancel：取消工单。</param>
        /// <param name="workflowProperties">需求单工单处理属性</param>
        /// <param name="manageOpinion">处理意见</param>
        /// <returns></returns>
        WorkflowExecutionResult ExecuteWorkflow(string username, Guid activityInstanceId, string command, string workflowProperties, string manageOpinion);

        /// <summary>
        /// 获取指定需求单处理列表
        /// </summary>
        /// <param name="workflowInstanceId">需求单工单标识(流程实例ID或工单号sheetId)</param>
        /// <returns></returns>
        IList<WorkflowRecord> GetWorkflowRecord(string workflowInstanceId);

        #region 流程接口 WebService

        /// <summary>
        /// 获取流程分组
        /// </summary>
        /// <returns></returns>
        IList<MenuGroup> GetMenuGroup();

        /// <summary>
        /// 获取流程列表
        /// </summary>
        /// <param name="groupName">流程分组名</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        IList<Botwave.XQP.API.Entity.Workflow> GetWorkflow(string groupName, string userName);

        /// <summary>
        /// 获取流程定义字符串
        /// </summary>
        /// <param name="flowID">唯一标识</param>
        /// <returns></returns>
        string GetWorkflowDefinitionString(Guid flowID);

        /// <summary>
        /// 部署流程
        /// </summary>
        /// <param name="userName">用户账号</param>
        /// <param name="xmlString">流程定义</param>
        /// <returns></returns>
        string WorkflowDeploy(string userName, string xmlString);

        /// <summary>
        /// 获取流程的唯一标识
        /// </summary>
        /// <param name="str">流程名称以及别名</param>
        /// <param name="type">流程名称以及别名</param>
        /// <returns></returns>
        string GetFlowID(string str, string type);

        /// <summary>
        /// 传入表单数据以处理需求单
        /// </summary>
        /// <param name="username">指定用户的Portal 登录账号</param>
        /// <param name="activityInstanceId">需求单工单标识(流程实例ID)</param>
        /// <param name="workflowProperties">需求单工单处理属性</param>
        /// <param name="manageOpinion">处理意见</param>
        /// <returns></returns>
        WorkflowExecutionResult SaveWorkflow(string username, Guid activityInstanceId, string workflowProperties, string manageOpinion);

        /// <summary>
        /// 转交工单
        /// </summary>
        /// <param name="username">指定用户的Portal 登录账号</param>
        /// <param name="assignedUser">被指定用户的Portal 登录账号</param>
        /// <param name="activityInstanceId">需求单工单标识(活动实例ID或工单号sheetId)</param>
        /// <param name="Message">转交意见</param>
        /// <returns></returns>
        WorkflowExecutionResult AssignWorkflow(string username, string assignedUser, string activityInstanceId, string Message);

        /// <summary>
        /// 高级查询条件
        /// </summary>
        /// <param name="username">指定用户的Portal 登录账号</param>
        /// <param name="workflowName">流程名</param>
        /// <returns></returns>
        SearchQueryResult SearchQuery(string username, string workflowName);

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="workflowName">流程</param>
        /// <param name="activityName">步骤</param>
        /// <param name="creator">发起人</param>
        /// <param name="actor">当前处理人</param>
        /// <param name="titleKeywords">标题关键字</param>
        /// <param name="contentKeywords">内容关键字</param>
        /// <param name="sheetId">受理号</param>
        /// <returns></returns>
        SearchResult Search(string username, string BeginTime, string EndTime, string workflowName, string activityName, string creator, string actor, string titleKeywords, string contentKeywords, string sheetId);

        /// <summary>
        /// 评论工单
        /// </summary>
        /// <param name="workflowInstanceId">工单唯一标识</param>
        /// <param name="username">用户名</param>
        /// <param name="content">评论内容</param>
        /// <param name="CommentProperties">附件信息XML格式</param>
        /// <returns></returns>
        CommentResult CommentWorkflow(string workflowInstanceId, string activityInstanceId, string username, string content, string CommentProperties);

        #endregion
    }
}
