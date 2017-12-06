using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using IBatisNet.DataMapper;
using Botwave.Commons;
using Botwave.Workflow;
using Botwave.Workflow.Allocator;
using Botwave.Workflow.Parser;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Plugin;
using Botwave.Extension.IBatisNet;

namespace Botwave.Workflow.IBatisNet
{
    public class ActivityExecutionService : AbstractActivityExecutionService
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(IBatisActivityExecutionService));

        /// <summary>
        /// 获取指定的流程步骤操作人的描述字符串的 SQL 模板.
        /// </summary>
        private const string Template_GetActorDescription_Proxy = @"SELECT td.ActivityInstanceId, td.UserName, td.ProxyName, tu.RealName ActorRealName, pu.RealName ProxyRealName
                  FROM bwwf_Tracking_Todo td
                        LEFT JOIN bw_Users tu ON tu.UserName = td.UserName
                        LEFT JOIN bw_Users pu ON pu.UserName = td.ProxyName
                  WHERE (td.ActivityInstanceId = '{0}') AND (td.UserName = '{1}')";

        /// <summary>
        /// 获取指定的流程步骤操作人的描述字符串的 SQL 模板.
        /// </summary>
        private const string Template_GetActorDescription_Actor = "SELECT UserName, RealName FROM bw_Users WHERE UserName = '{0}'";

        #region 重写 IActivityExecutionService 成员

        //public override void Execute(ActivityExecutionContext context)
        //{
        //    ActionResult retValue = new ActionResult();
        //    try
        //    {
        //        IBatisMapper.Mapper.BeginTransaction();
        //        retValue = base.Execute(context);
        //        IBatisMapper.Mapper.CommitTransaction();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        IBatisMapper.Mapper.RollBackTransaction();
        //        throw ex;
        //    }
        //    return retValue;
        //}

        #endregion

        #region 实现 AbstractActivityExecutionService 抽象类

        protected override void RemoveWorkflowByActivityInstanceId(Guid activityInstanceId)
        {
            throw new WorkflowException("暂不提供删除流程实例功能");
        }

        protected override void SaveActivityInstance(ActivityInstance activityInstance, ActivityInstanceActionType actionType)
        {
            switch (actionType)
            {
                case ActivityInstanceActionType.Complete:
                    activityInstance.IsCompleted = true; Guid ActivityInstanceId = activityInstance.ActivityInstanceId;
                    IBatisMapper.Insert("bwwf_ActivityInstance_Completed_Insert", activityInstance);
                    DataTable dt = GetUserNameCount(activityInstance.ActivityInstanceId.ToString());
                    if (dt != null && dt.Rows.Count > 0)//非会签的情况下多人处理保留记录
                    {
                        IBatisMapper.Update("bwwf_ActivityInstance_Completed_update", activityInstance.ActivityInstanceId);
                        
                        foreach (DataRow dr in dt.Rows)
                        {
                            activityInstance.ActivityInstanceId = Guid.NewGuid();
                            activityInstance.Reason = "同意(系统自动处理)";
                            activityInstance.Actor = dr["username"].ToString();
                            activityInstance.ActorDescription = dr["ActorName"].ToString();
                            IBatisMapper.Insert("bwwf_ActivityInstance_Completed_InsertOne", activityInstance);
                            activityInstance.ActivityInstanceId = ActivityInstanceId;
                        }
                    }
                    IBatisMapper.Delete("bwwf_ActivityInstance_Delete", ActivityInstanceId);
                    break;
                case ActivityInstanceActionType.Create:
                    IBatisMapper.Insert("bwwf_ActivityInstance_Insert", activityInstance);
                    break;
                case ActivityInstanceActionType.CreateAndComplete:
                    IBatisMapper.Insert("bwwf_ActivityInstance_Completed_DirectInsert", activityInstance);
                    break;
                case ActivityInstanceActionType.Update:
                    IBatisMapper.Update("bwwf_ActivityInstance_Update", activityInstance);
                    break;
                default:
                    break;
            }
        }

        protected override IList<ActivityInstance> GetActivityInstancesBySetId(Guid setId)
        {
            return IBatisMapper.Select<ActivityInstance>("bwwf_ActivityInstance_Select_Prev_By_SetId", setId);
        }

        protected override IList<ActivityInstance> GetParellelActivityInstances(Guid workflowInstanceId, Guid setId)
        {
            Hashtable ht = new Hashtable(2);
            ht.Add("SetId", setId);
            ht.Add("WorkflowInstanceId", workflowInstanceId);
            return IBatisMapper.Select<ActivityInstance>("bwwf_ActivityInstance_Select_ParellelActivityInstances", ht);
        }
        
        protected override void InsertTodoActivity(Guid activityInstanceId, string userName, string proxyName, int operateType)
        {
            TodoInfo todo = new TodoInfo(activityInstanceId, userName, proxyName, operateType);
            IBatisMapper.Insert("bwwf_Todo_Insert", todo);
        }

        protected override void CreateActivityInstanceIdSet(Guid setId, Guid activityInstanceId)
        {
            Hashtable ht = new Hashtable(2);
            ht.Add("SetId", setId);
            ht.Add("ActivityInstanceId", activityInstanceId);
            IBatisMapper.Insert("bwwf_ActivityInstanceSet_Insert", ht);
        }

        protected override void UpdateWorkflowState(Guid activityInstanceId, int state)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ActivityInstanceId", activityInstanceId);
            ht.Add("State", state);
            IBatisMapper.Update("bwwf_WorkflowInstance_Finish", ht);
        }

        protected override void CloseAllActivitiesOfWorkflowInstance(ActivityExecutionContext context)
        {
            // 关闭所有步骤:
            // 1. 取得全部未完成流程步骤实例.
            // 2. 移动全部流程步骤实例到 bwwf_Tracking_Activities_Completed 表中.
            // 3. 删除 bwwf_Tracking_Activities 表中的当前完成流程步骤.
            IList<ActivityInstance> activityInstances = IBatisMapper.Select<ActivityInstance>("bwwf_ActivityInstance_Select_UnCompleted_By_AIId", context.ActivityInstanceId);
            string actor = context.Actor;
            IDictionary<string, string> actorRealNames = new Dictionary<string, string>();
            foreach (ActivityInstance item in activityInstances)
            {
                string realName = null;
                if (actorRealNames.ContainsKey(actor))
                {
                    realName = actorRealNames[actor];
                }
                else
                {
                    realName = GetUserRealName(actor);
                    actorRealNames.Add(actor, realName);
                }
                item.Actor = actor;
                item.ActorDescription = realName;
                item.Command = "close_activities";
                item.Reason = context.Reason;
                this.SaveActivityInstance(item, ActivityInstanceActionType.Complete);
            }
            // IBatisMapper.Update("bwwf_ActivityInstance_CloseAllActivitiesOfWorkflowInstance", context);
        }

        /// <summary>
        /// 获取指定流程实例的指定流程步骤定义的前一未完成的待处理步骤任务实例列表.
        /// </summary>
        /// <param name="workflowInstanceId">流程实例编号.</param>
        /// <param name="activityId">流程步骤定义.</param>
        /// <returns></returns>
        protected override IList<ActivityInstance> GetPrevActivityInstances(Guid workflowInstanceId, Guid activityId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("WorkflowInstanceId", workflowInstanceId);
            parameters.Add("ActivityId", activityId);

            return IBatisMapper.Select<ActivityInstance>("bwwf_ActivityInstance_Select_Prev_By_ActivityId", parameters);
        }

        /// <summary>
        /// 获取指定的流程步骤操作人的描述字符串.
        /// </summary>
        /// <param name="context">流程步骤上下文对象.</param>
        /// <returns></returns>
        protected override string GetActorDescription(ActivityExecutionContext context)
        {
            string actor = context.Actor;
            string actorName = actor;

            DataTable results = IBatisDbHelper.ExecuteDataset(CommandType.Text, string.Format(Template_GetActorDescription_Proxy, context.ActivityInstanceId, actor)).Tables[0];

            if (results.Rows.Count > 0)
            {
                DataRow row = results.Rows[0];
                actorName = DbUtils.ToString(row["ActorRealName"]);
                string proxy = DbUtils.ToString(row["ProxyName"]);
                string proxyName = DbUtils.ToString(row["ProxyRealName"]);
                if (string.IsNullOrEmpty(actorName))
                    actorName = actor;

                return WorkflowHelper.GetActivityActorDescription(actorName, proxyName);
            }
            else
            {
                results = IBatisDbHelper.ExecuteDataset(CommandType.Text, string.Format(Template_GetActorDescription_Actor, actor)).Tables[0];
                if (results.Rows.Count > 0)
                    actorName = DbUtils.ToString(results.Rows[0]["RealName"]);
                if (string.IsNullOrEmpty(actorName))
                    actorName = actor;
            }
            return WorkflowHelper.GetActivityActorDescription(actorName, null);
        }
        #endregion

        /// <summary>
        /// 获取用户真实姓名(用于存储在 ActorDescription 字段中).
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        protected static string GetUserRealName(string userName)
        {
            return IBatisMapper.Mapper.QueryForObject<string>("bwwf_ActivityInstanceActors_Select_RealName_ByActor", userName);
        }

        protected DataTable GetUserNameCount(string ActivityInstanceId)
        {
            string strsql = string.Format(@"select * from dbo.vw_bwwf_Tracking_Todo where ActivityInstanceId='{0}' and state <> 2", ActivityInstanceId);
            return IBatisDbHelper.ExecuteDataset(CommandType.Text, strsql).Tables[0];
        }
    }
}