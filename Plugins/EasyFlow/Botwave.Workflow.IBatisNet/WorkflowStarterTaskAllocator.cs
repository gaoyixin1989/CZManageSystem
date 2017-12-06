using System;
using System.Collections.Generic;
using System.Text;

using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Allocator;

namespace Botwave.Workflow.IBatisNet
{
    /// <summary>
    /// 流程发起人任务分配算符.
    /// </summary>
    public class WorkflowStarterTaskAllocator : ITaskAllocator
    {
        #region ITaskAllocator Members

        /// <summary>
        /// 获取流程实例的发起人.
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        public IList<string> GetTargetUsers(TaskVariable variable)
        {
            string expression = variable.Id;
            IList<string> list = new List<string>();
            if (String.IsNullOrEmpty(expression))
            {
                return list;
            }

            string workflowInstanceId = DbUtils.FilterSQL(expression);
            string sql = String.Format("SELECT Creator FROM bwwf_Tracking_Workflows WHERE WorkflowInstanceId = '{0}'", workflowInstanceId);
            object obj = IBatisDbHelper.ExecuteScalar(System.Data.CommandType.Text, sql);
            if (null != obj)
            {
                list.Add(obj.ToString());
            }
            return list;
        }

        #endregion
    }
}
