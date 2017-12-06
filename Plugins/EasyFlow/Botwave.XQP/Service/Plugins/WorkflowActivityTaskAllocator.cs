using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Allocator;

namespace Botwave.XQP.Service.Plugins
{
    /// <summary>
    /// 过程控制任务分配算符
    /// </summary>
    public class WorkflowActivityTaskAllocator : ITaskAllocator
    {
        #region IWorkflowActivityTaskAllocator Members

        public IList<string> GetTargetUsers(TaskVariable variable)
        {
            string expression = variable.Id;
            IList<string> list = new List<string>();
            if (String.IsNullOrEmpty(expression))
            {
                return list;
            }

            string workflowInstanceId = DbUtils.FilterSQL(expression);
            string sql = String.Format("SELECT DISTINCT Actor FROM bwwf_Tracking_Activities_Completed WHERE WorkflowInstanceId = '{0}'", workflowInstanceId.ToUpper());
            if (variable.Expression != null)//提取历史步骤处理人
                sql = string.Format(@"select actor from bwwf_Tracking_Activities_Completed ta
                    inner join bwwf_activities ba 
                    on ta.activityid = ba.activityid 
                    where workflowInstanceId = '{0}' and activityname = '{1}'
                    union 
                    select username actor from bwwf_tracking_todo td
                    inner join bwwf_tracking_activities ta
                    on td.activityinstanceid=ta.activityinstanceid
                    inner join bwwf_activities ba 
                    on ta.activityid = ba.activityid 
                    where workflowInstanceId = '{0}' and activityname = '{1}'", workflowInstanceId.ToUpper(), variable.Expression.ToString());
            using (IDataReader reader = IBatisDbHelper.ExecuteReader(System.Data.CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    //对于强制完成、退回等操作，不一定有操作人
                    if (!reader.IsDBNull(0))
                    {
                        list.Add(reader.GetString(0));
                    }                    
                }
            }
            return list;
        }

        #endregion
    }
}
