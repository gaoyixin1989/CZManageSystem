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
using Botwave.Security.Domain;
using Botwave.Security.Service;
using Botwave.GMCCServiceHelpers;
using Botwave.XQP.Domain;

namespace Botwave.XQP.Service.Plugins
{
    public class PostActivityExecutionHandler : IPostActivityExecutionHandler
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(PostActivityExecutionHandler));
        private SMSProfile smsProfile = SMSProfile.GetProfile();

        #region properties

        private IActivityService activityService;
        private IWorkflowService workflowService;
        private IUserService userService;
        private IWorkflowNotifyExtendService workflowNotifyExtendService;

        public IActivityService ActivityService
        {
            get { return activityService; }
            set { activityService = value; }
        }

        public IWorkflowService WorkflowService
        {
            get { return workflowService; }
            set { workflowService = value; }
        }

        public IUserService UserService
        {
            get { return userService; }
            set { userService = value; }
        }

        public IWorkflowNotifyExtendService WorkflowNotifyExtendService
        {
            get { return workflowNotifyExtendService; }
            set { workflowNotifyExtendService = value; }
        }
        #endregion

        #region IPostActivityExecutionHandler Members

        private IPostActivityExecutionHandler next = null;

        public IPostActivityExecutionHandler Next
        {
            get { return next; }
            set { next = value; }
        }

        public void Execute(ActivityExecutionContext context)
        {
            Guid activityInstanceId = context.ActivityInstanceId;
            string command = context.Command.ToLower();
            log.WarnFormat("[ActivityInstanceId:{0}] execute IActivityExecutionHandler, the command is {1}", activityInstanceId, command);
            
            if (command.Equals(ActivityCommands.Complete))
            {
                try
                {
                    WorkflowInstance workflowInstance = workflowService.GetWorkflowInstanceByActivityInstanceId(activityInstanceId);
                    SZIntelligentRemind.TimerInfoDelete(workflowInstance.WorkflowInstanceId.ToString());
                }
                catch (Exception ex)
                {
                    log.Error(ex.ToString());
                }
            }
            if (command.Equals(ActivityCommands.ReturnToDraft))
            {
                //删除相关消息通知、待办待阅                
                WorkflowPostHelper.PostCancelWorkflowInstance(activityInstanceId);

                //发消息通知给发起人
                WorkflowInstance workflowInstance = workflowService.GetWorkflowInstanceByActivityInstanceId(activityInstanceId);
                string content = smsProfile.ActivityRejectMessage.ToLower().Replace("#title#", workflowInstance.Title);
                string creatorName = workflowInstance.Creator;

                //待办状态异常处理
                updateTodoState(workflowInstance.WorkflowInstanceId);

                UserInfo creator = userService.GetUserByUserName(creatorName);
                //AsynNotifyHelper.SendSMS(creator.Mobile, content);
                //AsynNotifyHelper.SendEmail(creator.Email, content);
                int notifyType = WorkflowNotify.GetNotifyTypeByWorkflow(creatorName, workflowInstance.WorkflowId);
                if (notifyType == 1 || notifyType == 3)
                {
                    Botwave.GMCCServiceHelpers.CZ.AsynNotifyHelper.SendSMS(creatorName, content);
                }
                if (notifyType == 1 || notifyType == 2)
                {
                    Botwave.GMCCServiceHelpers.CZ.AsynNotifyHelper.SendEmail(creatorName, content);
                }

                string sql = String.Format(@"select top 1 ActivityInstanceId from bwwf_Tracking_Activities where WorkflowInstanceId = '{0}'", workflowInstance.WorkflowInstanceId);
                object obj = IBatisDbHelper.ExecuteScalar(CommandType.Text, sql);
                if (null != obj)
                {
                    string entityId = obj.ToString();
                    string title = workflowInstance.Title;
                    string url =  WorkflowPostHelper.TransformUrlByActivityInstanceId(entityId);
                    AsynExtendedPendingJobHelper.AddPendingJob(creatorName, context.Actor, title, url, ActivityInstance.EntityType, entityId);
                }
            }
            else if (command.Equals(ActivityCommands.Withdraw))
            {
                //删除相关消息通知、待办待阅


                //删除流程相关活动实例、系统TODO
                WorkflowPostHelper.PostCancelWorkflowInstance(activityInstanceId);

                WorkflowInstance workflowInstance = workflowService.GetWorkflowInstanceByActivityInstanceId(activityInstanceId);
                if (workflowInstance != null)
                {
                    string wid = workflowInstance.WorkflowInstanceId.ToString();
                    string sql = String.Format(@"delete from bwwf_Tracking_Todo where ActivityInstanceId in 
(select ActivityInstanceId from bwwf_Tracking_Activities where WorkflowInstanceId = '{0}');
delete from bwwf_Tracking_Activities where WorkflowInstanceId = '{0}';
delete from bwwf_Tracking_Activities_Completed where WorkflowInstanceId = '{0}';", wid);
                    IBatisDbHelper.ExecuteNonQuery(System.Data.CommandType.Text, sql);

                    //待办状态异常处理
                    updateTodoState(workflowInstance.WorkflowInstanceId);
                }
                try
                {
                    SZIntelligentRemind.TimerInfoDelete(workflowInstance.WorkflowInstanceId.ToString());
                }
                catch (Exception ex)
                {
                    log.Error(ex.ToString());
                }
            }
            else
            {
                WorkflowPostHelper.PostCloseActivityInstance(activityInstanceId);

                WorkflowInstance workflowInstance = workflowService.GetWorkflowInstanceByActivityInstanceId(activityInstanceId);

                if (command.Equals(ActivityCommands.Reject))
                {
                    string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    IList<WorkflowNotifyActor> notifyActors = workflowNotifyExtendService.GetNextNotifyActors(activityInstanceId);
                    foreach (WorkflowNotifyActor actorItem in notifyActors)
                    {
                        log.Debug(actorItem.UserName);

                        string entityId = actorItem.ActivityInstanceId.ToString();
                        string title = actorItem.WorkItemTitle;
                        string url = WorkflowPostHelper.TransformUrlByActivityInstanceId(entityId);
                        AsynExtendedPendingJobHelper.AddPendingJob(actorItem.UserName, context.Actor, title, url, ActivityInstance.EntityType, entityId);
                    }

                    if (workflowInstance != null)
                    {
                        //待办状态异常处理
                        updateTodoState(workflowInstance.WorkflowInstanceId);
                    }
                }
                else
                {
                    // 获取下一步骤列表.
                    IList<ActivityInstance> activities = activityService.GetNextActivities(activityInstanceId);
                    if (activities == null || activities.Count == 0)
                    {
                        log.Warn("execute IActivityExecutionHandler. next activities is null.");
                    }
                    else
                    {
                        foreach (ActivityInstance item in activities)
                        {
                            AddActivityPendings(context.Actor, item);
                        }
                    }
                    if (command.Equals(ActivityCommands.Approve))
                    {
                        if (workflowInstance != null)
                        {
                            //待办状态异常处理
                            updateTodoState(workflowInstance.WorkflowInstanceId);
                        }
                    }
                }
            }

        }

        #endregion

        private static void AddActivityPendings(string parentOwner, ActivityInstance activity)
        {
            Guid activityInstanceId = activity.ActivityInstanceId;
            string entityId = activityInstanceId.ToString();
            log.WarnFormat("[ActivityInstanceId:{0}]execute IActivityExecutionHandler ... pending", activityInstanceId);

            IList<ActorInfo> actors = IBatisMapper.Select<ActorInfo>("bwwf_Todo_Select_ActivityActors", activityInstanceId); // 选择步骤执行人


            if (actors == null || actors.Count == 0)
            {
                log.WarnFormat("[ActivityInstanceId:{0}]The actors of activity is 0.", activityInstanceId);
                return;
            }

            string title = activity.WorkItemTitle;
            string url = WorkflowPostHelper.TransformUrlByActivityInstanceId(entityId);
            string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            foreach (ActorInfo actor in actors)
            {
                AsynExtendedPendingJobHelper.AddPendingJob(actor.UserName, parentOwner, title, url, ActivityInstance.EntityType, entityId);
                //委托授权且非完全授权时需用待阅信息通知委托人


                if (!String.IsNullOrEmpty(actor.ProxyName))
                {
                    string sql = String.Format(@"select a.IsFullAuthorized from bw_Authorizations as a left join bw_Users as u on a.FromUserId = u.UserId 
where a.BeginTime <= '{0}' and a.EndTime >= '{0}' and u.UserName = '{1}'", now, actor.UserName);
                    object obj = IBatisDbHelper.ExecuteScalar(System.Data.CommandType.Text, sql);
                    bool isFullAuthorized = (null == obj) ? true : Convert.ToBoolean(obj);
                    if (!isFullAuthorized)
                    {
                        AsynExtendedPendingJobHelper.AddPendingMsg(actor.ProxyName, parentOwner, title, url, ActivityInstance.EntityType, entityId);
                    }
                }
            }
        }

        /// <summary>
        /// 处理待办异常，如果待办产生后又被禁用
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        private void updateTodoState(Guid workflowInstanceId)
        {
            try
            {
                string sql = string.Format("select count(0) from bwwf_tracking_activities where workflowinstanceid = '{0}'", workflowInstanceId);
                object obj = IBatisDbHelper.ExecuteScalar(CommandType.Text, sql);
                if (Botwave.Commons.DbUtils.ToInt32(obj, 0) == 1)
                {
                    sql = string.Format(@"select count(0) from bwwf_tracking_todo where activityinstanceid =(
select activityinstanceid from bwwf_tracking_activities where workflowinstanceid = '{0}')", workflowInstanceId);
                    obj = IBatisDbHelper.ExecuteScalar(CommandType.Text, sql);
                    if (Botwave.Commons.DbUtils.ToInt32(obj, 0) == 1)
                    {
                        sql = string.Format(@"select state from bwwf_tracking_todo where activityinstanceid =(
select activityinstanceid from bwwf_tracking_activities where workflowinstanceid = '{0}')", workflowInstanceId);
                        obj = IBatisDbHelper.ExecuteScalar(CommandType.Text, sql);
                        if (Botwave.Commons.DbUtils.ToInt32(obj, 0) == 2)
                        {
                            log.Info("[" + workflowInstanceId + "]对应的待办异常，执行待办更新操作。。。");
                            sql = string.Format(@"update bwwf_tracking_todo set state = 0 where activityinstanceid =(
select activityinstanceid from bwwf_tracking_activities where workflowinstanceid = '{0}')", workflowInstanceId);
                            IBatisDbHelper.ExecuteNonQuery(CommandType.Text, sql);
                            log.Info("[" + workflowInstanceId + "]对应的待办异常，执行待办更新操作完毕。。。");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Errors in [" + workflowInstanceId + "]updateTodoState："+ex.ToString());
            }
        }
    }
}
