using System;
using System.Collections.Generic;
using System.Text;
using Botwave.XQP.API.Entity;
namespace Botwave.XQP.API.Interfaces
{

    /// <summary>
    /// 流程处理服务接口
    /// </summary>
    public interface IWorkflowProcessService
    {
        #region 查询

        /// <summary>
        /// 获取指定用户的待办列表
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <param name="username">指定用户的Portal 登录账号</param>
        /// <param name="workflows">指定获取数据的流程名称，多个流程以逗号隔开。</param>
        /// <param name="activities">指定获取数据的的流程步骤，多个步骤以逗号隔开。</param>
        /// <param name="beginTime">起始时间. 时间格式：2010-9-27 10:18</param>
        /// <param name="endTime">结束时间. 时间格式：2010-9-27 10:18</param>
        /// <returns>返回 需求单待办列表结果</returns>
        WorkflowTodoResult GetWorkflowTodoList(string systemId, string sysAccount, string sysPassword,
            string username, string workflows, string activities,
             string beginTime, string endTime);

        /// <summary>
        /// 获取指定用户的已办列表
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <param name="username">指定用户的Portal 登录账号</param>
        /// <param name="workflows">指定获取数据的流程名称，多个流程以逗号隔开。</param>
        /// <param name="activities">指定获取数据的的流程步骤，多个步骤以逗号隔开。</param>
        /// <param name="beginTime">起始时间. 时间格式：2010-9-27 10:18</param>
        /// <param name="endTime">结束时间. 时间格式：2010-9-27 10:18</param>
        /// <returns>返回 需求单已办列表结果</returns>
        WorkflowTaskResult GetWorkflowDoneList(string systemId, string sysAccount, string sysPassword,
            string username, string workflows, string activities, string beginTime, string endTime);

        /// <summary>
        /// 获取指定用户的“我的工单”列表信息
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <param name="username">指定用户的Portal 登录账号</param>
        /// <param name="workflows">指定获取数据的流程名称，多个流程以逗号隔开。</param>
        /// <param name="activities">指定获取数据的的流程步骤，多个步骤以逗号隔开。</param>
        /// <param name="beginTime">起始时间. 时间格式：2010-9-27 10:18</param>
        /// <param name="endTime">结束时间. 时间格式：2010-9-27 10:18</param>
        /// <returns>返回 需求单我的工单列表结果</returns>
        WorkflowTaskResult GetWorkflowMyTasks(string systemId, string sysAccount, string sysPassword, string username,
             string workflows, string activities, string beginTime, string endTime);

        /// <summary>
        /// 获取指定工单标识的需求单明细信息
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <param name="workflowInstanceId">需求单工单标识流程实例ID或工单号sheetId</param>
        /// <returns>返回详细结果</returns>
        WorkflowDetailResult GetWorkflowDetail(string systemId, string sysAccount, string sysPassword, string workflowInstanceId);

        /// <summary>
        /// 获取指定需求单工单的当前步骤以及处理状态（正在处理，已完成，已取消等）
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <param name="workflowInstanceId">需求单工单标识流程实例ID或工单号sheetId</param>
        /// <returns>返回状态结果</returns>
        WorkflowStateResult GetWorkflowState(string systemId, string sysAccount, string sysPassword, string workflowInstanceId);

        /// <summary>
        /// 获取制定类别的表单以及步骤和处理人列表
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <param name="WorkflowAlias">流程类别</param>
        /// <returns></returns>
        WorkflowInfoResult GetWorkflowInfo(string systemId, string sysAccount, string sysPassword, string workflowAlias, string userName, string activityInstanceId);

        #endregion

        #region 流程处理

        /// <summary>
        /// 发起一个新的需求单工单
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <param name="username">指定用户的Portal 登录账号</param>
        /// <param name="workflowId">流程定义ID（标识）</param>
        /// <param name="workflowTitle">需求单工单标题</param>
        /// <param name="startTime">工单发起时间</param>
        /// <param name="workflowProperties">需求单工单处理属性</param>
        /// <returns>返回需求单处理结果列表</returns>
        WorkflowExecutionResult StartWorkflow(string systemId, string sysAccount, string sysPassword, string username,
                                         string workflowId, string workflowTitle, string startTime, string workflowProperties);

