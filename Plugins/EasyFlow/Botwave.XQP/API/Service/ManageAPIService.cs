using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using Botwave.Easyflow.API;
using Botwave.XQP.API.Util;
using System.Collections;
using Botwave.XQP.Domain;
using Botwave.Workflow.Domain;
using Botwave.Extension.IBatisNet;
using Botwave.Commons;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;

namespace Botwave.XQP.API.Service
{
    /// <summary>
    /// 处理类
    /// </summary>
    public class ManageAPIService : IManageAPIService
    {
        #region 注入

        private Botwave.Workflow.Service.ICommentService commentService;
        private Botwave.Workflow.Service.IActivityService activityService;
        private Botwave.Workflow.Service.ITaskAssignService taskAssignService;
        private Botwave.Security.Service.IUserService userService;
        private Botwave.Workflow.Service.IWorkflowService workflowService;
        private Botwave.Workflow.IWorkflowEngine workflowEngine;
        private Botwave.Workflow.Service.IActivityDefinitionService activityDefinitionService;
        private Botwave.DynamicForm.Services.IFormInstanceService formInstanceService;
        private Botwave.Workflow.Service.IActivityAllocationService activityAllocationService;
        private Botwave.Workflow.Plugin.IWorkflowInstanceCreationController workflowInstanceCreationController;
        private Botwave.Workflow.Extension.Service.IWorkflowNotifyService workflowNotifyService;
        private IWorkflowUserService workflowUserService;

        /// <summary>
        /// 评论
        /// </summary>
        public Botwave.Workflow.Service.ICommentService CommentService
        {
            set { commentService = value; }
        }
        /// <summary>
        /// 转交
        /// </summary>
        public Botwave.Workflow.Service.IActivityService ActivityService
        {
            set { activityService = value; }
        }
        /// <summary>
        /// 转交
        /// </summary>
        public Botwave.Workflow.Service.ITaskAssignService TaskAssignService
        {
            set { taskAssignService = value; }
        }
        /// <summary>
        /// 发单
        /// </summary>
        public Botwave.Security.Service.IUserService UserService
        {
            set { userService = value; }
        }
        /// <summary>
        /// 处理工单
        /// </summary>
        public Botwave.Workflow.Service.IWorkflowService WorkflowService
        {
            set { workflowService = value; }
        }
        /// <summary>
        /// 处理工单
        /// </summary>
        public Botwave.Workflow.IWorkflowEngine WorkflowEngine
        {
            set { workflowEngine = value; }
        }
        /// <summary>
        /// 处理工单
        /// </summary>
        public Botwave.Workflow.Service.IActivityDefinitionService ActivityDefinitionService
        {
            set { activityDefinitionService = value; }
        }
        /// <summary>
        /// 处理工单
        /// </summary>
        public Botwave.DynamicForm.Services.IFormInstanceService FormInstanceService
        {
            set { formInstanceService = value; }
        }
        /// <summary>
        /// 处理工单
        /// </summary>
        public Botwave.Workflow.Service.IActivityAllocationService ActivityAllocationService
        {
            set { activityAllocationService = value; }
        }
        /// <summary>
        /// 发单或者处理单的权限判断
        /// </summary>
        public Botwave.Workflow.Plugin.IWorkflowInstanceCreationController WorkflowInstanceCreationController
        {
            set { workflowInstanceCreationController = value; }
        }

        //kamael by 2012-10-23
        public Botwave.Workflow.Extension.Service.IWorkflowNotifyService WorkflowNotifyService
        {
            set { workflowNotifyService = value; }
        }

        public IWorkflowUserService WorkflowUserService
        {
            set { workflowUserService = value; }
        }
        #endregion

