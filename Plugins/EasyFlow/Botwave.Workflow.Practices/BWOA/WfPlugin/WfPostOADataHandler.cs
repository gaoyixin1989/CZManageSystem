using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Botwave.Workflow.Plugin;
using Botwave.Workflow.Service;
using Botwave.Workflow.Domain;
using Botwave.Workflow;
using Botwave.Workflow.Practices.BWOA.Service;
using Botwave.Workflow.Practices.BWOA.Domain;
using Botwave.DynamicForm.Services;
using Botwave.DynamicForm;
using Botwave.Workflow.Extension.Service;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Practices.BWOA.Service.Impl;
using Botwave.Extension.IBatisNet;
using Botwave.XQP.Service.Plugins;
using Botwave.WebServiceClients;
using System.Configuration;
using Botwave.Security.Domain;
using Botwave.Security.Service;
using Botwave.XQP.Domain;
using Botwave.Workflow.Extension.Service.Support;
using Botwave.XQP.Service.Support;

namespace Botwave.Workflow.Practices.BWOA.WfPlugin
{
    public class WfPostOADataHandler :  IPostActivityExecutionHandler
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(WfPostOADataHandler));

        private IActivityService activityService;
        private IWorkflowService workflowService;
       // private IWorkflowExecutionService workflowExecutionService;
        private IBatisActivityExecutionService batisActivityExecutionService;
        private IActivityDefinitionService activityDefinitionService;
        private IWorkflowNotifyService workflowNotifyService;
        //private DefaultWorkflowUserService workflowUserService;
        //private WorkflowNotifyExtendService workflowNotifyExtendService;
        //private IFormDefinitionService formDefinitionService;
        private IFormInstanceService formInstanceService;
        private IWorkflowDefinitionService workflowDefinitionService;
        private IUserService userService;



        public IActivityService ActivityService
        {            set { activityService = value; }
        }
        public IWorkflowService WorkflowService
        {
            set { workflowService = value; }
        }

        //public IWorkflowExecutionService WorkflowExecutionService
        //{
        //    set { workflowExecutionService = value; }
        //}
        public IActivityDefinitionService ActivityDefinitionService
        {
            set { activityDefinitionService = value; }
        }
        public IBatisActivityExecutionService BatisActivityExecutionService
        {
            set { batisActivityExecutionService = value; }
        }
        public IWorkflowNotifyService WorkflowNotifyService
        {
            set { workflowNotifyService = value; }
        }

        //public WorkflowNotifyExtendService WorkflowNotifyExtendService
        //{
        //    set { workflowNotifyExtendService = value; }
        //}

        //public DefaultWorkflowUserService WorkflowUserService
        //{
        //    set { workflowUserService = value; }
        //}


        public IFormInstanceService FormInstanceService
        {
            set { formInstanceService = value; }
        }
        //public IFormDefinitionService FormDefinitionService
        //{
        //    set { formDefinitionService = value; }
        //}

        public IWorkflowDefinitionService WorkflowDefinitionService
        {
            set { workflowDefinitionService = value; }
        }

        public IUserService UserService
        {
            set { userService = value; }
        }

        private WorkflowDefinition wd;
        public WorkflowDefinition Wd
        {
            get { return wd; }
            set { wd = value; }
        }

        private ActivityDefinition activityDefinition;

        #region IPostActivityExecutionHandler Members

        private IPostActivityExecutionHandler next = null;
        public IPostActivityExecutionHandler Next
        {
            get { return next; }
            set { next = value; }
        }

        public void Execute(ActivityExecutionContext context)
        {
            ActivityInstance activityInstance = activityService.GetActivity(context.ActivityInstanceId);
            WorkflowInstance workflowInstance = workflowService.GetWorkflowInstanceByActivityInstanceId(context.ActivityInstanceId);
            wd = workflowDefinitionService.GetWorkflowDefinition(workflowInstance.WorkflowId);
            activityDefinition = activityDefinitionService.GetActivityDefinition(activityInstance.ActivityId);
           
            if (wd.WorkflowName == "日常报销")
            {
                if (activityDefinition.ActivityName == "报销申请")
                {
                    if (activityInstance.OperateType != 1
                        && context.Command != ActivityCommands.Withdraw)
                    {
                        context.Variables["txtExpenseNO"] = workflowInstance.SheetId;
                        string NO = "编号：" + DateTime.Now.Year.ToString() + workflowInstance.SheetId.Substring(4, 7);
                        context.Variables["F1"] = context.Variables["F1"].ToString().Replace("编号：", NO);
                    }
                    SaveForms(context, workflowInstance.WorkflowInstanceId, workflowInstance.Creator);
                }               
            }
            if (wd.WorkflowName == "采购申请")
            {
                if (activityDefinition.ActivityName == "财务付款")
                {
                    //发送待阅消息给签发支票的用户

                    IList<UserInfo> notifyActors = batisActivityExecutionService.GetNotifyActors(workflowInstance.WorkflowId);
                    if (notifyActors.Count > 0)
                    {
                        for (int i = 0; i < notifyActors.Count; i++)
                        {
                            ActivityInstance ai = new ActivityInstance();
                            ai.WorkflowInstanceId = activityInstance.WorkflowInstanceId;
                            ai.ActivityInstanceId = Guid.NewGuid();
                            ai.IsCompleted = true;
                            ai.OperateType = 0;
                            ai.CreatedTime = DateTime.Now;
                            ai.Command = "approve";
                            ai.Reason = "同意";

                            ai.Actor = notifyActors[i].UserName;
                            ai.ActorDescription = notifyActors[i].RealName;

                            WfPostOAPendingHandler pending = new WfPostOAPendingHandler();
                            Guid actid = pending.SelectActivitiesByActivityId(workflowInstance.WorkflowId);
                            ai.ActivityId = actid;
                            pending.InsertOAActivityInstance_Completed(ai);

                            SendCreatorMessage(workflowInstance.WorkflowId, context, notifyActors[i].UserName);
                        }
                    }
                }
            }

            if (workflowInstance.State == WorkflowConstants.Complete)
            {

                batisActivityExecutionService.SaveFlowWaterAccount(workflowInstance, context);
                if (wd.WorkflowName == "请假申请流程" || wd.WorkflowName == "加班申请流程")
                {
                    batisActivityExecutionService.SaveFlowLeaveWaterAccount(workflowInstance.Creator, workflowInstance.WorkflowId, workflowInstance.WorkflowInstanceId, context);
                    //获取前台员工的英文名
                    string userName = System.Configuration.ConfigurationManager.AppSettings["userName"];
                    //请假加班通过后发消息通知前台
                    SendCreatorMessage(workflowInstance.WorkflowId, context, userName);
                    //保存到uhrp中

                    SaveToUhrp(workflowInstance.Creator, context);
                }
                else if (wd.WorkflowName == "采购申请")
                {
                    batisActivityExecutionService.SavePurchaseWaterAccount(workflowInstance.WorkflowId, workflowInstance.WorkflowInstanceId, context);
                    
                }
                else  if (wd.WorkflowName == "系统部署流程")
                {
                    batisActivityExecutionService.SaveSysDeployWaterAccount(workflowInstance.WorkflowId, workflowInstance.WorkflowInstanceId, workflowInstance.Title, context);
                }
                else  if (wd.WorkflowName == "文档验收流程")
                {
                    batisActivityExecutionService.SaveDocInspectWaterAccount(workflowInstance.WorkflowId, workflowInstance.WorkflowInstanceId, workflowInstance.Title, context);
                }
               SendCreatorMessage(workflowInstance.WorkflowId, context, workflowInstance.Creator);
            }
            else if (context.Command != ActivityCommands.Withdraw)
            {
                if (wd.WorkflowName == "加班申请流程")
                {
                    //PM加班申请，且自动连续审批，则把记录推送到uhrp中

                    IList<NotifyActor> notifyActors = workflowNotifyService.GetNextNotifyActors(context.ActivityInstanceId);
                    if (notifyActors.Count == 0)
                    {
                        //获取前台员工的英文名
                        string userName = System.Configuration.ConfigurationManager.AppSettings["userName"];
                        //请假加班通过后发消息通知前台
                        SendCreatorMessage(workflowInstance.WorkflowId, context, userName);
                        //保存到uhrp中

                        SaveToUhrp(workflowInstance.Creator, context);
                    }
                    
                }
                SendMessage(workflowInstance.WorkflowId, context, workflowInstance);
            }

            if (context.Command.Equals(ActivityCommands.Withdraw))
            {
                //删除相关消息通知、待办待阅


                //删除流程相关活动实例、系统TODO
                WorkflowPostHelper.PostCancelWorkflowInstance(context.ActivityInstanceId);

                string wid = workflowInstance.WorkflowInstanceId.ToString();
                string sql = String.Format(@"delete from bwwf_Tracking_Todo where ActivityInstanceId in 
(select ActivityInstanceId from bwwf_Tracking_Activities where WorkflowInstanceId = '{0}');
delete from bwwf_Tracking_Activities where WorkflowInstanceId = '{0}';
delete from bwwf_Tracking_Activities_Completed where WorkflowInstanceId = '{0}';", wid);
                IBatisDbHelper.ExecuteNonQuery(System.Data.CommandType.Text, sql);
            }
        }


        #region 
        private void SaveForms(ActivityExecutionContext context, Guid WorkflowInstanceId, string actor)
        {

            // "通过"时才检查是否选择分派用户
            bool isApprove = ActivityCommands.Approve.Equals(context.Command);

            System.Collections.Specialized.NameValueCollection form = new System.Collections.Specialized.NameValueCollection();

            FormContext formContext = new FormContext(form, null);
            formContext.Variables = context.Variables;


            if (isApprove || ActivityCommands.Save.Equals(context.Command))
                formInstanceService.SaveForm(WorkflowInstanceId, formContext, actor);
        }
        #endregion 

        private void SendMessage(Guid WorkflowId, ActivityExecutionContext context, WorkflowInstance workflowInstance)
        {
            string cmdText = context.Command;
            Guid activityInstanceId = context.ActivityInstanceId;

            WorkflowProfile wfsetting = WorkflowProfile.LoadByWorkflowId(WorkflowId);
            //WorkflowSetting wfsetting = null;
            if (wfsetting == null)
                wfsetting = WorkflowProfile.Default;

            string toEmail = string.Empty;
            string toMobile = string.Empty;

            UserInfo user = new UserInfo();

            int operateType = TodoInfo.OpDefault;
            if (cmdText.Equals(ActivityCommands.ReturnToDraft, StringComparison.OrdinalIgnoreCase))
                operateType = TodoInfo.OpBack;

            IList<NotifyActor> notifyActors = workflowNotifyService.GetNextNotifyActors(activityInstanceId);
            IList<ActivityInstance> nextActivities = activityService.GetNextActivities(activityInstanceId);

            if (operateType == 0)
            {
                if (nextActivities != null && nextActivities.Count > 0)
                {
                    if (nextActivities != null && nextActivities.Count > 0)
                    {
                        for (int j = 0; j < nextActivities.Count; j++)
                        {
                            ActivityInstance activityInstance = nextActivities[j];
                            if (activityInstance.ActivityName == "财务付款")
                            {
                                SendCreatorMessage(workflowInstance.WorkflowId, context, workflowInstance.Creator);
                                break;
                            }
                        }
                        for (int i = 0; i < notifyActors.Count; i++)
                        {
                            //user = batisActivityExecutionService.GetUserByUserName(notifyActors[i].UserName);
                            user = userService.GetUserByUserName(notifyActors[i].UserName);
                            if (user != null)
                            {
                                if (user.Email == null)
                                    user.Email = "";
                                if (user.Mobile == null)
                                    user.Mobile = "";
                                toEmail = user.Email;
                                toMobile = user.Mobile;
                            }
                            batisActivityExecutionService.SendMessage(toEmail, toMobile, nextActivities, wfsetting, operateType, notifyActors[i].UserName);
                        }
                        //事件处理
                        SendEventHandler.SendEvent(operateType,context.Actor,workflowInstance.Creator,wd.WorkflowName, workflowInstance.Title, nextActivities[0].ActivityName);
                    }
                }
            }
            else if (operateType == 1)
            {
                //user = batisActivityExecutionService.GetUserByUserName(workflowInstance.Creator);
                user = userService.GetUserByUserName(workflowInstance.Creator);

                if (user != null)
                {
                    if (user.Email == null)
                        user.Email = "";
                    if (user.Mobile == null)
                        user.Mobile = "";
                    toEmail = user.Email;
                    toMobile = user.Mobile;
                    ActivityInstance nextActivitie = new ActivityInstance();
                    nextActivitie.Actor = workflowInstance.Creator;
                    nextActivitie.WorkItemTitle = workflowInstance.Title;
                    nextActivities.Add(nextActivitie);
                }

                batisActivityExecutionService.SendMessage(toEmail, toMobile, nextActivities, wfsetting, operateType, workflowInstance.Creator);
                //事件处理
                SendEventHandler.SendEvent(operateType,context.Actor, workflowInstance.Creator, wd.WorkflowName,workflowInstance.Title, nextActivities[0].ActivityName);
            }
        }

        private void SendCreatorMessage(Guid WorkflowId, ActivityExecutionContext context, string creator)
        {
            string cmdText = context.Command;
            Guid activityInstanceId = context.ActivityInstanceId;

            WorkflowProfile wfsetting = WorkflowProfile.LoadByWorkflowId(WorkflowId);
            //WorkflowProfile wfsetting = null;
            if (wfsetting == null)
                wfsetting = WorkflowProfile.Default;

            string toEmail = string.Empty;
            string toMobile = string.Empty;

            //UserInfo user = batisActivityExecutionService.GetUserByUserName(creator);]
            UserInfo user = userService.GetUserByUserName(creator);

            if (user != null)
            {
                if (user.Email == null)
                    user.Email = "";
                if (user.Mobile == null)
                    user.Mobile = "";
                toEmail = user.Email;
                toMobile = user.Mobile;
            }

            int operateType = TodoInfo.OpDefault;
            if (cmdText.Equals(ActivityCommands.Reject, StringComparison.OrdinalIgnoreCase))
                operateType = TodoInfo.OpBack;

            IList<NotifyActor> notifyActors = workflowNotifyService.GetNextNotifyActors(activityInstanceId);
            IList<ActivityInstance> nextActivities = activityService.GetNextActivities(activityInstanceId);

            batisActivityExecutionService.SendMessage(toEmail, toMobile, nextActivities, wfsetting, operateType, creator);

        }

        #endregion

        #region 保存数据到UHRP

        public void SaveToUhrp(string Creator, ActivityExecutionContext context)
        {
            //通过webservice把数据传到uhrp中


            Botwave.WebServiceClients.HRAttendanceService hrattendance = Botwave.WebServiceClients.ServiceFactory.GetHRAttendanceService();
        
            try
            {
                int flag = hrattendance.InsertAttendance("botwave", "password", Creator, context.Variables["LeaveType"].ToString(), decimal.Parse(context.Variables["TotalDayApply"].ToString()), DateTime.Parse(context.Variables["A1"].ToString()), DateTime.Parse(context.Variables["A2"].ToString()));
                if (flag == 1)
                {
                    log.Info(Creator + "：考勤数据添加成功，天数：" + context.Variables["TotalDayApply"].ToString() + "类型：" + context.Variables["LeaveType"].ToString());
                }
                else if (flag == 2)
                {
                    log.Info(Creator + "：用户验证出错，天数：" + context.Variables["TotalDayApply"].ToString() + "类型：" + context.Variables["LeaveType"].ToString());
                }
                else if (flag == 3)
                {
                    log.Info(Creator + "：考勤数据添加出错，天数：" + context.Variables["TotalDayApply"].ToString() + "类型：" + context.Variables["LeaveType"].ToString());
                }
            }
            catch
            {
                //ShowError(eee.Message.ToString());
            }
        }
        #endregion
    }
}
