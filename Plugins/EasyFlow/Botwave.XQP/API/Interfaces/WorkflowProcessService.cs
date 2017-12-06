using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Data;
using Botwave.XQP.API.Entity;
using Botwave.XQP.Service;
using Botwave.DynamicForm.Services;
using Botwave.Workflow.Service;
using Botwave.XQP.API.Interfaces;
using Botwave.XQP.API.Service;
using Botwave.XQP.API.Util;

namespace Botwave.XQP.API
{

    /// <summary>
    /// 流程处理服务
    /// </summary>
    public class WorkflowProcessService : IWorkflowProcessService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(WorkflowProcessService));

        ////流程字段数据服务
        //public IWorkflowFieldService WorkflowFieldService { protected set; get; }
        ////表单实例服务
        //public IFormInstanceService FormInstanceService { protected set; get; }
        ////流程服务集合
        //public IWorkflowGenericService WorkflowGenericService { protected set; get; }

        #region 属性

        /// <summary>
        /// 流程定义服务接口
        /// </summary>
        public IWorkflowDefinitionService WorkflowDefinitionService { set; get; }

        /// <summary>
        /// 流动实例服务接口
        /// </summary>
        public IActivityService ActivityService { set; get; }

        public IWorkflowPagerService WorkflowPagerService { set; get; }

        /// <summary>
        /// WebService实现类
        /// </summary>
        public IWorkflowAPIService WorkflowAPIService { set; get; }

        #endregion

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
        public WorkflowTodoResult GetWorkflowTodoList(string systemId, string sysAccount, string sysPassword,
            string username, string workflows, string activities, string beginTime, string endTime)
        {
            
            WorkflowTodoResult result = new WorkflowTodoResult();

            //if (!ValidateAuthorizationCode(result))//验证码验证
            //{
            //    return result;
            //}

            if (!Validate(systemId, sysAccount, sysPassword, result))
            {
                return result;
            }

            if (string.IsNullOrEmpty(username))
            {
                result.Message = "Portal 登录账号 不能为空";
                return result;
            }

            List<WorkflowTodo> workflowTodoArr = new List<WorkflowTodo>();
            DataTable source = WorkflowAPIService.GetTodoList(workflows, username, activities, beginTime, endTime, true);
            int n = source.Rows.Count;

            if (n == 0)
            {
                result.Message = "待办列表 数据为空";
                return result;
            }

            for (int j = 0; j < n; j++)
            {
                WorkflowTodo workflowTodo = new WorkflowTodo();
                workflowTodo.ActivityInstanceId = source.Rows[j]["ActivityInstanceId"].ToString().Trim();
                workflowTodo.WorkflowInstanceId = source.Rows[j]["WorkflowInstanceId"].ToString().Trim();
                workflowTodo.Category = source.Rows[j]["WorkflowAlias"].ToString().Trim();
                workflowTodo.SheetId = source.Rows[j]["SheetId"].ToString().Trim();
                workflowTodo.Title = source.Rows[j]["title"].ToString().Trim();
                workflowTodo.WorkflowName = source.Rows[j]["WorkflowName"].ToString().Trim();
                workflowTodo.CurrentActvities = source.Rows[j]["ActivityName"].ToString().Trim();
                workflowTodo.CurrentActors = source.Rows[j]["TodoActors"].ToString().Trim();
                workflowTodo.Starter = source.Rows[j]["Creator"].ToString().Trim();
                workflowTodo.StartedTime = Util.XmlAnalysisHelp.ToDatetime(source.Rows[j]["StartedTime"].ToString().Trim());
                workflowTodo.CreatedTime = Util.XmlAnalysisHelp.ToDatetime(source.Rows[j]["CreatedTime"].ToString().Trim());
                workflowTodo.ExpectFinishedTime = source.Rows[j]["expectFinishedTime"].ToString().Trim();
                workflowTodoArr.Add(workflowTodo);
            }
            result.Todos = workflowTodoArr.ToArray();

            return result;
        }

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
        public WorkflowTaskResult GetWorkflowDoneList(string systemId, string sysAccount, string sysPassword,
            string username, string workflows, string activities, string beginTime, string endTime)
        {
            WorkflowTaskResult result = new WorkflowTaskResult();
            if (this.Validate(systemId, sysAccount, sysPassword, result))
            {
                List<WorkflowTask> list = new List<WorkflowTask>();
                DataTable table = this.WorkflowAPIService.GetDoneList(workflows, activities, username, beginTime, endTime, false);
                int count = table.Rows.Count;
                if (count == 0)
                {
                    result.Message = "已办列表 数据为空";
                    return result;
                }
                for (int i = 0; i < count; i++)
                {
                    WorkflowTask item = new WorkflowTask();
                    item.WorkflowInstanceId = table.Rows[i]["WorkflowInstanceId"].ToString().Trim();
                    item.Category = table.Rows[i]["WorkflowAlias"].ToString().Trim();
                    item.SheetId = table.Rows[i]["SheetId"].ToString().Trim();
                    item.Title = table.Rows[i]["Title"].ToString().Trim();
                    item.CurrentActvities = table.Rows[i]["CurrentActivityNames"].ToString().Trim();
                    item.CurrentActors = table.Rows[i]["CurrentActors"].ToString().Trim();
                    item.Starter = table.Rows[i]["CreatorName"].ToString().Trim();
                    item.StartedTime = XmlAnalysisHelp.ToDatetime(table.Rows[i]["StartedTime"].ToString().Trim());
                    item.FinishedTime = table.Rows[i]["FinishedTime"].ToString().Trim();
                    item.ActivityName = table.Rows[i]["ActivityName"].ToString().Trim();
                    item.ActivityInstanceId = table.Rows[i]["ActivityInstanceId"].ToString().Trim();
                    item.ActivityActor = table.Rows[i]["ActivityActor"].ToString().Trim();
                    list.Add(item);
                }
                result.Tasks = list.ToArray();
            }
            return result;
        }

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
        public WorkflowTaskResult GetWorkflowMyTasks(string systemId, string sysAccount, string sysPassword,
            string username, string workflows, string activities, string beginTime, string endTime)
        {
            WorkflowTaskResult result = new WorkflowTaskResult();
            if (!Validate(systemId, sysAccount, sysPassword, result))
            {
                return result;
            }

            if (string.IsNullOrEmpty(username))
            {
                result.Message = "Portal 登录账号 不能为空";
                return result;
            }

            List<WorkflowTask> WorkflowTasks = new List<WorkflowTask>();
            DataTable source = WorkflowAPIService.GetMyTasksList(workflows, activities, username, beginTime, endTime, null);
            int n = source.Rows.Count;

            if (n == 0)
            {
                result.Message = "我的工单  数据为空";
                return result;
            }

            for (int j = 0; j < n; j++)
            {
                WorkflowTask workflowtask = new WorkflowTask();

                workflowtask.WorkflowInstanceId = source.Rows[j]["WorkflowInstanceId"].ToString().Trim();
                workflowtask.Category = source.Rows[j]["WorkflowAlias"].ToString().Trim();
                workflowtask.SheetId = source.Rows[j]["SheetId"].ToString().Trim();
                workflowtask.Title = source.Rows[j]["Title"].ToString().Trim();
                workflowtask.CurrentActvities = source.Rows[j]["ActivityName"].ToString().Trim();
                workflowtask.CurrentActors = source.Rows[j]["CurrentActors"].ToString().Trim();
                workflowtask.Starter = source.Rows[j]["CreatorName"].ToString().Trim();
                workflowtask.StartedTime = Util.XmlAnalysisHelp.ToDatetime(source.Rows[j]["StartedTime"].ToString().Trim());
                workflowtask.ResortTime = CalculateResortTime(source.Rows[j]["ActivityCreatedTime"].ToString().Trim());
                workflowtask.WorkflowName = source.Rows[j]["WorkflowName"].ToString().Trim();
                WorkflowTasks.Add(workflowtask);
            }
            result.Tasks = WorkflowTasks.ToArray();

            return result;
        }

        /// <summary>
        /// 获取指定工单标识的需求单明细信息
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <param name="workflowInstanceId">需求单工单标识流程实例ID或工单号sheetId</param>
        /// <returns>返回详细结果</returns>
        public WorkflowDetailResult GetWorkflowDetail(string systemId, string sysAccount, string sysPassword, string workflowInstanceId)
        {
            WorkflowDetailResult DetailResult = new WorkflowDetailResult();
            if (!Validate(systemId, sysAccount, sysPassword, DetailResult))
            {
                return DetailResult;
            }

            if (string.IsNullOrEmpty(workflowInstanceId))
            {
                DetailResult.Message = "流程实例ID或工单号sheetId 不能为空";
                return DetailResult;
            }

            WorkflowDetail Detail = WorkflowAPIService.GetWorkflowDetailList(workflowInstanceId);
            DetailResult.Detail = Detail;

            if (Detail == null)
            {
                DetailResult.Message = "无该需求单明细信息 ";
            }
            return DetailResult;
        }

        /// <summary>
        /// 获取指定需求单工单的当前步骤以及处理状态（正在处理，已完成，已取消等）
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <param name="workflowInstanceId">需求单工单标识流程实例ID或工单号sheetId</param>
        /// <returns>返回状态结果</returns>
        public WorkflowStateResult GetWorkflowState(string systemId, string sysAccount, string sysPassword, string workflowInstanceId)
        {
            WorkflowStateResult StateResult = new WorkflowStateResult();
            if (!Validate(systemId, sysAccount, sysPassword, StateResult))
            {
                return StateResult;
            }

            if (string.IsNullOrEmpty(workflowInstanceId))
            {
                StateResult.Message = "流程实例ID或工单号sheetId 不能为空";
                return StateResult;
            }

            DataTable soure = WorkflowAPIService.GetWorkflowStateList(workflowInstanceId);
            if (soure != null && soure.Rows.Count > 0)
            {
                StateResult.Creator = soure.Rows[0]["Creator"].ToString().Trim();
                StateResult.CurrentActivities = soure.Rows[0]["ActivityName"].ToString().Trim().Split(',');
                StateResult.State = int.Parse(soure.Rows[0]["State"].ToString().Trim());
                StateResult.WorkflowInstanceId = soure.Rows[0]["WorkflowInstanceId"].ToString().Trim();
                StateResult.WorkflowSheetId = soure.Rows[0]["SheetId"].ToString().Trim();
                StateResult.WorkflowTitle = soure.Rows[0]["Title"].ToString().Trim();
            }
            else
            {
                StateResult.Message = "无该需求单工单";
            }
            return StateResult;
        }

        /// <summary>
        /// 获取制定类别的表单以及步骤和处理人列表
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <param name="WorkflowAlias">流程类别</param>
        /// <param name="userName">用户名</param>
        /// <param name="activityInstanceId">工单标识</param>
        /// <returns></returns>
        public WorkflowInfoResult GetWorkflowInfo(string systemId, string sysAccount, string sysPassword, string workflowAlias, string userName, string activityInstanceId)
        {
            WorkflowInfoResult InfoResult = new WorkflowInfoResult();
            if (!Validate(systemId, sysAccount, sysPassword, InfoResult))
            {
                return InfoResult;
            }

            if (!string.IsNullOrEmpty(workflowAlias))
            {
                InfoResult.Fields = WorkflowAPIService.GetFieldInfo(workflowAlias);
                if (!string.IsNullOrEmpty(userName))
                {
                    InfoResult.Activitys = WorkflowAPIService.GetActivityInfo(workflowAlias, userName);
                }
            }

            Guid? aiid = Util.XmlAnalysisHelp.ToGuid(activityInstanceId);
            if (aiid.HasValue)
            {
                InfoResult.NextActivityies = WorkflowAPIService.GetNextActivityInfo(aiid.Value, userName);
            }

            return InfoResult;
        }

        #endregion

        #region 流程处理

        /// <summary>
        /// 发起一个新的需求单工单
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <param name="username">指定用户的Portal 登录账号</param>
        /// <param name="workflowId">流程定义名称（标识）</param>
        /// <param name="workflowTitle">需求单工单标题</param>
        /// <param name="startTime">工单发起时间</param>
        /// <param name="workflowProperties">需求单工单处理属性</param>
        /// <returns>返回需求单处理结果列表</returns>
        public WorkflowExecutionResult StartWorkflow(string systemId, string sysAccount, string sysPassword, string username,
                                         string workflowId, string workflowTitle, string startTime, string workflowProperties)
        {
            WorkflowExecutionResult ExecutionResult = new WorkflowExecutionResult();

            if (!Validate(systemId, sysAccount, sysPassword, ExecutionResult))
            {
                return ExecutionResult;
            }

            if (string.IsNullOrEmpty(username))
            {
                ExecutionResult.Message = "Portal 登录账号 不能为空";
                return ExecutionResult;
            }
            if (string.IsNullOrEmpty(workflowId))
            {
                ExecutionResult.Message = "流程定义名称（标识）不能为空";
                return ExecutionResult;
            }

            if (string.IsNullOrEmpty(workflowTitle))
            {
                ExecutionResult.Message = "需求单工单标题 不能为空";
                return ExecutionResult;
            }

            if (string.IsNullOrEmpty(workflowProperties))
            {
                ExecutionResult.Message = "需求单工单处理属性 不能为空";
                return ExecutionResult;
            }

            if (string.IsNullOrEmpty(startTime))
            {
                startTime = DateTime.Now.ToString();
            }

            ExecutionResult = WorkflowAPIService.StartWorkflow(username, workflowId, workflowTitle, startTime, workflowProperties);

            return ExecutionResult;
        }


        /// <summary>
        /// 传入表单数据以处理需求单
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <param name="username">指定用户的Portal 登录账号</param>
        /// <param name="activityInstanceId">需求单工单标识(活动实例ID或工单号sheetId)</param>
        /// <param name="command">工单处理命令。approve：通过审核。reject：退回工单。cancel：取消工单。</param>
        /// <param name="workflowProperties">需求单工单处理属性</param>
        /// <param name="manageOpinion">处理意见</param>
        /// <returns></returns>
        public WorkflowExecutionResult ExecuteWorkflow(string systemId, string sysAccount, string sysPassword, string username,
                                        string activityInstanceId, string command, string workflowProperties, string manageOpinion)
        {
            WorkflowExecutionResult ExecutionResult = new WorkflowExecutionResult();
            ExecutionResult.Success = "-1";
            if (!Validate(systemId, sysAccount, sysPassword, ExecutionResult))
            {
                return ExecutionResult;
            }

            if (string.IsNullOrEmpty(username))
            {
                ExecutionResult.Message = "指定用户的Portal 登录账号 不能为空";
                return ExecutionResult;
            }

            Guid? aiid = Util.XmlAnalysisHelp.ToGuid(activityInstanceId);
            if (!aiid.HasValue)
            {
                ExecutionResult.Message = "活动实例ID（标识）不能为空";
                return ExecutionResult;
            }

            if (string.IsNullOrEmpty(command))
            {
                ExecutionResult.Message = "单处理命令 不能为空";
                return ExecutionResult;
            }

            if (string.IsNullOrEmpty(workflowProperties))
            {
                ExecutionResult.Message = "需求单工单处理属性 不能为空";
                return ExecutionResult;
            }

            try
            {
                ExecutionResult = WorkflowAPIService.ExecuteWorkflow(username, aiid.Value, command, workflowProperties, manageOpinion);
                ExecutionResult.Success = "1";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ExecutionResult.Message = ex.Message;
            }

            return ExecutionResult;
        }

        /// <summary>
        /// 获取指定需求单处理列表
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <param name="workflowInstanceId">需求单工单标识(流程实例ID或工单号sheetId)</param>
        /// <returns></returns>
        public WorkflowRecordResult GetWorkflowRecord(string systemId, string sysAccount, string sysPassword, string workflowInstanceId)
        {
            WorkflowRecordResult wrResult = new WorkflowRecordResult();

            if (!Validate(systemId, sysAccount, sysPassword, wrResult))
            {
                return wrResult;
            }

            if (string.IsNullOrEmpty(workflowInstanceId))
            {
                wrResult.Message = "流程实例ID或工单号sheetId 不能为空";
                return wrResult;
            }
            IList<WorkflowRecord> Record = WorkflowAPIService.GetWorkflowRecord(workflowInstanceId);
            if (Record != null)
            {
                wrResult.Records = (new List<WorkflowRecord>(Record)).ToArray();
                wrResult.Message = "获取指定需求单处理列表 成功";
            }
            else
                wrResult.Message = "流程实例ID或工单号sheetId 不存在";
            return wrResult;
        }

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
        public WorkflowMenuGroupResult GetMenuGroup(string systemId, string sysAccount, string sysPassword)
        {
            WorkflowMenuGroupResult result = new WorkflowMenuGroupResult();
            if (!Validate(systemId, sysAccount, sysPassword, result))//是否调用成功
            {
                return result;
            }

            try
            {
                List<MenuGroup> list = WorkflowAPIService.GetMenuGroup() as List<MenuGroup>;
                result.GroupNames = list.ToArray();
                result.Success = "1";//执行成功
            }
            catch (Exception ex)
            {
                log.Error(ex);
                result.Message = ex.Message;
                result.Success = "-1";//执行错误
            }
            return result;
        }

        /// <summary>
        /// 获取流程列表
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <param name="groupName">流程分组名</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public WorkflowResult GetWorkflow(string systemId, string sysAccount, string sysPassword, string groupName, string userName)
        {
            WorkflowResult result = new WorkflowResult();
            if (!Validate(systemId, sysAccount, sysPassword, result))//是否调用成功
            {
                return result;
            }

            try
            {
                result.Workflows =(new List<Botwave.XQP.API.Entity.Workflow>(WorkflowAPIService.GetWorkflow(groupName, userName))).ToArray();
                result.Success = "1";//执行成功
            }
            catch (Exception ex)
            {
                log.Error(ex);
                result.Message = ex.Message;
                result.Success = "-1";//执行错误
            }

            return result;
        }

        /// <summary>
        /// 获取流程定义
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <param name="flowID">流程标识</param>
        /// <param name="flowType">流程标识类型</param>
        /// <returns></returns>
        public WorkflowDefinitionResult GetWorkflowDefinition(string systemId, string sysAccount, string sysPassword, string flowID,string flowType)
        {
            WorkflowDefinitionResult result = new WorkflowDefinitionResult();
            result.Success = "-1";//执行错误
            if (!Validate(systemId, sysAccount, sysPassword, result))//是否调用成功
            {
                return result;
            }

            try
            {
                if (string.IsNullOrEmpty(flowID))
                {
                    result.Message = "唯一标志不能为空";
                    return result;
                }

                Guid? workflowID = null;
                switch (flowType)
                {
                    case "唯一标志":
                        workflowID = XmlAnalysisHelp.ToGuid(flowID);
                        break;
                    case "名称":
                        workflowID = new Guid(WorkflowAPIService.GetFlowID(flowID, flowType));
                        break;
                    case "别名":
                        workflowID = new Guid(WorkflowAPIService.GetFlowID(flowID, flowType));
                        break;
                    default:
                        workflowID = XmlAnalysisHelp.ToGuid(flowID);
                        break;
                }
                if (workflowID == null)
                {
                    if (string.IsNullOrEmpty(flowType))
                        flowType = "唯一标志";
                    result.Message = "不存在该" + flowType;
                    return result;
                }
                result.ExportFlowStr = WorkflowAPIService.GetWorkflowDefinitionString(workflowID.Value);

                result.Success = "1";//执行成功
            }
            catch (Exception ex)
            {
                log.Error(ex);
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 部署流程
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <param name="userName">用户portal帐号</param>
        /// <param name="xmlString">流程定义</param>
        /// <returns></returns>
        public WorkflowDeployResult WorkflowDeploy(string systemId, string sysAccount, string sysPassword, string userName, string xmlString)
        {
            WorkflowDeployResult result = new WorkflowDeployResult();
            result.Success = "-1";//执行错误
            
            if (!Validate(systemId, sysAccount, sysPassword, result))//是否调用成功
            {
                return result;
            }

            try
            {
                if (string.IsNullOrEmpty(userName))
                {
                    result.Message = "用户portal帐号不能为空";
                    return result;
                }

                string flowid = WorkflowAPIService.WorkflowDeploy(userName, xmlString);//部署流程
                try { XmlAnalysisHelp.ToGuid(flowid); }
                catch
                {
                    result.Message = flowid;
                    return result;
                }
                result.Success = "1";
                result.WorkflowId = flowid;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                result.Message = ex.Message;
            }

            return result;
        }

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
        public WorkflowExecutionResult SaveWorkflow(string systemId, string sysAccount, string sysPassword, string username,
                                        string activityInstanceId,  string workflowProperties)
        {
            WorkflowExecutionResult ExecutionResult = new WorkflowExecutionResult();
            ExecutionResult.Success = "-1";
            if (!Validate(systemId, sysAccount, sysPassword, ExecutionResult))
            {
                return ExecutionResult;
            }

            if (string.IsNullOrEmpty(username))
            {
                ExecutionResult.Message = "指定用户的Portal 登录账号 不能为空";
                return ExecutionResult;
            }

            Guid? aiid = Util.XmlAnalysisHelp.ToGuid(activityInstanceId);
            if (!aiid.HasValue)
            {
                ExecutionResult.Message = "活动实例ID（标识）不能为空";
                return ExecutionResult;
            }

            if (string.IsNullOrEmpty(workflowProperties))
            {
                ExecutionResult.Message = "需求单工单处理属性 不能为空";
                return ExecutionResult;
            }

            try
            {
                string manageOpinion = string.Empty;
                ExecutionResult = WorkflowAPIService.SaveWorkflow(username, aiid.Value, workflowProperties, manageOpinion);
                ExecutionResult.Success = "1";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ExecutionResult.Message = ex.Message;
            }

            return ExecutionResult;
        }

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
        public WorkflowExecutionResult AssignWorkflow(string systemId, string sysAccount, string sysPassword, string username, string assignedUser,string activityInstanceId, string Message)
        {
            WorkflowExecutionResult ExecutionResult = new WorkflowExecutionResult();
            ExecutionResult.Success = "-1";

            if (!Validate(systemId, sysAccount, sysPassword, ExecutionResult))
            {
                return ExecutionResult;
            }

            if (string.IsNullOrEmpty(username))
            {
                ExecutionResult.Message = "指定用户的Portal 登录账号 不能为空";
                return ExecutionResult;
            }

            if (string.IsNullOrEmpty(assignedUser))
            {
                ExecutionResult.Message = "被指定用户的Portal 登录账号 不能为空";
                return ExecutionResult;
            }

            Guid? aiid = Util.XmlAnalysisHelp.ToGuid(activityInstanceId);
            if (!aiid.HasValue)
            {
                ExecutionResult.Message = "活动实例ID（标识）不能为空";
                return ExecutionResult;
            }

            try
            {
                WorkflowAPIService.AssignWorkflow(username, assignedUser, activityInstanceId, Message);
                ExecutionResult.Success = "1";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ExecutionResult.Message = ex.Message;
            }

            return ExecutionResult;
        }

        /// <summary>
        /// 高级查询条件
        /// </summary>
        /// <param name="systemId">综合应用平台分配的系统标识号</param>
        /// <param name="sysAccount">接入综合应用平台的账号</param>
        /// <param name="sysPassword">接入综合应用平台的密码</param>
        /// <param name="username">指定用户的Portal 登录账号</param>
        /// <param name="workflowName">流程名</param>
        /// <returns></returns>
        public SearchQueryResult SearchQuery(string systemId, string sysAccount, string sysPassword, string username, string workflowName)
        {
            SearchQueryResult SearcResult = new SearchQueryResult();
            SearcResult.Success = "-1";

            if (!Validate(systemId, sysAccount, sysPassword, SearcResult))
            {
                return SearcResult;
            }

            if (string.IsNullOrEmpty(username))
            {
                SearcResult.Message = "Portal 登录账号 不能为空";
                return SearcResult;
            }

            try
            {
                SearcResult = WorkflowAPIService.SearchQuery(username, workflowName);
                SearcResult.Success = "1";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SearcResult.Message = ex.Message;
            }
            return SearcResult;
        }

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
        public SearchResult Search(string systemId, string sysAccount, string sysPassword, string username, string BeginTime, string EndTime, string workflowName, string activityName, string creator, string actor, string titleKeywords, string contentKeywords, string sheetId)
        {
            SearchResult Result = new SearchResult();
            Result.Success = "-1";

            if (!Validate(systemId, sysAccount, sysPassword, Result))
            {
                return Result;
            }

            if (string.IsNullOrEmpty(username))
            {
                Result.Message = "Portal 登录账号 不能为空";
                return Result;
            }

            try
            {
                DateTime now = DateTime.Now; string BeginTimes = BeginTime, EndTimes = EndTime;
                if (string.IsNullOrEmpty(BeginTime))
                    BeginTimes = now.AddMonths(-1).ToString("yyyy-MM-dd");
                if (string.IsNullOrEmpty(EndTime))
                    EndTimes = now.AddDays(1).ToString("yyyy-MM-dd");
                Result = WorkflowAPIService.Search(username, BeginTimes, EndTimes, workflowName, activityName, creator, actor, titleKeywords, contentKeywords, sheetId);
                Result.Success = "1";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Result.Message = ex.Message;
            }
            return Result;
        }

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
        public CommentResult CommentWorkflow(string systemId, string sysAccount, string sysPassword, string workflowInstanceId, string activityInstanceId, string username, string content, string CommentProperties)
        {
            CommentResult comResult = new CommentResult();
            comResult.Success = "-1";

            if (!Validate(systemId, sysAccount, sysPassword, comResult))
            {
                return comResult;
            }

            if (string.IsNullOrEmpty(username))
            {
                comResult.Message = "Portal 登录账号 不能为空";
                return comResult;
            }
             Guid? wiid = Util.XmlAnalysisHelp.ToGuid(workflowInstanceId);
             if (!wiid.HasValue)
             {
                 comResult.Message = "工单唯一标识 不能为空";
                 return comResult;
             }
            try
            {
                comResult = WorkflowAPIService.CommentWorkflow(workflowInstanceId,activityInstanceId, username, content, CommentProperties);
                comResult.Success = "1";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                comResult.Message = ex.Message;
            }
            return comResult;
        }

        #endregion

        private bool Validate(string systemId, string sysAccount, string sysPassword, ApiResult result)
        {
            //Proxies.InterfaceActionResult value = Proxies.InterfaceManagerHelper.Default.ValiateInterface(systemId, sysAccount, sysPassword);
            //result.AppAuth = value.AppAuth;
            //result.Message = value.ReturnMessage;
            //return value.AppAuth == 0;
            result.AppAuth = 1;
            return true;
        }

        /// <summary>
        /// 计算滞留时间
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string CalculateResortTime(string obj)
        {
            if (string.IsNullOrEmpty(obj)) return null;

            DateTime dtSource;
            try
            {
                dtSource = DateTime.Parse(obj);
                return Botwave.Commons.DateTimeUtils.ResolveInterval(DateTime.Now, dtSource);
            }
            catch { }

            return null;
        }
    }
}
