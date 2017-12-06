using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Service;

namespace Botwave.XQP.Service.Support
{
    public class ExtendAvtivityService : IActivityService
    {
        /// <summary>
        /// 分页获取待办任务列表
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="workflowName"></param>
        /// <param name="keywords"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public DataTable GetTaskListByUserName(string userName, string workflowName, string keywords, int pageIndex, int pageSize, ref int recordCount)
        {
            string tableName = "vw_bwwf_Tracking_Todo";
            string fieldKey = "ActivityInstanceId";
            string fieldShow = @"ActivityInstanceId, UserName, State, ProxyName, OperateType, 
                          IsCompleted, CreatedTime,FinishedTime, Actor, ActivityName, Title, WorkflowAlias,WorkflowName, 
                          WorkflowInstanceId, SheetId,StartedTime, Urgency, Importance, 
                          Creator, CreatorName, AliasImage, TodoActors";
            string fieldOrder = "CreatedTime DESC, Urgency DESC";

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

        public IList<Workflow.Domain.ActivityInstance> GetActivitiesByExternalEntity(string entityType, string entityId)
        {
            throw new NotImplementedException();
        }

        public IList<Workflow.Domain.ActivityInstance> GetActivitiesInSameWorkflow(Guid activityInstanceId)
        {
            throw new NotImplementedException();
        }

        public Workflow.Domain.ActivityInstance GetActivity(Guid activityInstanceId)
        {
            throw new NotImplementedException();
        }

        public IList<Workflow.Domain.ActivityInstance> GetCompletedActivities(Guid workflowInstanceId, Guid activityId)
        {
            throw new NotImplementedException();
        }

        public IList<Workflow.Domain.ActivityInstance> GetCompletedActivitiesOfPrevDefinitionByCurrent(Workflow.Domain.ActivityInstance activityInstance)
        {
            throw new NotImplementedException();
        }

        public IList<Workflow.Domain.ActivityInstance> GetCurrentActivities(Guid workflowInstanceId)
        {
            throw new NotImplementedException();
        }

        public Workflow.Domain.ActivityInstance GetCurrentActivity(Guid workflowInstanceId)
        {
            throw new NotImplementedException();
        }

        public Guid GetCurrentActivityId(Guid activityInstanceId)
        {
            throw new NotImplementedException();
        }

        public Workflow.Domain.ActivityInstance GetLatestCompletedActivityByActivityName(Guid workflowId, Guid workflowInstanceId, string activityName)
        {
            throw new NotImplementedException();
        }

        public IList<Workflow.Domain.ActivityInstance> GetNextActivities(Guid activityInstanceId)
        {
            throw new NotImplementedException();
        }

        public IList<Workflow.Domain.ActivityInstance> GetPrevActivities(Guid workflowInstanceId, Guid activityId)
        {
            throw new NotImplementedException();
        }

        public DataTable GetTaskListByProxy(string proxyName, int pageIndex, int pageSize, ref int recordCount)
        {
            throw new NotImplementedException();
        }

        public IList<Workflow.Domain.ActivityInstance> GetWorkflowActivities(Guid workflowInstanceId)
        {
            throw new NotImplementedException();
        }

        public IList<Workflow.Domain.ActivityInstance> GetWorkflowCompletedActivities(Guid workflowInstanceId)
        {
            throw new NotImplementedException();
        }
    }
}
