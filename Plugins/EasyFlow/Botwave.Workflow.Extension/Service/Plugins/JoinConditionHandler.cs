using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Parser;

namespace Botwave.Workflow.Extension.Service.Plugins
{
    /// <summary>
    /// 合并条件处理器实现类.
    /// </summary>
    public class JoinConditionHandler : IJoinConditionHandler
    {
        #region IJoinConditionHandler Members

        /// <summary>
        /// 是否可以合并.
        /// </summary>
        /// <param name="workflowInstanceId">流程实例Id.</param>
        /// <param name="ifSelectedActivities">如果选择了的活动.</param>
        /// <param name="mustCompletedActivities">需要完成的活动.</param>
        /// <returns></returns>
        public bool CanJoin(Guid workflowInstanceId, IList<string> ifSelectedActivities, IList<string> mustCompletedActivities)
        {
            int countOfIfSelectedActivities = ifSelectedActivities.Count;
            int countOfMustCompletedActivities = mustCompletedActivities.Count;
            if (countOfIfSelectedActivities != countOfMustCompletedActivities)
            {
                throw new Botwave.Workflow.WorkflowException("流程合并条件配置错误");
            }

            /* 获取指定流程实例的已完成活动列表 completedList (按时间倒序排列) 及未完成活动列表 todoList中
             * 将todoList中的活动加入已选择活动字典selectedActivities中
             * 将completedList中的活动分别加入已选择活动字典selectedActivities及已完成活动字典completedActivities中,如果有多个,只取第一个
             * 
             * 遍历 activity in ifSelectedActivities
             *  if activity 在 selectedActivities中
             *      if mustCompletedActivities的对应位置的activity 不在 completedActivities中
             *          can not join
             */
            bool canJoin = true;
            IDictionary<string, bool> selectedActivities = new Dictionary<string, bool>();
            string sql = String.Format(@"select a.ActivityName
from bwwf_Tracking_Activities as tac 
	left join bwwf_Activities as a on tac.ActivityId = a.ActivityId
where tac.WorkflowInstanceId = '{0}';
select a.ActivityName
from bwwf_Tracking_Activities_Completed as tac 
	left join bwwf_Activities as a on tac.ActivityId = a.ActivityId
where tac.WorkflowInstanceId = '{0}' order by tac.CreatedTime", workflowInstanceId);
            using (IDataReader reader = IBatisDbHelper.ExecuteReader(CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    string activityName = reader.GetString(0);
                    if (!selectedActivities.ContainsKey(activityName))
                    {
                        selectedActivities.Add(activityName, false);
                    }
                }

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        string activityName = reader.GetString(0);
                        if (!selectedActivities.ContainsKey(activityName))
                        {
                            selectedActivities.Add(activityName, true);
                        }
                    }
                }
            }

            for (int i = 0; i < countOfIfSelectedActivities; i++)
            {
                string activityName = ifSelectedActivities[i];
                if (selectedActivities.ContainsKey(activityName))
                {
                    string mustCompletedActivityName = mustCompletedActivities[i];
                    if (!(selectedActivities.ContainsKey(mustCompletedActivityName)
                        && selectedActivities[mustCompletedActivityName]))
                    {
                        canJoin = false;
                        break;
                    }
                }
            }

            return canJoin;
        }

        #endregion
    }
}
