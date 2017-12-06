using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using System.Web;
using Botwave.Extension.IBatisNet;
using System.Data;
using System.Data.SqlClient;
using Botwave.GMCCServiceHelpers;

namespace Botwave.XQP.Service.Support
{
    /// <summary>
    /// 流程运维模块辅助类
    /// </summary>
    public class WorkflowMaintenanceService : IWorkflowMaintenanceService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(WorkflowMaintenanceService));
        #region service interfaces
        private IActivityService activityService;
        private ITaskAssignService taskAssignService;
        private ICountersignedService countersignedService;

        public IActivityService ActivityService
        {
            set { activityService = value; }
        }

        public ITaskAssignService TaskAssignService
        {
            set { taskAssignService = value; }
        }

        public ICountersignedService CountersignedService
        {
            set { countersignedService = value; }
        }
        #endregion
        #region IWorkflowMaintenanceService Members
        public void WorkflowProcessHistoryUpdate(Guid workflowInstanceId, HttpRequest request)
        {
            //保存处理意见
            IList<ActivityInstance> activityInstanceList = activityService.GetWorkflowActivities(workflowInstanceId);
            IList<Assignment> assignmentList = taskAssignService.GetAssignments(workflowInstanceId);
            foreach (ActivityInstance item in activityInstanceList)
            {
                if (!string.IsNullOrEmpty(request.Form["activity_reason_" + item.ActivityInstanceId]))
                {
                    //修改处理意见
                    string sql=string.Format("update bwwf_tracking_activities_completed set reason='{0}' where activityinstanceid='{1}'", request.Form["activity_reason_" + item.ActivityInstanceId],item.ActivityInstanceId);
                    IBatisDbHelper.ExecuteNonQuery(CommandType.Text, sql);
                }
                //修改会签处理意见
                IList<Countersigned> countersignedList = countersignedService.GetCountersignedList(item.ActivityInstanceId);
                foreach (Countersigned countersigned in countersignedList)
                {
                    if (!string.IsNullOrEmpty(request.Form["countersigned_reason_" + countersigned.ActivityInstanceId+"_"+countersigned.UserName]))
                    {
                        IBatisDbHelper.ExecuteNonQuery(CommandType.Text, string.Format("update bwwf_Tracking_Countersigned set Message='{0}' where activityinstanceid='{1}' and username='{2}'", request.Form["countersigned_reason_" + countersigned.ActivityInstanceId + "_" + countersigned.UserName], countersigned.ActivityInstanceId, countersigned.UserName));
                    }
                }
            }
            //修改转交意见
            foreach (Assignment item in assignmentList)
            {
                if (!string.IsNullOrEmpty(request.Form["assign_reason_" + item.ActivityInstanceId+"_"+item.AssignedTime.Value.ToString("yyyyMMddHHmmss")]))
                {
                    //修改处理意见
                    string sql = string.Format("update bwwf_Tracking_Assignments set Message='{0}' where activityinstanceid='{1}' and CONVERT(varchar,assignedtime,20)='{2}'", request.Form["assign_reason_" + item.ActivityInstanceId + "_" + item.AssignedTime.Value.ToString("yyyyMMddHHmmss")], item.ActivityInstanceId, item.AssignedTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    IBatisDbHelper.ExecuteNonQuery(CommandType.Text, sql);
                }
            }
        }

        public void WorkflowProcessHistoryDelete(Guid workflowInstanceId, HttpRequest request)
        {
            //删除处理记录
            IList<ActivityInstance> activityInstanceList = activityService.GetWorkflowActivities(workflowInstanceId);
            IList<Assignment> assignmentList = taskAssignService.GetAssignments(workflowInstanceId);
            foreach (ActivityInstance item in activityInstanceList)
            {
                if (!string.IsNullOrEmpty(request.Form["activity_chk_" + item.ActivityInstanceId]))
                {
                    //修改处理意见
                    string sql = string.Format("delete from bwwf_tracking_activities_completed where activityinstanceid='{0}'", item.ActivityInstanceId);
                    IBatisDbHelper.ExecuteNonQuery(CommandType.Text, sql);
                    
                    //删除转交，会签信息
                    sql = string.Format("delete from bwwf_Tracking_Assignments where activityinstanceid='{0}'", item.ActivityInstanceId);
                    IBatisDbHelper.ExecuteNonQuery(CommandType.Text, sql);
                    sql = string.Format("delete from bwwf_Tracking_Countersigned where activityinstanceid='{0}'", item.ActivityInstanceId);
                    IBatisDbHelper.ExecuteNonQuery(CommandType.Text, sql);

                }
                //删除会签记录
                IList<Countersigned> countersignedList = countersignedService.GetCountersignedList(item.ActivityInstanceId);
                foreach (Countersigned countersigned in countersignedList)
                {
                    if (!string.IsNullOrEmpty(request.Form["countersigned_chk_" + countersigned.ActivityInstanceId + "_" + countersigned.UserName]))
                    {
                        IBatisDbHelper.ExecuteNonQuery(CommandType.Text, string.Format("delete from bwwf_Tracking_Countersigned set where activityinstanceid='{0}' and username='{1}'", countersigned.ActivityInstanceId, countersigned.UserName));
                    }
                }
            }
            //删除转交意见
            foreach (Assignment item in assignmentList)
            {
                if (!string.IsNullOrEmpty(request.Form["assign_chk_" + item.ActivityInstanceId + "_" + item.AssignedTime.Value.ToString("yyyyMMddHHmmss")]))
                {
                    //修改处理意见
                    string sql = string.Format("delete from bwwf_Tracking_Assignments set where activityinstanceid='{0}' and CONVERT(varchar,assignedtime,20)='{1}'", item.ActivityInstanceId, item.AssignedTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    IBatisDbHelper.ExecuteNonQuery(CommandType.Text, sql);
                }
            }
        }

        public string DeleteWorkflowInstance(List<string> list)
        {
            int success = 0;
            try
            {
                IBatisMapper.Mapper.BeginTransaction();
                foreach (string workflowInstanceId in list)
                {
                    Guid WorkflowInstanceId = new Guid(workflowInstanceId);
                    WorkflowInstance workflowinstance = IBatisMapper.Load<WorkflowInstance>("bwwf_WorkflowInstance_Select", WorkflowInstanceId);
                    IBatisMapper.Insert("bwwf_WorkflowInstance_History_Insert", WorkflowInstanceId);
                    IBatisMapper.Delete("bwwf_WorkflowInstance_Delete", WorkflowInstanceId);
                    // 删除流程实例的后续处理.
                    IDbDataParameter param = IBatisDbHelper.CreateParameter();
                    param.ParameterName = "@WorkflowInstanceId";
                    param.DbType = DbType.Guid;
                    param.Value = WorkflowInstanceId;
                    IBatisDbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "xqp_wf_PostDeleteWorkflowInstance", param);
                    success++;
                }
                IBatisMapper.Mapper.CommitTransaction();
                return "成功删除" + success + "条记录.";
            }
            catch (Exception ex)
            {
                IBatisMapper.Mapper.RollBackTransaction();
                log.Error(ex);
                string errmsg = Botwave.Commons.ExceptionLogger.Log(ex.ToString());
                return errmsg;
            }
        }

        /// <summary>
        /// 查询被转移人的任务列表
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="refcount"></param>
        /// <returns></returns>
        public DataTable SearchTasks(Botwave.Workflow.Extension.Service.AdvancedSearchCondition condition,string actors,string processer,int pageIndex,int pageSize,ref int recordCount)
        {
            //将从0开始的页码转换为从1开始的页码
            pageIndex++;

            string str = string.Empty;
            StringBuilder builder = new StringBuilder();
            if ((condition.Workflows != null) && (condition.Workflows.Count > 0))
            {
                foreach (string str2 in condition.Workflows)
                {
                    builder.AppendFormat("'{0}',", str2);
                }
                builder.Length--;
                str = builder.ToString();
            }

            string fieldOrder = "StartedTime desc";
            if (!String.IsNullOrEmpty(condition.OrderField))
            {
                fieldOrder = condition.OrderField;
            }

            IDbDataParameter[] parameterArray = IBatisDbHelper.CreateParameterSet(15);
            parameterArray[0].ParameterName = "@BeginTime";
            parameterArray[0].DbType = DbType.String;
            parameterArray[0].Value = condition.BeginTime;
            parameterArray[1].ParameterName = "@EndTime";
            parameterArray[1].DbType = DbType.String;
            parameterArray[1].Value = condition.EndTime;
            parameterArray[2].ParameterName = "@Title";
            parameterArray[2].DbType = DbType.String;
            parameterArray[2].Value = condition.Title;
            parameterArray[3].ParameterName = "@SheetId";
            parameterArray[3].DbType = DbType.String;
            parameterArray[3].Value = condition.SheetId;
            parameterArray[4].ParameterName = "@WorkflowName";
            parameterArray[4].DbType = DbType.String;
            parameterArray[4].Value = condition.WorkflowName;
            parameterArray[5].ParameterName = "@AllowedWorkflowName";
            parameterArray[5].DbType = DbType.String;
            parameterArray[5].Value = str;
            parameterArray[6].ParameterName = "@ActivityName";
            parameterArray[6].DbType = DbType.String;
            parameterArray[6].Value = condition.ActivityName;
            parameterArray[7].ParameterName = "@CreatorName";
            parameterArray[7].DbType = DbType.String;
            parameterArray[7].Value = condition.CreatorName;
            parameterArray[8].ParameterName = "@ProcessorName";
            parameterArray[8].DbType = DbType.String;
            parameterArray[8].Value = processer;
            parameterArray[9].ParameterName = "@Actor";
            parameterArray[9].DbType = DbType.String;
            parameterArray[9].Value = actors;
            parameterArray[10].ParameterName = "@Keywords";
            parameterArray[10].DbType = DbType.String;
            parameterArray[10].Value = condition.Keywords;
            parameterArray[11].ParameterName = "@PageIndex";
            parameterArray[11].DbType = DbType.Int32;
            parameterArray[11].Value = pageIndex;
            parameterArray[12].ParameterName = "@PageSize";
            parameterArray[12].DbType = DbType.Int32;
            parameterArray[12].Value = pageSize;
            parameterArray[13].ParameterName = "@FieldOrder";
            parameterArray[13].DbType = DbType.String;
            parameterArray[13].Value = fieldOrder;
            parameterArray[14].ParameterName = "@RecordCount";
            parameterArray[14].DbType = DbType.Int32;
            parameterArray[14].Value = (int)recordCount;
            parameterArray[14].Direction = ParameterDirection.InputOutput;
            DataSet set = IBatisDbHelper.ExecuteDataset(CommandType.StoredProcedure, "bwwf_ext_AdvancedSearch_ByTask", parameterArray);
            recordCount = Convert.ToInt32(parameterArray[14].Value);
            return set.Tables[0];
        }

        public string TransferTodoTask(IList<Guid> workflowInstanceIds, string fromUsers, string toUser)
        {
            int count = 0;
            try
            {
                IBatisMapper.Mapper.BeginTransaction();
                foreach (Guid workflowInstanceId in workflowInstanceIds)
                {
                    
                    DataTable todoDt = IBatisDbHelper.ExecuteDataset(CommandType.Text, string.Format("select activityinstanceid,username from bwwf_tracking_todo where ActivityInstanceId in (select activityinstanceid from bwwf_tracking_activities where workflowinstanceid='{1}') and username in ({0}) and state < 2",fromUsers,workflowInstanceId)).Tables[0];
                    foreach (DataRow dw in todoDt.Rows)
                    {
                        Guid activityInstanceId = new Guid(DbUtils.ToString(dw[0]));
                        string fromUser = DbUtils.ToString(dw[1]);
                        SqlParameter[] param ={new SqlParameter("@ActivityInstanceId",activityInstanceId)
                                         ,new SqlParameter("@ToUser",toUser)
                                         ,new SqlParameter("@FromUser",fromUser)};
                        //转移待办
                        IBatisDbHelper.ExecuteNonQuery(CommandType.Text, "update bwwf_tracking_todo set state=0,username=@ToUser where ActivityInstanceId=@ActivityInstanceId and username=@FromUser and username<>@ToUser", param);
                        SqlParameter[] param1 = { new SqlParameter("@ActivityInstanceId", activityInstanceId) };
                        //删除推送
                        IBatisDbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[dbo].[xqp_wf_PostCloseActivityInstance]", param1);
                        count++;
                    }
                }
                IBatisMapper.Mapper.CommitTransaction();
                return "转移" + count + "条待办任务【ToUser:" + toUser + "】成功。";
            }
            catch (Exception ex)
            {
                IBatisMapper.Mapper.RollBackTransaction();
                log.Error("转移待办任务出错："+ex.ToString());
                return "转移待办任务【ToUser:"+toUser+"】出错：" + ex.Message;
            }
        }

        public string TransferDoneTask(IList<Guid> workflowInstanceIds, string fromUsers, string toUser)
        {
            try
            {
                IBatisMapper.Mapper.BeginTransaction();
                foreach (Guid workflowInstanceId in workflowInstanceIds)
                {
                    SqlParameter[] param ={new SqlParameter("@WorkflowInstanceId",workflowInstanceId)
                                         ,new SqlParameter("@ToUser",toUser)};
                    //转移已办
                    IBatisDbHelper.ExecuteNonQuery(CommandType.Text
                        , string.Format("update bwwf_tracking_activities_completed set actor=@ToUser,ActorDescription=(select realname from bw_users where username=@ToUser) where WorkflowInstanceId=@WorkflowInstanceId and actor in ({0})",fromUsers), param);
                    //删除推送
                    //IBatisDbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[dbo].[xqp_wf_PostCloseActivityInstance]", param);
                }
                IBatisMapper.Mapper.CommitTransaction();
                return "转移" + workflowInstanceIds.Count + "条已办任务【ToUser:" + toUser + "】成功。";
            }
            catch (Exception ex)
            {
                IBatisMapper.Mapper.RollBackTransaction();
                log.Error("转移待办任务出错：" + ex.ToString());
                return "转移已办任务【ToUser:" + toUser + "】出错：" + ex.Message;
            }
        }
        #endregion
    }
}
