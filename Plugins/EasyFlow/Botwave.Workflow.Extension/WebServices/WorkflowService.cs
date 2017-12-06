using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Services;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.Workflow.Extension.Util;

namespace Botwave.Workflow.Extension.WebServices
{
    /// <summary>
    /// 流程 Web Service 接口.
    /// </summary>
    [WebService(Namespace = "Botwave.Workflow.Extension.WebServices.WorkflowService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WorkflowService : System.Web.Services.WebService
    {
        /// <summary>
        /// 查询指定流程流水号的流程当前状态信息结果.
        /// </summary>
        /// <param name="workflowSheetId">流程流水号.</param>
        /// <returns>返回流程当前状态信息结果对象.</returns>
        [WebMethod]
        public WorkflowCurrentStateResult QueryWorkflowCurrentState(string workflowSheetId)
        {
            WorkflowCurrentStateResult results = new WorkflowCurrentStateResult();
            if (string.IsNullOrEmpty(workflowSheetId))
            {
                results.IsSuccess = false;
                results.State = -100;
                return results;
            }
            object state = IBatisMapper.Mapper.QueryForObject("bwwf_Tracking_Workflows_Select_State_BySheetId", workflowSheetId);
            if (state != null)
            {
                results.State = DbUtils.ToInt32(state);
                results.Activities = GetWorkflowCurrentActivities(workflowSheetId);
            }
            else
            {
                results.IsSuccess = false;
                results.State = -100;
            }
            return results;
        }

        #region 数据操作

        private const string Sql_GetCurrentActivities = @"SELECT ta.ActivityInstanceId, ta.WorkflowInstanceId, ta.IsCompleted, ta.OperateType,a.ActivityName,
                ISNULL(ta.Actor, dbo.fn_bwwf_GetTodoActors(ta.ActivityInstanceId, '')) Actors
            FROM vw_bwwf_Tracking_Activities_All ta
				LEFT JOIN bwwf_Activities a ON a.ActivityId = ta.ActivityId
            WHERE ActivityInstanceId IN(
              (
				          SELECT (
	                        SELECT TOP 1 ActivityInstanceId FROM vw_bwwf_Tracking_Activities_All
	                        WHERE (NOT EXISTS(
                                    SELECT ActivityInstanceId FROM vw_bwwf_Tracking_Activities_All
                                    WHERE IsCompleted = 0 AND WorkflowInstanceId = (
                                            SELECT WorkflowInstanceId FROM bwwf_Tracking_Workflows WHERE SheetId = '{0}'
                                     )
                                   )  
                              )
                              AND WorkflowInstanceId = (
                                    SELECT WorkflowInstanceId FROM bwwf_Tracking_Workflows WHERE SheetId = '{0}'
	                             )
					                ORDER BY CreatedTime desc
				            )
                )
                UNION
                (
                    SELECT ActivityInstanceId FROM vw_bwwf_Tracking_Activities_All
                    WHERE IsCompleted = 0 AND WorkflowInstanceId = (
                      SELECT WorkflowInstanceId FROM bwwf_Tracking_Workflows WHERE SheetId = '{0}'
                    )
                )
            )";

        private static ActivityResult[] GetWorkflowCurrentActivities(string workflowSheetId)
        {
            if (string.IsNullOrEmpty(workflowSheetId))
                return new ActivityResult[0];

            string sql = string.Format(Sql_GetCurrentActivities, workflowSheetId);
            DataTable resultTable = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
            if(resultTable == null || resultTable.Rows.Count ==0)
                return new ActivityResult[0];

            int count = resultTable.Rows.Count;
            ActivityResult[] results = new ActivityResult[count];
            for (int i = 0; i < count; i++)
            {
                DataRow row = resultTable.Rows[i];
                string activityName = DbUtils.ToString(row["ActivityName"]);
                string actors = DbUtils.ToString(row["Actors"]);
                results[i] = new ActivityResult(activityName, ParserActors(actors));
            }

            return results;
        }

        /// <summary>
        /// 解析流程步骤操作人（格式:用户名/用户真实姓名,用户名/用户真实姓名）.
        /// </summary>
        /// <param name="actors"></param>
        /// <returns></returns>
        private static string[] ParserActors(string actors)
        {
            if (string.IsNullOrEmpty(actors))
                return new string[0];
            string[] actorArray = actors.Split(',', '，');
            int length = actorArray.Length;

            string[] results = new string[length];
            for (int i = 0; i < length; i++)
            {
                string item = actorArray[i];
                if (item.IndexOf('/') > 0)
                {
                    item = item.Substring(0, item.IndexOf('/'));
                }
                results[i] = item;
            }

            return results;
        }
        #endregion
    }
}
