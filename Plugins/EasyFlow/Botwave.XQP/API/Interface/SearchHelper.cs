using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Botwave.Extension.IBatisNet;
using Botwave.XQP.API.Util;
using Botwave.Easyflow.API;
using System.Collections;
using Botwave.XQP.API.Entity;
using Botwave.Workflow.Domain;

namespace Botwave.XQP.API.Interface
{
    public class SearchHelper
    {
        #region 获取指定工单标识的需求单明细信息Detail

        /// <summary>
        /// 获取指定工单标识的明细信息
        /// </summary>
        /// <param name="workflowInstanceId">需求单工单标识流程实例ID</param>
        /// <param name="sheetId">需求单工单工单号sheetId</param>
        /// <returns></returns>
        public DataTable GetWorkflowDetail(string workflowInstanceId, string sheetId)
        {
            DataTable dtReturn = null;

            if (XmlAnalysisHelp.ToGuid(workflowInstanceId) == null && string.IsNullOrEmpty(sheetId))
            {
                throw new WorkflowAPIException(14, "workflowInstanceId,sheetId");
            }

            //Hashtable ha = new Hashtable();
            //ha.Add("WorkflowInstanceId", workflowInstanceId);
            //ha.Add("SheetId", sheetId);
            //dtReturn = Botwave.Extension.IBatisNet.IBatisMapper.Mapper..("API_Select_WorkflowDetail", ha).DataSet.Tables[0];
            string where = string.Format(" WorkflowInstanceid = '{0}'", workflowInstanceId);
            if (!string.IsNullOrEmpty(sheetId))
                where = string.Format(" SheetId = '{0}'", sheetId);
            string sql = string.Format(@"select a.*,b.WorkflowAlias,us.Realname as Username,us.Mobile ,xp.ismobile from (
select WorkflowInstanceId,SheetId,State,Title,Secrecy,Urgency,Importance,ExpectFinishedTime,StartedTime,FinishedTime,
bwwf_Workflows.WorkflowName,bwwf_Tracking_Workflows.Creator,bwwf_Workflows.CreatedTime,bwwf_Workflows.WorkflowId,
fun_bwwf_getCurrentActNames(WorkflowInstanceId) ActivityName,
fun_bwwf_getCurrentActors(WorkflowInstanceId) CurrentActors
from bwwf_Tracking_Workflows inner join bwwf_Workflows 
on bwwf_Tracking_Workflows.WorkflowId=bwwf_Workflows.WorkflowId
) a
left join (select workflowAlias,workflowName from bwwf_workflowsettings) b on a.WorkflowName=b.WorkflowName
left join xqp_workflowsettings xp on a.WorkflowName=xp.WorkflowName
inner join bw_Users us on us.username=a.creator
where {0}", where);
            dtReturn = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];

            return dtReturn;
        }

        /// <summary>
        /// 获取指定工单标识的明细信息
        /// </summary>
        /// <param name="activityInstanceId">需求单工单标识流程步骤实例ID</param>
        /// <returns></returns>
        public DataTable GetWorkflowDetail(string activityInstanceId)
        {
            DataTable dtReturn = null;

            if (XmlAnalysisHelp.ToGuid(activityInstanceId) == null)
            {
                throw new WorkflowAPIException(14, "activityInstanceId");
            }

            //Hashtable ha = new Hashtable();
            //ha.Add("WorkflowInstanceId", workflowInstanceId);
            //ha.Add("SheetId", sheetId);
            //dtReturn = Botwave.Extension.IBatisNet.IBatisMapper.Mapper..("API_Select_WorkflowDetail", ha).DataSet.Tables[0];
            string sql = string.Format(@"select ta.activityInstanceId,a.*,b.WorkflowAlias,us.Realname as Username,us.Mobile,xp.ismobile from (
select WorkflowInstanceId,SheetId,State,Title,Secrecy,Urgency,Importance,ExpectFinishedTime,StartedTime,FinishedTime,
bwwf_Workflows.WorkflowName,bwwf_Tracking_Workflows.Creator,bwwf_Workflows.CreatedTime,bwwf_Workflows.WorkflowId,
fun_bwwf_getCurrentActNames(WorkflowInstanceId) ActivityName,
fun_bwwf_getCurrentActors(WorkflowInstanceId) CurrentActors
from bwwf_Tracking_Workflows inner join bwwf_Workflows 
on bwwf_Tracking_Workflows.WorkflowId=bwwf_Workflows.WorkflowId
) a
inner join vw_bwwf_tracking_act_all ta
on a.WorkflowInstanceId = ta.WorkflowInstanceId
left join (select workflowAlias,workflowName from bwwf_workflowsettings) b on a.WorkflowName=b.WorkflowName
left join xqp_workflowsettings xp on a.WorkflowName=xp.WorkflowName
inner join bw_Users us on us.username=a.creator
where 
ta.activityInstanceId = '{0}'", activityInstanceId);
            dtReturn = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];