        /// <summary>
        /// 评论工单
        /// </summary>
        /// <param name="workflowInstanceId">工单唯一标识</param>
        /// <param name="activityInstanceId">活动ID</param>
        /// <param name="username">用户名</param>
        /// <param name="content">评论内容</param>
        /// <param name="CommentProperties">附件信息XML格式</param>
        /// <returns></returns>
        public void CommentWorkflow(string workflowInstanceId, string activityInstanceId, string username, string content, string CommentProperties)
        {

            #region 数据过滤

            if (XmlAnalysisHelp.ToGuid(workflowInstanceId) == null)
            {
                throw new WorkflowAPIException(13, "workflowInstanceId");
            }
            if (XmlAnalysisHelp.ToGuid(activityInstanceId) == null)
            {
                throw new WorkflowAPIException(13, "activityInstanceId");
            }

            if (string.IsNullOrEmpty(content))
            {
                throw new WorkflowAPIException(13, "content");
            }

            #endregion

            #region 附件处理
            Guid commentid = Guid.NewGuid();

            if (!string.IsNullOrEmpty(CommentProperties))
            {
                Botwave.XQP.API.Entity.Attachment[] att = XmlAnalysisHelp.AttachmentXml(CommentProperties);
                foreach (Botwave.XQP.API.Entity.Attachment a in att)
                {
                    CommonHelper.SaveAttachmentInfo(a, commentid, "W_C");
                }
            }

            #endregion

            Botwave.Workflow.Domain.Comment item = new Botwave.Workflow.Domain.Comment();
            item.WorkflowInstanceId = new Guid(workflowInstanceId);
            item.ActivityInstanceId = new Guid(activityInstanceId);
            item.Id = commentid;
            item.Creator = username;
            item.Message = content;

            // 添加评论
            commentService.AddComment(item);
        }

        /// <summary>
        /// 转交工单
        /// </summary>
        /// <param name="username">指定用户的Portal 登录账号</param>
        /// <param name="assignedUser">被指定用户的Portal 登录账号</param>
        /// <param name="activityInstanceId">活动ID</param>
        /// <param name="Message">转交意见</param>
        /// <returns></returns>
        public void AssignWorkflow(string username, string assignedUser, string activityInstanceId, string Message)
        {
            #region 数据过滤

            if (string.IsNullOrEmpty(assignedUser))
            {
                throw new WorkflowAPIException(13, "assignedUser");
            }

            if (XmlAnalysisHelp.ToGuid(activityInstanceId) == null)
            {
                throw new WorkflowAPIException(13, "activityInstanceId");
            }

            if (string.IsNullOrEmpty( CommonHelper.GetRealName(assignedUser)))
            {
                throw new WorkflowAPIException(16);
            }

            #endregion

            if (taskAssignService.GetTodoInfo(new Guid(activityInstanceId), assignedUser) != null)//转交权限判断
            {
                throw new WorkflowAPIException(12);
            }

            Botwave.Workflow.Domain.ActivityInstance activityInstance = activityService.GetActivity(new Guid(activityInstanceId));
            if (activityInstance == null)
            {
                throw new WorkflowAPIException(7, "activityInstanceId");
            }

            Botwave.Workflow.Domain.Assignment assignment = new Botwave.Workflow.Domain.Assignment();
            assignment.ActivityInstanceId = new Guid(activityInstanceId);
            assignment.AssignedTime = DateTime.Now;
            assignment.AssignedUser = assignedUser;
            assignment.AssigningUser = username;
            assignment.Message = Message;

            taskAssignService.Assign(assignment);  // 转交信息.

        }

