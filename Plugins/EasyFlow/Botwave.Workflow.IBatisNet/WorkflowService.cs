using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Commons;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Extension.IBatisNet;
using IBatisNet.DataMapper.SessionStore;
using IBatisNet.DataMapper;

namespace Botwave.Workflow.IBatisNet
{
    public class WorkflowService : IWorkflowService
    {
        #region IWorkflowService Members

        public WorkflowInstance GetWorkflowInstance(Guid workflowInstanceId)
        {
            IList<WorkflowInstance> list = IBatisMapper.Select<WorkflowInstance>("bwwf_WorkflowInstance_Select", workflowInstanceId);
            if (null == list || list.Count == 0)
            {
                return null;
            }

            return list[0];
        }

        public IList<WorkflowInstance> GetWorkflowInstance()
        {
            return IBatisMapper.Select<WorkflowInstance>("bwwf_WorkflowInstance_Select", null);          
        }


        public IList<WorkflowInstance> GetWorkflowInstanceByWorkflowId(Guid workflowId)
        {
            return IBatisMapper.Select<WorkflowInstance>("bwwf_WorkflowInstance_Select_By_WorkflowId", workflowId);
        }

        /// <summary>
        /// 根据活动实例获取流程实例的工作项数据
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        public WorkflowInstance GetWorkflowInstanceByActivityInstanceId(Guid activityInstanceId)
        { 
            IList<WorkflowInstance> list =  IBatisMapper.Select<WorkflowInstance>("bwwf_WorkflowInstance_Select_By_ActivityInstanceId", activityInstanceId);
            if (null == list || list.Count == 0)
            {
                return null;
            }

            return list[0];
        }

        public void InsertWorkflowInstance(WorkflowInstance instance)
        {
            IBatisMapper.Insert("bwwf_WorkflowInstance_Insert", instance);
        }

        public void UpdateWorkflowInstance(WorkflowInstance workflowInstance)
        {
            IBatisMapper.Update("bwwf_WorkflowInstance_Update", workflowInstance);
        }

        public void UpdateWorkflowInstanceForStart(WorkflowInstance workflowInstance)
        {
            IBatisMapper.Update("bwwf_WorkflowInstance_Update", workflowInstance);
        }

        public IList<WorkflowInstance> GetWorkflowInstances(string prefixTitle)
        {
            return IBatisMapper.Select<WorkflowInstance>("bwwf_WorkflowInstance_Select_Like_Title", prefixTitle);
        }

        public DataTable GetWorkflowInstanceByDraft(string userName)
        {
            const string sqlTemplate = @"SELECT tw.WorkflowInstanceId, tw.WorkflowId, tw.SheetId, tw.State, tw.Creator, tw.StartedTime, tw.FinishedTime, tw.Title, 
        tw.Secrecy, tw.Urgency, tw.Importance, tw.ExpectFinishedTime, tw.Requirement, ws.WorkflowAlias, ws.AliasImage
        FROM bwwf_Tracking_Workflows tw LEFT OUTER JOIN
            bwwf_Workflows w ON w.WorkflowId = tw.WorkflowId LEFT OUTER JOIN
            bwwf_WorkflowSettings ws ON ws.WorkflowName = w.WorkflowName
        WHERE tw.State = 0 AND tw.Creator = '{0}'
        ORDER BY tw.StartedTime DESC";
            userName = DbUtils.FilterSQL(userName);
            string sql = string.Format(sqlTemplate, userName);
            return IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public DataTable GetWorkflowInstanceByDraft(string userName, int topCount)
        {
            const string sqlTemplate = @"SELECT TOP {0} tw.WorkflowInstanceId, tw.WorkflowId, tw.SheetId, tw.State, tw.Creator, tw.StartedTime, tw.FinishedTime, tw.Title, 
        tw.Secrecy, tw.Urgency, tw.Importance, tw.ExpectFinishedTime, tw.Requirement, ws.WorkflowAlias, ws.AliasImage
        FROM bwwf_Tracking_Workflows tw LEFT OUTER JOIN
            bwwf_Workflows w ON w.WorkflowId = tw.WorkflowId LEFT OUTER JOIN
            bwwf_WorkflowSettings ws ON ws.WorkflowName = w.WorkflowName
        WHERE tw.State = 0 AND tw.Creator = '{1}'
        ORDER BY tw.StartedTime DESC";
            userName = DbUtils.FilterSQL(userName);
            string sql = string.Format(sqlTemplate, topCount, userName);
            return IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public int DeleteWorkflowInstance(Guid workflowInstanceId)
        {
            return IBatisMapper.Delete("bwwf_WorkflowInstance_Delete", workflowInstanceId);
        }

        #endregion
    }
}