            return dtReturn;
        }

        /// <summary>
        /// 获取指定工单的表单列表
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public DataTable GetWorkflowFieldList(string workflowInstanceId, IDictionary<string, object> formVariables)
        {
            DataTable dtReturn = null;

            //dtReturn = IBatisMapper.Load<DataTable>("API_Select_WorkflowDetail_Field", workflowInstanceId).DataSet.Tables[0];
            string sql = string.Format(@"select b.FormInstanceId, Fname as FKey,Name,
(case
 when value_str is null then
		value_text
 when value_text is null then
    value_str
  else 
  cast (value_decimal as nchar(50))
end) 
FValue
from bwdf_FormItemDefinitions a,bwdf_FormItemInstances b
where a.id=b.FormItemDefinitionId  and b.FormInstanceId='{0}'", workflowInstanceId);
            dtReturn = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];

            if (formVariables != null)
            {
                foreach (DataRow dw in dtReturn.Rows)
                {
                    if (formVariables.ContainsKey(dw["FKey"].ToString()))
                        dw["FValue"] = formVariables[dw["FKey"].ToString()];
                }
            }
            return dtReturn;
        }

        /// <summary>
        /// 获取指定工单的步骤列表
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public DataTable GetWorkflowActivitiesList(string workflowInstanceId)
        {
            DataTable dtReturn = null;
            object state = IBatisDbHelper.ExecuteScalar(CommandType.Text, string.Format("select state from bwwf_tracking_workflows where workflowinstanceid='{0}'", workflowInstanceId));

            //dtReturn = IBatisMapper.Load<DataTable>("API_Select_WorkflowDetail_Activitys", workflowInstanceId).DataSet.Tables[0];
            string sql = string.Format(@"select td.activityinstanceid,ba.ActivityId,ba.activityname as Name,td.UserName as Actors,
td.OperateType
 from bwwf_tracking_workflows tw
left join bwwf_tracking_activities ta on tw.workflowinstanceid = ta.workflowinstanceid
left join bwwf_tracking_todo td
on ta.activityinstanceid = td.activityinstanceid
left join bwwf_activities ba
on ta.activityid = ba.activityid
where tw.workflowinstanceid = '{0}'", workflowInstanceId);
            if (Botwave.Commons.DbUtils.ToInt32(state) == 99)
            {
                sql = string.Format(@"select td.activityinstanceid,ba.ActivityId,ba.activityname as Name,td.UserName as Actors,
td.OperateType
 from bwwf_tracking_workflows tw
left join bwwf_Tracking_Activities_Completed ta on tw.workflowinstanceid = ta.workflowinstanceid
left join bwwf_tracking_todo td
on ta.activityinstanceid = td.activityinstanceid
left join bwwf_activities ba
on ta.activityid = ba.activityid
where tw.workflowinstanceid = '{0}'", workflowInstanceId);
            }
            else if (Botwave.Commons.DbUtils.ToInt32(state) == 2)
            {
                sql = string.Format(@"select ta.activityinstanceid,ba.ActivityId,(CASE tw.STATE
WHEN 2 THEN ba.activityname
WHEN 99 THEN '取消'
END) as Name,dbo.fun_bwwf_getActivityActors(ActivityInstanceId,0) as Actors,
ta.OperateType
 from bwwf_tracking_workflows tw
inner join bwwf_Tracking_Activities_Completed ta on tw.workflowinstanceid = ta.workflowinstanceid
inner join bwwf_activities ba
on ta.activityid = ba.activityid
where ta.WorkflowInstanceId = '{0}' and ba.state=2", workflowInstanceId);
            }
            dtReturn = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];

            return dtReturn;
        }

        /// <summary>
        /// 获取指定工单的下行步骤列表
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public DataTable GetWorkflowNextActivitysList(string workflowInstanceId)
        {
            DataTable dtReturn = null;

            //dtReturn = IBatisMapper.Load<DataTable>("API_Select_WorkflowDetail_Activitys_Next", workflowInstanceId).DataSet.Tables[0];
            string sql = string.Format(@"select g.ActivityId,ActivityName as Name,UserName as Actors from (
select a.ActivityId,dbo.fun_bwwf_getActivityActors(ActivityInstanceId,0) as UserName,a.NextActivitySetId from 
bwwf_Tracking_Activities  b,bwwf_Activities a where a.ActivityId=b.ActivityId and 
WorkflowInstanceId ='{0}') f
, (select a.ActivityId, SetID,ActivityName from bwwf_Activities a,bwwf_ActivitySet b where a.ActivityId=b.ActivityId ) g
where f.NextActivitySetId=g.setid", workflowInstanceId);
            dtReturn = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];

            return dtReturn;
        }

        /// <summary>
        /// 获取当前步骤的上一步骤
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        public DataTable GetWorklfowPreActivityList(string activityInstanceId)
        {
            string sql = string.Format(@"select t1.Activityid,ba.activityname as Name,fun_bwwf_getActivityActors(ActivityInstanceId,0) as Actors
 from
(SELECT BTA1.Activityinstanceid, BTA1.activityid FROM bwwf_Tracking_Activities_Completed BTA1 WHERE BTA1.ActivityInstanceId IN(
          SELECT activityinstanceid FROM bwwf_tracking_activities_set where setid =(
                 SELECT prevsetid FROM vw_bwwf_tracking_act_all BTA1
                 WHERE BTA1.activityInstanceId = '{0}')
        ) )t1
 inner join 
 bwwf_activities ba
 on t1.Activityid = ba.activityid", activityInstanceId);
            return IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];

        }

        /// <summary>
        /// 获取指定工单的附件列表
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public DataTable GetWorkflowAttachmentList(string workflowInstanceId)
        {
            DataTable dtReturn = null;

            //Hashtable ha = new Hashtable();
            //ha.Add("WorkflowInstanceId", workflowInstanceId);
            //ha.Add("EntityType", "W_A");
            //dtReturn = IBatisMapper.Load<DataTable>("API_Select_WorkflowDetail_Attachment", ha).DataSet.Tables[0];
            string sql = string.Format(@" SELECT att.CreatedTime,att.Creator,us.realname as Username,att.Title as Name,att.FileName as Url FROM xqp_Attachment  att inner join bw_Users us on us.username=att.creator WHERE ID in(
SELECT AttachmentId FROM xqp_Attachment_Entity WHERE ENTITYID='{0}' and entitytype='W_A')", workflowInstanceId);
            dtReturn = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];

            return dtReturn;
        }

        #endregion

        #region 获取指定工单标识的需求单处理信息recordlist
        /// <summary>
        /// 获取指定需求单处理列表
        /// </summary>
        /// <param name="workflowInstanceId">需求单工单标识(流程实例ID或工单号sheetId)</param>
        /// <returns></returns>
        public IList<WorkflowRecord> GetWorkflowRecord(string workflowInstanceId)
        {
            IList<WorkflowRecord> wrList = new List<WorkflowRecord>();


            #region workflowInstanceId 流程实例ID

            string strSql = string.Format("select WorkflowInstanceId,workflowID,SheetID,Title from dbo.bwwf_Tracking_Workflows where cast(WorkflowInstanceId as varchar(50))='{0}' or SheetID='{0}'", workflowInstanceId);
            DataTable dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, strSql).Tables[0];
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            #endregion

            IList<Assignment> workflowAssignments = IBatisMapper.Select<Assignment>("bwwf_Assignment_Select_By_WorkflowInstanceId", workflowInstanceId);//转交信息

            IList<ActivityInstance> Activitys = IBatisMapper.Select<ActivityInstance>("bwwf_ActivityInstance_Select_WorkflowInstanceId", workflowInstanceId);
            foreach (ActivityInstance Activity in Activitys)
            {
                WorkflowRecord wr = new WorkflowRecord();
                wr.ActivityInstanceId = Activity.ActivityInstanceId.ToString();
                wr.ActivityName = Activity.ActivityName == null ? "" : Activity.ActivityName.ToString();
                wr.Actor = Activity.Actor == null ? "" : Activity.Actor.ToString();
                wr.CompletedTime = Activity.FinishedTime == null ? "" : Activity.FinishedTime.ToString();
                wr.CreatedTime = Activity.CreatedTime == null ? "" : Activity.CreatedTime.ToString();
                switch (Activity.Command)
                {
                    case "approve":
                        wr.Command = 1;
                        break;
                    case "reject":
                        wr.Command = 0;
                        break;
                    default:
                        break;
                }

                #region 转交

                if (workflowAssignments.Count > 0)
                {
                    foreach (Assignment ass in workflowAssignments)
                    {
                        if (Activity.ActivityInstanceId == ass.ActivityInstanceId)
                        {
                            WorkflowRecord wr1 = new WorkflowRecord();
                            wr1.ActivityInstanceId = ass.ActivityInstanceId.ToString();
                            wr1.ActivityName = Activity.ActivityName;
                            wr1.Actor = ass.AssignedUser;
                            wr1.Command = 2;
                            wr1.CreatedTime = ass.AssignedTime.ToString();
                            wrList.Add(wr1);
                        }
                    }
                }

                #endregion

                #region 会签

                if (!string.IsNullOrEmpty(Activity.CountersignedCondition))
                {
                    IList<Countersigned> countersigneds = IBatisMapper.Select<Countersigned>("bwwf_Countersigned_Select_By_ActivityInstanceId", Activity.ActivityInstanceId);
                    if (countersigneds != null && countersigneds.Count > 0)
                    {
                        foreach (Countersigned Countersigne in countersigneds)
                        {
                            WorkflowRecord wr2 = new WorkflowRecord();
                            wr2.ActivityInstanceId = Countersigne.ActivityInstanceId.ToString();
                            wr2.ActivityName = Activity.ActivityName;
                            wr2.Actor = Countersigne.UserName;
                            wr2.Command = 1;
                            wr2.CreatedTime = Countersigne.CreatedTime.ToString();
                            wrList.Add(wr2);
                        }
                    }
                }

                #endregion

                wrList.Add(wr);
            }

            return wrList;
        }
        #endregion
        #region 获取指定需求单处理列表

        /// <summary>
        /// 处理列表
        /// </summary>
        /// <param name="workflowInstanceId">需求单工单标识流程实例ID</param>
        /// <param name="sheetId">需求单工单工单号sheetId</param>
        /// <returns></returns>
        public DataTable GetWorkflowRecordActivityList(string workflowInstanceId, string sheetId, string isApproval)
        {
            DataTable dtReturn = null;
            if (XmlAnalysisHelp.ToGuid(workflowInstanceId) == null && string.IsNullOrEmpty(sheetId))
            {
                throw new WorkflowAPIException(14, "workflowInstanceId,sheetId");
            }

            Hashtable ha = new Hashtable();
            ha.Add("WorkflowInstanceId", workflowInstanceId);
            ha.Add("SheetId", sheetId);
            ha.Add("IsApproval", isApproval);
            dtReturn = string.IsNullOrEmpty(isApproval) ? APIServiceSQLHelper.QueryForDataSet("API_Select_WorkflowRecord_ActivityInstance", ha) : APIServiceSQLHelper.QueryForDataSet("API_Select_WorkflowRecord_ActivityInstance_Business", ha);

            return dtReturn;
        }

        /// <summary>
        /// 转交列表
        /// </summary>
        /// <param name="workflowInstanceId">需求单工单标识流程实例ID</param>
        /// <returns></returns>
        public DataTable GetWorkflowRecordAssignmentList(string workflowInstanceId)
        {
            DataTable dtReturn = null;

            dtReturn = APIServiceSQLHelper.QueryForDataSet("API_Select_WorkflowRecord_Assignment", workflowInstanceId);//转交信息

            return dtReturn;
        }

        /// <summary>
        /// 会签列表
        /// </summary>
        /// <param name="activityInstanceId">需求单工单标识活动实例ID</param>
        /// <returns></returns>
        public DataTable GetWorkflowRecordCountersignedList(string activityInstanceId)
        {
            DataTable dtReturn = null;

            dtReturn = APIServiceSQLHelper.QueryForDataSet("API_Select_WorkflowRecord_Countersigned", activityInstanceId);//会签信息

            return dtReturn;
        }

        /// <summary>
        /// 获取指定用户的待办数
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public DataTable GetMyTodoCount(string username)
        {
            string sql = string.Format("select count(td.ActivityInstanceId) from bwwf_Tracking_Todo td LEFT JOIN bwwf_Tracking_Activities ta ON ta.ActivityInstanceId = td.ActivityInstanceId where username = '{0}' and state < 2 and ta.iscompleted = 0", username);
            return IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        /// <summary>
        /// 获取虚拟用户待办数
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public DataTable GetMyPoolTodoCount(string username)
        {
            string sql = string.Format("select count(td.ActivityInstanceId) from bwwf_Tracking_Todo td LEFT JOIN bwwf_Tracking_Activities ta ON ta.ActivityInstanceId = td.ActivityInstanceId where state < 2 and exists (select parentusername from SZ_WORKFLOWS_VirtualUsers vu where username = '{0}' and vu.parentusername = td.username) and ta.iscompleted = 0", username);
            return IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }
        #endregion
    }
}
