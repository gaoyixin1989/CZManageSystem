using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Botwave.XQP.API.Service
{
    public interface ISearchAPIService
    {
        /// 获取指定用户的待办列表
        /// </summary>
        /// <param name="userName">登录用户</param>
        /// <param name="workflowName">指定获取数据的的流程名(多个以,号隔开)</param>
        /// <param name="keyWords">关键字</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetTodoList(string userName, string workflowName, string keyWords, string pageIndex, string pageCount, ref int recordCount);

        /// <summary>
        /// 获取指定用户的已办列表
        /// </summary>
        /// <param name="userName">登录用户</param>
        /// <param name="workflowName">当前流程名(多个以,号隔开)</param>
        /// <param name="keyWords">关键字</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetDoneList(string userName, string workflowName, string keyWords, string beginTime, string endTime, string pageIndex, string pageCount, ref int recordCount);

        /// <summary>
        /// 获取指定用户的“我的工单”列表信息
        /// </summary>
        /// <param name="userName">登录用户</param>
        /// <param name="workflowName">当前流程名(多个以,号隔开)</param>
        /// <param name="keyWords">关键字</param>
        /// <param name="state">工单状态(0:所有，1:未完成，2:已完成，3:可撤回)</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetMyTasksList(string userName, string workflowName, string keyWords, string state, string beginTime, string endTime, string pageIndex, string pageCount, ref int recordCount);

        #region 获取指定需求单处理列表

        /// <summary>
        /// 处理列表
        /// </summary>
        /// <param name="workflowInstanceId">需求单工单标识流程实例ID</param>
        /// <param name="sheetId">需求单工单工单号sheetId</param>
        /// <returns></returns>
        DataTable GetWorkflowRecordActivityList(string workflowInstanceId, string sheetId);

        /// <summary>
        /// 转交列表
        /// </summary>
        /// <param name="workflowInstanceId">需求单工单标识流程实例ID</param>
        /// <returns></returns>
        DataTable GetWorkflowRecordAssignmentList(string workflowInstanceId);

        /// <summary>
        /// 会签列表
        /// </summary>
        /// <param name="activityInstanceId">需求单工单标识活动实例ID</param>
        /// <returns></returns>
        DataTable GetWorkflowRecordCountersignedList(string activityInstanceId);

        #endregion

        #region 获取详细信息

        /// <summary>
        /// 获取表单详细信息
        /// </summary>
        /// <param name="workflowAlias">流程类别简写</param>
        /// <param name="workflowName">流程类别中文</param>
        /// <returns></returns>
        DataTable GetFieldInfoList(string workflowAlias, string workflowName);

        /// <summary>
        /// 获取流程提单的处理人
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="workflowAlias">流程类别</param>
        /// <param name="workflows">流程明晰化</param>
        /// <returns></returns>
        DataTable GetActivityInfoList(string userName, string workflowAlias, string workflowName);

        /// <summary>
        /// 获取流程提单的抄送人
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="workflowAlias">流程类别</param>
        /// <param name="workflowName">流程类别中文</param>
        /// <returns></returns>
        DataTable GetActivitiesProfileInfoList(string userName, string workflowAlias, string workflowName);

        /// <summary>
        /// 获取下一步处理人
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        DataTable GetNextActivityInfoList(string userName, string activityInstanceId);

        #endregion

        #region 高级查询

        /// <summary>
        /// 高级查询条件
        /// </summary>
        /// <param name="userName">指定用户的Portal 登录账号</param>
        /// <returns></returns>
        DataTable GetSearchQueryList(string userName);

        /// <summary>
        /// 高级查询条件
        /// </summary>
        /// <param name="userName">指定用户的Portal 登录账号</param>
        /// <param name="workflowName">流程名</param>
        /// <returns></returns>
        DataTable GetSearchQueryList(string userName, string workflowName);

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="workflowName">流程</param>
        /// <param name="activityName">步骤</param>
        /// <param name="creator">发起人</param>
        /// <param name="actor">当前处理人</param>
        /// <param name="titleKeywords">标题关键字</param>
        /// <param name="contentKeywords">内容关键字</param>
        /// <param name="sheetId">受理号</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetSearchList(string userName, string beginTime, string endTime, string workflowName, string activityName, string creator, string actor, string titleKeywords, string contentKeywords, string sheetId, string pageIndex, string pageCount, ref int recordCount);

        #endregion

        /// <summary>
        /// 获取指定需求单工单的当前步骤以及处理状态（正在处理，已完成，已取消等）
        /// </summary>
        /// <param name="workflowInstanceId">需求单工单标识流程实例ID</param>
        /// <param name="sheetId">需求单工单标识工单号sheetId</param>
        /// <returns></returns>
        DataTable GetWorkflowState(string workflowInstanceId, string sheetId);

        /// <summary>
        /// 获取流程分组
        /// </summary>
        /// <returns></returns>
        DataTable GetMenuGroup();

        /// <summary>
        /// 获取流程列表
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="groupName">流程分组名</param>
        /// <returns></returns>
        DataTable GetWorkflow(string userName, string groupName);

        #region 获取指定工单标识的需求单明细信息Detail

        /// <summary>
        /// 获取指定工单标识的明细信息
        /// </summary>
        /// <param name="workflowInstanceId">需求单工单标识流程实例ID</param>
        /// <param name="sheetId">需求单工单工单号sheetId</param>
        /// <returns></returns>
        DataTable GetWorkflowDetail(string workflowInstanceId, string sheetId);

        /// <summary>
        /// 获取工单详细信息
        /// </summary>
        /// <param name="workflowinstanceid">需求单工单标识流程实例ID</param>
        /// <returns></returns>
        DataTable GetWorkflowInfo(string workflowinstanceid);

        /// <summary>
        /// 获取指定工单的表单
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        DataTable GetWorkflowFieldList(string workflowInstanceId);

        /// <summary>
        /// 获取指定工单的下一步
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        DataTable GetWorkflowActivitysList(string workflowInstanceId);

        /// <summary>
        /// 获取指定工单的附件
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        DataTable GetWorkflowAttachmentList(string workflowInstanceId);

        #endregion

        #region 获取评论信息

        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        DataTable GetCommentList(string workflowInstanceId);

        /// <summary>
        /// 评论附件信息
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        DataTable GetCommentAttachemntList(Guid commentId, string workflowInstanceId);

        /// <summary>
        /// 获取步骤和处理人
        /// </summary>
        /// <param name="Activityinstanceid"></param>
        /// <returns></returns>
        DataTable GetActivityActor(string Workflowinstanceid, string workflowname, string actor, string activityname, string workflowProperties);
        #endregion
    }
}