        /// <summary>
        /// 发起工单
        /// </summary>
        /// <param name="userName">指定用户的Portal 登录账号</param>
        /// <param name="workflowId">流程定义ID（标识）</param>
        /// <param name="workflowTitle">需求单工单标题</param>
        /// <param name="startTime">工单发起时间</param>
        /// <param name="workflowProperties">需求单工单处理属性</param>
        /// <returns></returns>
        public string StartWorkflow(string userName, string workflowId, string workflowTitle,string workflowProperties)
        {
            string _workflowInstanceId = string.Empty;
            string _workflowID = string.Empty, FormDefinitionId = string.Empty;//FormDefinitionId 表单定义ID

            #region 数据过滤

            if (string.IsNullOrEmpty(workflowId))
            {
                throw new WorkflowAPIException(13, "workflowId");
            }

            if (string.IsNullOrEmpty(workflowTitle))
            {
                throw new WorkflowAPIException(13, "workflowTitle");
            }

            DataTable dt = APIServiceSQLHelper.QueryForDataSet("API_Select_Workflow_Start_WorkflowId", workflowId);
            if (dt != null && dt.Rows.Count > 0)
            {
                _workflowID = dt.Rows[0][0].ToString();//流程定义ID
            }
            else
            { throw new WorkflowAPIException(9); }

            #endregion

            Botwave.XQP.API.Entity.WorkflowDetail Detail = null;
            try
            {
                Detail = XmlAnalysisHelp.AnalysisXml(workflowProperties);
            }
            catch
            {
                throw new WorkflowAPIException(8);
            }



            Guid workflowInstanceId = Guid.NewGuid();//(流程实例ID)主键值
            Botwave.Security.Domain.UserInfo user = null;
            user = userService.GetUserByUserName(userName);
            Botwave.Security.LoginUser _user = new Botwave.Security.LoginUser(user);
            _user.Resources = CommonHelper.GetResources(userName);

            #region 权限判断

            if (!workflowInstanceCreationController.CanCreate(new Guid(_workflowID), userName, _user))
            {
                throw new WorkflowAPIException(10);
            }

            #endregion

            Botwave.Workflow.Domain.WorkflowInstance instance = new Botwave.Workflow.Domain.WorkflowInstance();

            instance.WorkflowId = new Guid(_workflowID);
            instance.WorkflowInstanceId = workflowInstanceId;
            instance.Creator = userName;
            instance.Secrecy = Detail.Secrecy;
            instance.Urgency = Detail.Urgency;
            instance.Importance = Detail.Importance;
            instance.ExpectFinishedTime = string.IsNullOrEmpty(Detail.ExpectFinishedTime) ? (DateTime?)null : DateTime.Parse(Detail.ExpectFinishedTime);
            instance.State = Detail.State;
            instance.Title = workflowTitle;
            instance.SheetId = Detail.SheetId;

            #region 表单

            Botwave.XQP.Util.FormContext Form = new Botwave.XQP.Util.FormContext();
            IDictionary<string, object> dict = new Dictionary<string, object>();
            if (Detail.Fields != null && Detail.Fields.Length != 0)
            {
                for (int i = 0; i < Detail.Fields.Length; i++)
                {
                    Botwave.XQP.API.Entity.Field f = Detail.Fields[i];
                    dict.Add(f.Key, f.Value);
                }
            }
            Form.Variables = dict;

            #endregion

            Botwave.Workflow.ActivityExecutionContext context = new Botwave.Workflow.ActivityExecutionContext();
            context.Actor = user.UserName;
            context.Command = Botwave.Workflow.ActivityCommands.Approve;
            context.Reason = "同意";

            #region 下一流程

            IDictionary<Guid, IDictionary<string, string>> dicts = new Dictionary<Guid, IDictionary<string, string>>();
            if (Detail.NextActivities != null && Detail.NextActivities.Length != 0)
            {
                for (int i = 0; i < Detail.NextActivities.Length; i++)
                {
                    Botwave.XQP.API.Entity.Activity a = Detail.NextActivities[i];
                    Hashtable ha = new Hashtable();
                    ha.Add("WorkflowId", _workflowID);
                    ha.Add("ActivityName", a.Name);
                    DataTable dt1 = APIServiceSQLHelper.QueryForDataSet("API_Select_Workflow_Start_ActivityId", ha);
                    if (dt1 != null && dt1.Rows.Count > 0)
                    {
                        string ActivityId = dt1.Rows[0]["ActivityId"] == null ? string.Empty : dt1.Rows[0]["ActivityId"].ToString();
                        if (ActivityId != null)
                        {
                            IDictionary<string, string> names = new Dictionary<string, string>();
                            for (int s = 0; s < a.Actors.Length; s++)
                            {
                                if (!string.IsNullOrEmpty(a.Actors[s]))
                                    names.Add(a.Actors[s].ToString(), "");
                            }
                            
                            CZActivityDefinition dataItem = CZActivityDefinition.GetWorkflowActivityByActivityId(new Guid(ActivityId));
                            string extendAllocatorArgs = dataItem.ExtendAllocatorArgs;
                            if (!string.IsNullOrEmpty(extendAllocatorArgs))//字段控制自动根据字段内容获取处理人
                            {
                                foreach (string allocatorArg in extendAllocatorArgs.Replace(" ", "").Split(';', '；'))
                                {
                                    string[] ocatorArray = allocatorArg.Split(':', '：');
                                    int lengthOfAllocatorArray = ocatorArray.Length;
                                    if (lengthOfAllocatorArray == 0)
                                        continue;
                                    if (ocatorArray[0] == "field")
                                    {
                                        IList<string> fieldList = FieldControlDal.GetTargetUsers(dict,workflowId,dataItem.ActivityName);
                                        foreach (string actor in fieldList)
                                        {
                                            if(!names.ContainsKey(actor))
                                                names.Add(actor, "");
                                        }
                                        break;
                                    }
                                }
                            }
                            dicts.Add(new Guid(ActivityId), names);
                        }
                    }
                    else
                        throw new Exception("步骤名称：" + a.Name + "不存在");
                }
            }
            else
                throw new Exception("步骤为空或步骤节点（nextactivities）有误");
            #endregion

            context.ActivityAllocatees = dicts;
            if (Form.Variables != null)
                context.Variables = Form.Variables;
            context.Variables["Secrecy"] = instance.Secrecy;
            context.Variables["Urgency"] = instance.Urgency;
            context.Variables["Importance"] = instance.Importance;
            context.Variables["CurrentUser"] = user;//添加当前用户为流程变量

            #region 附件处理

            if (Detail.Attachments != null)
            {
                for (int i = 0; i < Detail.Attachments.Length; i++)
                {
                    CommonHelper.SaveAttachmentInfo(Detail.Attachments[i], workflowInstanceId, "W_A");
                }
            }

            #endregion

            Guid activityinstanceid = Botwave.XQP.Util.WorkflowTransactionHelper.StartWorkflow(instance, Form, context, _user, false);
            _workflowInstanceId = workflowInstanceId.ToString();

            try
            {
                ActivityInstance activityInstance = activityService.GetActivity(activityinstanceid);
                WorkflowSetting wfsetting = IBatisMapper.Load<WorkflowSetting>("bwwf_WorkflowSettings_Select_ByWorkflowId", workflowId);
                if (wfsetting == null)
                    wfsetting = WorkflowSetting.Default;
                IList<NotifyActor> NotifyActor = workflowNotifyService.GetNotifyActors(activityinstanceid);
                string creator = ManageActivityinstance.GetCreator(workflowInstanceId);
                ActorDetail sender = workflowUserService.GetActorDetail(creator);
                workflowNotifyService.SendMessage(sender, activityInstance.OperateType, activityInstance.ActivityInstanceId, wfsetting, instance, NotifyActor);
            }
            catch (Exception ex)
            {

            }


            return _workflowInstanceId;
        }

