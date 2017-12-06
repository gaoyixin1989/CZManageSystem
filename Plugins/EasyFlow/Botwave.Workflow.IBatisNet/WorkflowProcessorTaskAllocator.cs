using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Allocator;

namespace Botwave.Workflow.IBatisNet
{
    /// <summary>
    /// 流程处理人任务分配算符
    /// </summary>
    public class WorkflowProcessorTaskAllocator : ITaskAllocator
    {
        #region ITaskAllocator Members

        public IList<string> GetTargetUsers(TaskVariable variable)
        {
            string expression = variable.Id;
            IList<string> list = new List<string>();
            if (String.IsNullOrEmpty(expression))
            {
                return list;
            }

            string workflowInstanceId = DbUtils.FilterSQL(expression);
            string sql = String.Format("SELECT DISTINCT Actor FROM bwwf_Tracking_Activities_Completed WHERE WorkflowInstanceId = '{0}'", workflowInstanceId);
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