        /// <summary>
        /// 传入表单数据以处理需求单
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <param name="username">指定用户的Portal 登录账号</param>
        /// <param name="workflowInstanceId">需求单工单标识(流程实例ID或工单号sheetId)</param>
        /// <param name="command">工单处理命令。approve：通过审核。reject：退回工单。cancel：取消工单。</param>
        /// <param name="workflowProperties">需求单工单处理属性</param>
        /// <returns></returns>
        WorkflowExecutionResult ExecuteWorkflow(string systemId, string sysAccount, string sysPassword, string username,
                                        string activityInstanceId, string command, string workflowProperties, string manageOpinion);

        /// <summary>
        /// 获取指定需求单处理列表
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <param name="workflowInstanceId">需求单工单标识(流程实例ID或工单号sheetId)</param>
        /// <returns></returns>
        WorkflowRecordResult GetWorkflowRecord(string systemId, string sysAccount, string sysPassword, string workflowInstanceId);

        #endregion

        /* 流程接口------v0.2 */
        #region 流程接口 WebService

        /// <summary>
        /// 获取流程分组信息
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <returns></returns>
        WorkflowMenuGroupResult GetMenuGroup(string systemId, string sysAccount, string sysPassword);

        /// <summary>
        /// 获取流程列表
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <param name="groupName">流程分组名</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        WorkflowResult GetWorkflow(string systemId, string sysAccount, string sysPassword, string groupName, string userName);

        /// <summary>
        /// 获取流程定义
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <param name="flowID">流程标识</param>
        /// <param name="flowType">流程标识类型</param>
        /// <returns></returns>
        WorkflowDefinitionResult GetWorkflowDefinition(string systemId, string sysAccount, string sysPassword, string flowID, string flowType);

        /// <summary>
        /// 部署流程
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <param name="userName">用户portal帐号</param>
        /// <param name="xmlString">流程定义</param>
        /// <returns></returns>
        WorkflowDeployResult WorkflowDeploy(string systemId, string sysAccount, string sysPassword, string userName, string xmlString);

        /// <summary>
        /// 保存工单
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <param name="username">指定用户的Portal 登录账号</param>
        /// <param name="activityInstanceId">需求单工单标识(活动实例ID或工单号sheetId)</param>
        /// <param name="workflowProperties">需求单工单处理属性</param>
        /// <returns></returns>
        WorkflowExecutionResult SaveWorkflow(string systemId, string sysAccount, string sysPassword, string username,
                                        string activityInstanceId, string workflowProperties);
        /// <summary>
        /// 转交工单
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <param name="username">指定用户的Portal 登录账号</param>
        /// <param name="assignedUser">被指定用户的Portal 登录账号</param>
        /// <param name="activityInstanceId">需求单工单标识(活动实例ID或工单号sheetId)</param>
        /// <param name="Message">转交意见</param>
        /// <returns></returns>
        WorkflowExecutionResult AssignWorkflow(string systemId, string sysAccount, string sysPassword, string username, string assignedUser, string activityInstanceId, string Message);

        /// <summary>
        /// 高级查询条件
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <param name="username">指定用户的Portal 登录账号</param>
        /// <param name="workflowName">流程名</param>
        /// <returns></returns>
        SearchQueryResult SearchQuery(string systemId, string sysAccount, string sysPassword, string username, string workflowName);

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
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
        SearchResult Search(string systemId, string sysAccount, string sysPassword, string username, string BeginTime, string EndTime, string workflowName, string activityName, string creator, string actor, string titleKeywords, string contentKeywords, string sheetId);

        /// <summary>
        /// 评论工单
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <param name="workflowInstanceId">工单唯一标识</param>
        /// <param name="username">用户名</param>
        /// <param name="content">评论内容</param>
        /// <param name="CommentProperties">附件信息XML格式</param>
        /// <returns></returns>
        CommentResult CommentWorkflow(string systemId, string sysAccount, string sysPassword, string workflowInstanceId, string activityInstanceId, string username, string content, string CommentProperties);

        #endregion

    }
}
