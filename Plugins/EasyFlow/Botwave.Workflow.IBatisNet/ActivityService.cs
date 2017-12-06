using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using Botwave.Commons;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Plugin;
using Botwave.Extension.IBatisNet;

namespace Botwave.Workflow.IBatisNet
{
    public class ActivityService : IActivityService
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(IBatisActivityService));

        #region IActivityService Members

        public ActivityInstance GetActivity(Guid activityInstanceId)
        {
            return IBatisMapper.Mapper.QueryForObject<ActivityInstance>("bwwf_ActivityInstance_Load_By_Id", activityInstanceId);
        }

        public IList<ActivityInstance> GetCompletedActivities(Guid workflowInstanceId, Guid activityId)
        {
            Hashtable parameters = new Hashtable(2);
            parameters.Add("WorkflowInstanceId", workflowInstanceId);
            parameters.Add("ActivityId", activityId);
            return IBatisMapper.Select<ActivityInstance>("bwwf_ActivityInstance_Select_Completed_By_WfIId_AId", parameters);
        }

        public IList<ActivityInstance> GetActivitiesInSameWorkflow(Guid activityInstanceId)
        {
            return IBatisMapper.Select<ActivityInstance>("bwwf_ActivityInstance_Select", activityInstanceId);
        }

        public ActivityInstance GetCurrentActivity(Guid workflowInstanceId)
        {
            IList<ActivityInstance> activities = IBatisMapper.Select<ActivityInstance>("bwwf_ActivityInstance_Select_CurrentActivity_ByWorkflowInstanceId", workflowInstanceId);
            if (activities == null || activities.Count == 0)
                return null;
            return activities[0];
        }

        public IList<ActivityInstance> GetCurrentActivities(Guid workflowInstanceId)
        {
            return IBatisMapper.Select<ActivityInstance>("bwwf_ActivityInstance_Select_CurrentActivities", workflowInstanceId);
        }

        public IList<ActivityInstance> GetPrevActivities(Guid workflowInstanceId, Guid activityId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("WorkflowInstanceId", workflowInstanceId);
            parameters.Add("ActivityId", activityId);
            return IBatisMapper.Select<ActivityInstance>("bwwf_ActivityInstance_Select_PrevActivities_By_WFId_AId", parameters);
        }

        public IList<ActivityInstance> GetNextActivities(Guid activityInstanceId)
        {
            return IBatisMapper.Select<ActivityInstance>("bwwf_ActivityInstance_Select_NextActivities", activityInstanceId);
        }

        public IList<ActivityInstance> GetWorkflowActivities(Guid workflowInstanceId)
        {
            return IBatisMapper.Select<ActivityInstance>("bwwf_ActivityInstance_Select_WorkflowInstanceId", workflowInstanceId);
        }

        public IList<ActivityInstance> GetActivitiesByExternalEntity(string entityType, string entityId)
        {
            Hashtable parametersTable = new Hashtable(2);
            parametersTable.Add("ExternalEntityType", entityType);
            parametersTable.Add("ExternalEntityId", entityId);
            return IBatisMapper.Select<ActivityInstance>("bwwf_ActivityInstance_Select_ExternalEntity", parametersTable);
        }

        public IList<ActivityInstance> GetWorkflowCompletedActivities(Guid workflowInstanceId)
        {
            return IBatisMapper.Select<ActivityInstance>("bwwf_ActivityInstance_Select_Completed_By_Workflow", workflowInstanceId);
        }

        public Guid GetCurrentActivityId(Guid activityInstanceId)
        {
            object obj = IBatisMapper.Mapper.QueryForObject("bwwf_Get_Current_Activityid", activityInstanceId);
            if (obj == null)
                return Guid.Empty;
            else
                return new Guid(obj.ToString());
        }

        //public IList<Guid> GetCurrentActivityIds(Guid activityInstanceId)
        //{
        //    IList<Guid> currentIds = new List<Guid>();
        //    IList<ActivityInstance> currentInstances = this.GetCurrentActivities(activityInstanceId);
        //    foreach (ActivityInstance tempInstance in currentInstances)
        //    {
        //        currentIds.Add(tempInstance.ActivityId);
        //    }
        //    return currentIds;
        //}

        /// <summary>
        /// 根据名称(以及流程及流程实例)获取活动实例
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="workflowInstanceId"></param>
        /// <param name="activityName"></param>
        /// <returns></returns>
        public ActivityInstance GetActivityByActivityName(Guid workflowId, Guid workflowInstanceId, string activityName)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("WorkflowId", workflowId);
            parameters.Add("WorkflowInstanceId", workflowInstanceId);
            parameters.Add("ActivityName", activityName);
            IList<ActivityInstance> list = IBatisMapper.Select<ActivityInstance>("bwwf_ActivityInstance_Select_By_ActivityName", parameters);
            return (list.Count == 0) ? null : list[0];
        }

        public ActivityInstance GetLatestCompletedActivityByActivityName(Guid workflowId, Guid workflowInstanceId, string activityName)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("WorkflowId", workflowId);
            parameters.Add("WorkflowInstanceId", workflowInstanceId);
            parameters.Add("ActivityName", activityName);
            IList<ActivityInstance> list = IBatisMapper.Select<ActivityInstance>("bwwf_ActivityInstance_Select_By_ActivityName", parameters);
            return (list.Count == 0) ? null : list[0];
        }

        public IList<ActivityInstance> GetCompletedActivitiesOfPrevDefinitionByCurrent(ActivityInstance activityInstance)
        {
            return IBatisMapper.Select<ActivityInstance>("bwwf_ActivityInstance_Select_Instances_Of_PrevDefinition_By_CurrentInstance", activityInstance);
        }

        public DataTable GetTaskListByUserName(string userName, string workflowName, string keywords, int pageIndex, int pageSize, ref int recordCount)
        {
            string tableName = "vw_bwwf_Tracking_Todo";
            string fieldKey = "ActivityInstanceId";
            string fieldShow = @"ActivityInstanceId, UserName, State, ProxyName, OperateType, 
                          IsCompleted, CreatedTime,FinishedTime, Actor, ActivityName, Title, WorkflowAlias,WorkflowName, 
                          WorkflowInstanceId, SheetId,StartedTime, Urgency, Importance, 
                          Creator, CreatorName, AliasImage, TodoActors";
            string fieldOrder = "Urgency DESC, CreatedTime DESC";

            StringBuilder where = new StringBuilder();
            where.AppendFormat("(IsCompleted = 0) AND (UserName = '{0}')", userName);
            if (!string.IsNullOrEmpty(workflowName))
                where.AppendFormat(" AND (WorkflowName = '{0}')", workflowName);
            if (!string.IsNullOrEmpty(keywords))
            {
                keywords = DbUtils.FilterSQL(keywords);
                where.AppendFormat(" AND ((Actor LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (ActivityName LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (Title LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (WorkflowAlias LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (SheetId LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (CreatorName LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (ActivityInstanceId LIKE '%{0}%'))", keywords);
            }

            return IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where.ToString(), ref recordCount);
        }

        public DataTable GetTaskListByProxy(string proxyName, int pageIndex, int pageSize, ref int recordCount)
        {
            string tableName = "vw_bwwf_Tracking_Todo";
            string fieldKey = "ActivityInstanceId";
            string fieldShow = @"ActivityInstanceId, UserName, State, ProxyName, OperateType, 
                          IsCompleted, CreatedTime, Actor, ActivityName, Title, WorkflowAlias,WorkflowName, 
                          WorkflowInstanceId, SheetId,StartedTime, Urgency, Importance, 
                          Creator, CreatorName, AliasImage, ActorName";
            string fieldOrder = "Urgency DESC, CreatedTime DESC";

            StringBuilder where = new StringBuilder();
            where.AppendFormat("(IsCompleted = 0) AND (ProxyName = '{0}')", proxyName);

            return IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where.ToString(), ref recordCount);
        }

        #endregion
    }
}

