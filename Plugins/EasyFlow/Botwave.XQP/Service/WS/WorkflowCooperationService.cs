using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Plugin;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;
using Botwave.Security;
using Botwave.Security.Domain;
using Botwave.Security.Service;
using Botwave.XQP.Domain;

namespace Botwave.XQP.Service.WS
{
    public class WorkflowCooperationService : IWorkflowCooperation
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(WorkflowCooperationService));

        /// <summary>
        /// 短信内容配置对象,
        /// </summary>
        private SMSProfile smsProfile = SMSProfile.GetProfile();
        /// <summary>
        /// 对同一工单步骤最大审核测试(即审核重试次数).
        /// </summary>
        private const int MaxReceivedCout = 5;

        #region getter/setter

        private IAuthenticator authenticator;
        private IActivityService activityService;
        private IFormInstanceService formInstanceService;
        private ITaskAssignService taskAssignService;
        private IWorkflowEngine workflowEngine;
        private IWorkflowService workflowService;
        private IActivityDefinitionService activityDefinitionService;
        private IFormDefinitionService formDefinitionService;
        private IWorkflowDefinitionService workflowDefinitionService;
        private IUserService userService;
        private IRoleService roleService;
        private IWorkflowNotifyService workflowNotifyService;
        private IPostActivityExecutionHandler postActivityExecutionMessageHandler;

        private IAuthenticator Authenticator
        {
            set { authenticator = value; }
        }
        public IActivityService ActivityService
        {
            set { activityService = value; }
        }
        public IFormInstanceService FormInstanceService
        {
            set { formInstanceService = value; }
        }
        public ITaskAssignService TaskAssignService
        {
            set { taskAssignService = value; }
        }
        public IWorkflowEngine WorkflowEngine
        {
            set { workflowEngine = value; }
        }
        public IWorkflowService WorkflowService
        {
            set { workflowService = value; }
        }
        public IActivityDefinitionService ActivityDefinitionService
        {
            set { activityDefinitionService = value; }
        }
        public IFormDefinitionService FormDefinitionService
        {
            set { formDefinitionService = value; }
        }
        public IWorkflowDefinitionService WorkflowDefinitionService
        {
            set { workflowDefinitionService = value; }
        }
        public IUserService UserService
        {
            set { userService = value; }
        }

        public IRoleService RoleService
        {
            set { roleService = value; }
        }

        public IWorkflowNotifyService WorkflowNotifyService
        {
            set { workflowNotifyService = value; }
        }

        public IPostActivityExecutionHandler PostActivityExecutionMessageHandler
        {
            set { postActivityExecutionMessageHandler = value; }
        }
        #endregion

        #region IWorkflowCooperation Members

        /// <summary>
        /// 执行流程活动.
        /// </summary>
        /// <param name="sysAccount">系统接入帐号</param>
        /// <param name="sysPassword">系统接入密码</param>
        /// <param name="activityInstanceId">待执行活动实例Id</param>
        /// <param name="actor">处理人用户名.</param>
        /// <param name="content">短信内容.</param>
        /// <returns></returns>
        public ActionResult ExecuteActivity(string sysAccount, string sysPassword, Guid activityInstanceId, string actor, string content)
        {
            ActionResult result = new ActionResult();
            result.ReturnValue = true;
            try
            {
                result.AppAuth = authenticator.Authenticate(sysAccount, sysPassword);
                if (AppAuthConstants.IsValid(result.AppAuth))
                {
                    log.InfoFormat("[{0}] execute me to execute activity: {1}/{2}/{3}", sysAccount, activityInstanceId, actor, content);

                    int count = GetReceivedMessageCount(activityInstanceId, actor);
                    // 判断审批次数是否超过限制.
                    if (count >= MaxReceivedCout)
                    {
                        log.InfoFormat("over the max received count: {0}", count);

                        this.PostExecuteActivityOnReceivedOut(activityInstanceId, actor, count);
                        result.ReturnValue = false;
                        result.ReturnMessage = "短信审批请求次数超过系统允许的最大次数.";
                        return result;
                    }

                    content = (content == null ? string.Empty : content.Trim());
                    //string commandValue = (content.Length > 0 ? content.Substring(0, 1) : string.Empty);
                    string commandValue = (content.Length > 0 ? content : string.Empty);
                    if (commandValue == "0")//|| commandValue == "1"
                    {
                        string command = ActivityCommands.Reject; //(commandValue == "1" ? ActivityCommands.Approve : ActivityCommands.Reject);
                        this.BeginExecuteActivity(activityInstanceId, actor, command, content, null);
                    }
                    else
                    {
                        ActivityInstance activityInstance = activityService.GetActivity(activityInstanceId);
                        if (activityInstance != null)
                        {
                            DataTable dt = GetNextActivitys(activityInstance);
                            if (int.Parse(commandValue) > dt.Rows.Count)
                            {
                                this.PostExecuteActivityOnInvalid(activityInstanceId, actor, count);
                                result.ReturnValue = false;
                                result.ReturnMessage = "短信消息内容格式不正确.";
                                return result;
                            }
                            string command = ActivityCommands.Approve;
                            if (dt.Rows.Count == 1)//只有一条的情况下  默认执行同意
                            {
                                this.BeginExecuteActivity(activityInstanceId, actor, command, content, null);
                            }
                            else
                            {
                                string ActivityId = dt.Rows[int.Parse(commandValue) - 1]["ActivityId"].ToString();
                                this.BeginExecuteActivity(activityInstanceId, actor, command, content, ActivityId);
                            }
                        }
                        else
                        {
                            log.InfoFormat("{0} 无法找到对象", activityInstanceId);
                        }
                    }
                }
                else
                {
                    ActionResultHelper.SetLoginErrorInfo(sysAccount, sysPassword, result);
                }
            }
            catch (Exception ex)
            {
                ActionResultHelper.HandleException(result, ex, log);
            }
            return result;
        }

        /// <summary>
        /// 拷贝流程实例.
        ///     应用于人力流程每月自动创建工单.
        /// </summary>
        /// <param name="sysAccount">系统接入帐号</param>
        /// <param name="sysPassword">系统接入密码</param>
        /// <param name="workflowInstanceId">流程实例Id</param>
        /// <returns></returns>
        public ActionResult CopyWorkflowInstance(string sysAccount, string sysPassword, Guid workflowInstanceId)
        {
            ActionResult result = new ActionResult();
            result.ReturnValue = true;
            try
            {
                result.AppAuth = authenticator.Authenticate(sysAccount, sysPassword);
                if (AppAuthConstants.IsValid(result.AppAuth))
                {
                    log.InfoFormat("now {0}/{1} execute me to execute activity", sysAccount, sysPassword);
                    if (null != workflowInstanceId)
                    {
                        log.InfoFormat("the workflowInstanceId is [{0}]", workflowInstanceId);
                    }
                    else
                    {
                        log.Info("the activityInstanceId is null");
                    }

                    BeginCopyWorkflowInstance(workflowInstanceId);
                }
                else
                {
                    ActionResultHelper.SetLoginErrorInfo(sysAccount, sysPassword, result);
                }
            }
            catch (Exception ex)
            {
                ActionResultHelper.HandleException(result, ex, log);
            }
            return result;
        }
      
        #endregion

        DataTable GetNextActivitys(ActivityInstance activityInstance)
        {
            string strSql = string.Format(@"select a.ActivityId,a.ActivityName,SortOrder from dbo.bwwf_Activities a inner join 
(
	select ACS.* from dbo.bwwf_ActivitySet ACS
	inner join dbo.bwwf_Activities AC
	on AC.NextActivitySetId=ACS.setid and AC.ActivityId='{0}'
)
b
on a.ActivityId=b.ActivityId order by sortorder", activityInstance.ActivityId.ToString());
            DataTable dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, strSql).Tables[0];
            
            return dt;
        }

        #region Methods

        /// <summary>
        /// 执行流程活动.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <param name="actor"></param>
        /// <param name="command">approve/reject.</param>
        /// <param name="comment"></param>
        private void BeginExecuteActivity(Guid activityInstanceId, string actor, string command, string comment, string ActivityId)
        {
            if (string.IsNullOrEmpty(actor))
            {
                IList<Botwave.Entities.BasicUser> actorInfo = taskAssignService.GetUsers4Assignment(activityInstanceId);
                actor = actorInfo.Count > 0 ? actorInfo[0].UserName : "";
                if (string.IsNullOrEmpty(actor))
                {
                    log.Info("短信审批失败：未找到处理人。ActivityInstanceId: " + activityInstanceId);
                    return;
                }
            }

            command = (command == null ? string.Empty : command.Trim().ToLower());
            command = (command.Equals(ActivityCommands.Reject) ? ActivityCommands.Reject : ActivityCommands.Approve);

            if (string.IsNullOrEmpty(comment))
            {
            }
            string reason = (command.Equals(ActivityCommands.Approve) ? "同意" : "不同意");

            ActivityInstance activityInstance = activityService.GetActivity(activityInstanceId);
            IList<FormItemInstance> itemInstanceList = formInstanceService.GetFormItemInstancesByFormInstanceId(activityInstance.WorkflowInstanceId, true);
            IDictionary<string, object> variables = new Dictionary<string, object>();
            foreach (FormItemInstance item in itemInstanceList)
            {
                if (!variables.ContainsKey(item.Definition.FName))
                    variables.Add(item.Definition.FName, item.Definition.ItemDataType == FormItemDefinition.DataType.Text ? item.TextValue : item.Value);
            }

            ActivityExecutionContext context = new ActivityExecutionContext();
            context.ActivityInstanceId = activityInstanceId;
            context.Actor = actor;
            context.Command = command;
            context.Reason = reason;
            
            context.Variables = variables;
            #region 选择步骤

            if (!string.IsNullOrEmpty(ActivityId))
            {
                IDictionary<Guid, IDictionary<string, string>> dicts = new Dictionary<Guid, IDictionary<string, string>>();
                IDictionary<string, string> names = new Dictionary<string, string>();
                names.Add(actor, "");
                dicts.Add(new Guid(ActivityId), names);
                context.ActivityAllocatees = dicts;
            }

            #endregion

            try
            {
                workflowEngine.ExecuteActivity(context);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.PostExecuteActivityOnFeedback(actor, activityInstanceId, false);
                throw ex;
            }

            this.PostExecuteActivityOnFeedback(actor, activityInstanceId, true);
            this.PostExecuteActivity(context);
        }

        /// <summary>
        /// 复制流程实例.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        private void BeginCopyWorkflowInstance(Guid workflowInstanceId)
        {
            //获取拷贝对象流程实例
            WorkflowInstance workflowInstance = workflowService.GetWorkflowInstance(workflowInstanceId);

            if (null == workflowInstance)
            {
                log.Info("the workflowInstance is null");
            }

            //创建流程实例
            Guid newworkflowInstanceId = Guid.NewGuid();
            workflowInstance.WorkflowInstanceId = newworkflowInstanceId;
            workflowInstance.State = 1;
            workflowService.InsertWorkflowInstance(workflowInstance);

            //创建表单实例
            FormDefinition definition = formDefinitionService.GetFormDefinitionByExternalEntity("Form_Workflow", workflowInstance.WorkflowId);
            formInstanceService.CreateFormInstance(newworkflowInstanceId, definition.Id, workflowInstance.Creator);

            IList<FormItemInstance> itemDataList = formInstanceService.GetFormItemInstancesByFormInstanceId(workflowInstanceId, true);
            IList<FormItemInstance> itemNewList = formInstanceService.GetFormItemInstancesByFormInstanceId(newworkflowInstanceId, true);
            IDictionary<string, object> variables = new Dictionary<string, object>();
            foreach (FormItemInstance itemData in itemDataList)
            {
                foreach (FormItemInstance itemNew in itemNewList)
                {
                    if (itemData.Definition.Id.Equals(itemNew.Definition.Id))
                    {
                        if (itemNew.Definition.ItemDataType == FormItemDefinition.DataType.Text)
                            itemNew.TextValue = itemData.TextValue;
                        else itemNew.Value = itemData.Value;
                        break;
                    }
                }
                if (!variables.ContainsKey(itemData.Definition.FName))
                    variables.Add(itemData.Definition.FName, itemData.Definition.ItemDataType == FormItemDefinition.DataType.Text ? itemData.TextValue : itemData.Value);
            }
            formInstanceService.UpdateFormItemInstance(itemNewList);

            //创建活动实例
            string actor = null;
            IList<ActivityInstance> activityInstanceList = activityService.GetWorkflowActivities(workflowInstanceId);
            foreach (ActivityInstance instance in activityInstanceList)
            {
                actor = instance.Actor.Length > 0 ? instance.Actor.Substring(0, instance.Actor.IndexOf("/")) : "";
                instance.ActivityInstanceId = Guid.NewGuid();
                instance.WorkflowInstanceId = newworkflowInstanceId;
                instance.Actor = actor;

                //保存人事数据管理员上一步骤之前的活动实例至已完成活动实例表
                ActivityDefinition activityDefinition = activityDefinitionService.GetActivityDefinition(instance.ActivityId);
                if (activityDefinition.SortOrder < 3)
                {
                    IBatisMapper.Insert("bwwf_ActivityInstance_Completed_Insert", instance);
                }
                //保存人事数据管理员上一步骤的活动实例至已活动实例表，并执行活动
                else if (activityDefinition.SortOrder == 4 || activityDefinition.SortOrder == 5 || activityInstanceList.Count == 6)
                {
                    instance.IsCompleted = false;
                    instance.Actor = null;
                    IBatisMapper.Insert("bwwf_ActivityInstance_Insert", instance);
                    ActivityExecutionContext context = new ActivityExecutionContext();
                    context.ActivityInstanceId = instance.ActivityInstanceId;
                    context.Actor = actor;
                    context.Command = "approve";
                    context.Reason = "同意";
                    context.Variables = variables;

                    workflowEngine.ExecuteActivity(context);
                    break;
                }
                else
                    IBatisMapper.Insert("bwwf_ActivityInstance_Completed_Insert", instance);
            }
        }

        /// <summary>
        /// 执行流程活动实例的后续处理.
        /// </summary>
        /// <param name="context"></param>
        private void PostExecuteActivity(ActivityExecutionContext context)
        {
            if (null != postActivityExecutionMessageHandler)
            {
                postActivityExecutionMessageHandler.Execute(context);
                IPostActivityExecutionHandler next = postActivityExecutionMessageHandler.Next;
                while (next != null)
                {
                    next.Execute(context);
                    next = next.Next;
                }
            }
        }

        /// <summary>
        /// 短信消息内容格式不正确时的处理.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <param name="actor"></param>
        /// <param name="receviedCount"></param>
        private void PostExecuteActivityOnInvalid(Guid activityInstanceId, string actor, int receviedCount)
        {
            if (string.IsNullOrEmpty(actor) || receviedCount > 5)
                return;

            string content = smsProfile.ReceiveInvalidMessage;
            Botwave.GMCCServiceHelpers.CZ.AsynNotifyHelper.SendMessage(4, actor, string.Empty, content, ActivityInstance.EntityType, activityInstanceId.ToString());
        }

        /// <summary>
        /// 发送短信审批超出次数的短信提醒.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <param name="actor"></param>
        /// <param name="receviedCount"></param>
        private void PostExecuteActivityOnReceivedOut(Guid activityInstanceId, string actor, int receviedCount)
        {
            if (string.IsNullOrEmpty(actor) || receviedCount != 5)
                return;
            string content = smsProfile.LastReceiveInvalidMessage;
            Botwave.GMCCServiceHelpers.CZ.AsynNotifyHelper.SendMessage(2, actor, string.Empty, content, ActivityInstance.EntityType, activityInstanceId.ToString());
        }

        /// <summary>
        /// 发送短信审批的反馈消息通知.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="activityInstanceId"></param>
        /// <param name="isSuccess"></param>
        private void PostExecuteActivityOnFeedback(string actor, Guid activityInstanceId, bool isSuccess)
        {
            if (string.IsNullOrEmpty(actor) || activityInstanceId == Guid.Empty)
                return;

            string content = (isSuccess ? smsProfile.FeedbackSuccessMessage : smsProfile.FeedbackErrorMessage);

            if (isSuccess && content.IndexOf("#nextactivities#", StringComparison.OrdinalIgnoreCase) > -1)
            {
                bool hasCompleted = false;
                string nextValues = string.Empty;
                // 下行步骤信息.
                IList<string> activityNames = GetNextActivityNames(activityInstanceId, out hasCompleted);
                string activities = string.Empty;
                foreach (string name in activityNames)
                    activities += string.Format(",{0}", name);
                if (activities.Length > 0)
                {
                    activities = activities.Remove(0, 1);
                    nextValues += string.Format("下一步骤：{0}。", activities);
                }
                string users = string.Empty;
                if (!hasCompleted)
                {
                    IList<string> actors = GetNextActors(activityInstanceId);

                    foreach (string name in actors)
                        users += string.Format(",{0}", name.Trim());
                    if (users.Length > 1)
                    {
                        users = users.Remove(0, 1);
                        if (users.Length > 0)
                            nextValues += string.Format("下一步骤处理人：{0}。", users);
                    }
                }
                content = content.ToLower().Replace("#nextactivities#", nextValues);
            }
            
            Botwave.GMCCServiceHelpers.CZ.AsynNotifyHelper.SendSMS(actor, string.Empty, content, ActivityInstance.EntityType, activityInstanceId.ToString());
        }

        /// <summary>
        /// 下行步骤处理人.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        private IList<string> GetNextActors(Guid activityInstanceId)
        {
            IList<NotifyActor> actors = workflowNotifyService.GetNextNotifyActors(activityInstanceId);
            IList<string> results = new List<string>();
            foreach (NotifyActor actor in actors)
            {
                if (!results.Contains(actor.RealName))
                    results.Add(actor.RealName);
            }
            return results;
        }

        /// <summary>
        /// 下行步骤名称.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <param name="hasCompleted"></param>
        /// <returns></returns>
        private static IList<string> GetNextActivityNames(Guid activityInstanceId, out bool hasCompleted)
        {
            hasCompleted = false;
            if (activityInstanceId == Guid.Empty)
                return new List<string>();
            string sql = @"SELECT distinct a.ActivityName, a.State FROM vw_bwwf_Tracking_Activities_All ta
	                                    LEFT JOIN bwwf_Activities a ON a.ActivityId = ta.ActivityID
                                    WHERE ta.PrevSetId IN (
	                                    SELECT SetID FROM bwwf_Tracking_Activities_Set WHERE ActivityInstanceId = '{0}'
                                    )";
            DataTable resultTable = IBatisDbHelper.ExecuteDataset(CommandType.Text, string.Format(sql, activityInstanceId)).Tables[0];
            IList<string> results = new List<string>();
            foreach (DataRow row in resultTable.Rows)
            {
                string activityName = DbUtils.ToString(row["ActivityName"]);
                int state = DbUtils.ToInt32(row["State"]);
                if (!hasCompleted && state == 2)
                {
                    hasCompleted = (resultTable.Rows.Count == 1);
                }
                results.Add(activityName);
            }
            return results;
        }

        /// <summary>
        /// 获取指定用户在工单指定步骤发送过的短信审批数.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <param name="actor"></param>
        /// <returns></returns>
        private static int GetReceivedMessageCount(Guid activityInstanceId, string actor)
        {
            string sql = "SELECT COUNT(0) FROM vw_cz_ReceivedSMS_Detail WHERE ([State] = 1 OR [State] = -1) AND (Receiver = '{0}') AND (EntityID = '{1}')";
            object result = IBatisDbHelper.ExecuteScalar(CommandType.Text, string.Format(sql, actor, activityInstanceId));
            if (result == null)
                return 0;
            return DbUtils.ToInt32(result);
        }
        #endregion
    }
}