        /// <summary>
        /// 传入表单数据以处理需求单
        /// </summary>
        /// <param name="userName">指定用户的Portal 登录账号</param>
        /// <param name="activityInstanceId">需求单工单标识(流程实例ID)</param>
        /// <param name="command">工单处理命令。approve：通过审核。reject：退回工单。cancel：取消工单。</param>
        /// <param name="workflowProperties">需求单工单处理属性</param>
        /// <param name="manageOpinion">处理意见</param>
        /// <returns></returns>
        public string ExecuteWorkflow(string userName, string activityInstanceId, string command, string workflowProperties, string manageOpinion)
        {
            #region 数据过滤

            if (XmlAnalysisHelp.ToGuid(activityInstanceId) == null)
            {
                throw new WorkflowAPIException(13, "activityInstanceId");
            }

            if (string.IsNullOrEmpty(command))
            {
                throw new WorkflowAPIException(13, "command");
            }

            if (!Botwave.Workflow.ActivityCommands.Approve.Equals(command.ToLower()) && !Botwave.Workflow.ActivityCommands.Cancel.Equals(command.ToLower()) && !Botwave.Workflow.ActivityCommands.Save.Equals(command.ToLower()) && !Botwave.Workflow.ActivityCommands.Reject.Equals(command.ToLower()))
            {
                throw new WorkflowAPIException(15);
            }
            
            #endregion

            Botwave.XQP.API.Entity.WorkflowDetail Detail = null;
            try
            {
                Detail = XmlAnalysisHelp.AnalysisXml(workflowProperties);
            }
            catch 
            {
                throw new WorkflowAPIException(8);
            }

            Guid ActivityInstanceId = new Guid(activityInstanceId);

            Botwave.Workflow.Domain.ActivityInstance activityInstance = activityService.GetActivity(ActivityInstanceId);
            if (activityInstance == null)
            {
                throw new WorkflowAPIException(7);
            }

            Botwave.Workflow.Domain.TodoInfo todo = taskAssignService.GetTodoInfo(ActivityInstanceId, userName);
            if (null == todo)
            {
                throw new WorkflowAPIException(11);
            }

            Botwave.Security.Domain.UserInfo user = null;
            // 用户信息.
            user = userService.GetUserByUserName(userName);
            Botwave.Security.LoginUser loginUser = new Botwave.Security.LoginUser(user);

            #region form实例

            // 表单实例绑定.
            IDictionary<string, object> formVariables = new Dictionary<string, object>();
            if (Detail.Fields != null && Detail.Fields.Length != 0)
            {
                for (int i = 0; i < Detail.Fields.Length; i++)
                {
                    Botwave.XQP.API.Entity.Field f = Detail.Fields[i];
                    formVariables.Add(f.Key, f.Value);
                }
            }

            #endregion

            // 工单基本信息更新.
            Guid workflowInstanceId = activityInstance.WorkflowInstanceId;
            Botwave.Workflow.Domain.WorkflowInstance instance = workflowService.GetWorkflowInstance(workflowInstanceId);
            instance.Secrecy = Detail.Secrecy;
            instance.Urgency = Detail.Urgency;
            instance.Importance = Detail.Importance;
            instance.ExpectFinishedTime = string.IsNullOrEmpty(Detail.ExpectFinishedTime) ? (DateTime?)null : DateTime.Parse(Detail.ExpectFinishedTime);

            // 工单处理.
            Botwave.Workflow.ActivityExecutionContext context = new Botwave.Workflow.ActivityExecutionContext();
            context.ActivityInstanceId = ActivityInstanceId;
            context.Actor = userName;
            context.Command = command.ToLower();
            context.Variables = formVariables;
            context.Reason = manageOpinion;//备注信息
            context.ExternalEntityType = activityInstance.ExternalEntityType;
            context.ExternalEntityId = activityInstance.ExternalEntityId;
            if (instance != null)
            {
                context.Variables["Secrecy"] = instance.Secrecy;
                context.Variables["Urgency"] = instance.Urgency;
                context.Variables["Importance"] = instance.Importance;
            }
            context.Variables["CurrentUser"] = loginUser;//添加当前用户为流程变量

            // 取消流程.（对取消特别对待）
            if (Botwave.Workflow.ActivityCommands.Cancel.Equals(context.Command))
            {
                workflowEngine.CancelWorkflow(context);
                return "";
            }

            #region 获取选中步骤以及处理人.

            Guid workflowId = instance.WorkflowId;
            string selectedActivity = string.Empty;
            IDictionary<Guid, IDictionary<string, string>> dicts = new Dictionary<Guid, IDictionary<string, string>>();
            if (Detail.NextActivities != null && Detail.NextActivities.Length != 0)
            {
                for (int i = 0; i < Detail.NextActivities.Length; i++)
                {
                    Botwave.XQP.API.Entity.Activity a = Detail.NextActivities[i];
                    Hashtable ha = new Hashtable();
                    ha.Add("WorkflowId", workflowId);
                    ha.Add("ActivityName", a.Name);
                    DataTable dt1 = APIServiceSQLHelper.QueryForDataSet("API_Select_Workflow_Start_ActivityId", ha);
                    if (dt1 != null && dt1.Rows.Count > 0)
                    {
                        string ActivityId = dt1.Rows[0]["ActivityId"] == null ? string.Empty : dt1.Rows[0]["ActivityId"].ToString();
                        if (ActivityId != null)
                        {
                            IDictionary<string, string> names = new Dictionary<string, string>();
                            for (int s = 0; s < a.Actors.Length; s++)
                            {
                                names.Add(a.Actors[s].ToString(), "");
                            }
                            dicts.Add(new Guid(ActivityId), names);
                        }
                    }
                }
            }
            #endregion

            #region 处理

            // "通过"时才检查是否选择分派用户
            int operateType = TodoInfo.OpDefault; //kamael by 2012-10-23
            bool isApprove = Botwave.Workflow.ActivityCommands.Approve.Equals(context.Command);
            if (isApprove)
            {
                context.ActivityAllocatees = dicts;
            }

            // "退还"时设置退还步骤
            else if (Botwave.Workflow.ActivityCommands.Reject.Equals(context.Command))
            {
                selectedActivity = Detail.NextActivities[0].Name;
                Hashtable parameters = new Hashtable();
                parameters.Add("ActivityInstanceId", activityInstanceId);
                parameters.Add("RejectActivityName", selectedActivity);
                object obj = IBatisMapper.Mapper.QueryForObject<int>("bwwf_ActivityInstanceReject_Select", activityInstanceId);
                if (null != obj && DbUtils.ToInt32(obj) == 0)
                {
                    IBatisMapper.Insert("bwwf_ActivityInstanceReject_Insert", parameters);
                }
                else
                {
                    IBatisMapper.Update("bwwf_ActivityInstanceReject_Update", parameters);
                }
            }

            string strCompletedActivityNames = String.Empty;
            IList<Botwave.Workflow.Domain.ActivityInstance> instanceList = activityService.GetActivitiesInSameWorkflow(ActivityInstanceId);
            foreach (Botwave.Workflow.Domain.ActivityInstance instances in instanceList)
            {
                if (instances.ActivityInstanceId.Equals(activityInstanceId)) continue;
                Botwave.Workflow.Domain.ActivityDefinition definition = activityDefinitionService.GetActivityDefinition(instances.ActivityId);
                strCompletedActivityNames += definition.ActivityName;
            }

            context.Variables["CompletedActivities"] = strCompletedActivityNames;   //添加已完成步骤为流程变量
            workflowEngine.ExecuteActivity(context);

            //审批通过进行表单更新
            if (isApprove || Botwave.Workflow.ActivityCommands.Save.Equals(context.Command))
            {
                Botwave.XQP.Util.FormContext formContext = new Botwave.XQP.Util.FormContext();
                formContext.Variables = formVariables;

                formInstanceService.SaveForm(workflowInstanceId, formContext.Variables, userName);
            }
            #endregion

            #region 附件处理

            if (Detail.Attachments != null)
            {
                for (int i = 0; i < Detail.Attachments.Length; i++)
                {
                    CommonHelper.SaveAttachmentInfo(Detail.Attachments[i], workflowInstanceId, "W_A");
                }
            }

            #endregion


            #region kamael by 2012-10-23
            string Nowactivityinstanceid = ManageActivityinstance.GetActivityinstanceid(workflowInstanceId);

            try
            {
                //if (ManageActivityinstance.GetEnableApprovalSMSSwitch(workflowId, Nowactivityinstanceid))
                //{
                WorkflowSetting wfsetting = IBatisMapper.Load<WorkflowSetting>("bwwf_WorkflowSettings_Select_ByWorkflowId", workflowId);
                if (wfsetting == null)
                    wfsetting = WorkflowSetting.Default;
                IList<NotifyActor> NotifyActor = workflowNotifyService.GetNotifyActors(new Guid(Nowactivityinstanceid));
                string creator = ManageActivityinstance.GetCreator(workflowInstanceId);
                ActorDetail sender = workflowUserService.GetActorDetail(creator);
                workflowNotifyService.SendMessage(sender, activityInstance.OperateType, activityInstance.ActivityInstanceId, wfsetting, instance, NotifyActor);
                //}
            }
            catch (Exception ex)
            {
                
            }

            //返回当前活动ID  activityinstanceid
            return Nowactivityinstanceid;
            #endregion
            //返回当前活动ID  activityinstanceid
           // return ManageActivityinstance.GetActivityinstanceid(workflowInstanceId);
        }

        //public DataTable GetNextActivities(string userName,string activityInstanceId)
        //{
        //    DataTable dtReturn = null;
        //    try
        //    {
        //        IList<Botwave.Workflow.Domain.ActivityDefinition> activities = activityDefinitionService.GetNextActivityDefinitionsByInstanceId(new Guid(activityInstanceId));
        //        for (int i = 0; i < activities.Count; i++)
        //        {
        //            Botwave.Workflow.Domain.ActivityDefinition dataItem = activities[i];
        //            IDictionary<string, string> dict = activityAllocationService.GetTargetUsers(Guid.Empty, dataItem, userName, true);

        //            Activity item = new Activity();
        //            item.Name = dataItem.ActivityName;
        //            item.Actors = dict.Keys.ToArray();
        //            result.Add(item);
        //        }
        //    }
        //    catch 
        //}
    }
}