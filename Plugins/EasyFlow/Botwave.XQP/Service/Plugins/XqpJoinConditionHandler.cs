using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Parser;

namespace Botwave.XQP.Service.Plugin
{
    public class XqpJoinConditionHandler : IJoinConditionHandler
    {
        #region IJoinConditionHandler Members

        public bool CanJoin(Guid workflowInstanceId, IList<string> ifSelectedActivities, IList<string> mustCompletedActivities)
        {
            

            /* 获取指定流程实例的已完成活动列表 completedList (按时间倒序排列) 及未完成活动列表 todoList中
             * 将todoList中的活动加入已选择活动字典selectedActivities中
             * 将completedList中的活动分别加入已选择活动字典selectedActivities及已完成活动字典completedActivities中,如果有多个,只取第一个
             * 
             * 遍历 activity in ifSelectedActivities
             *  if activity 在 selectedActivities中
             *      if mustCompletedActivities的对应位置的activity 不在 completedActivities中
             *          can not join
             *  end
             *  if ifSelectedActivities的集合为1 and mustCompletedActivities对象为空 and ifSelectedActivities[0] equals "all"
             *      if bwwf_Tracking_Activities 中还有活动集合
             *          can not join
             *  end
             */
            int countOfIfSelectedActivities = ifSelectedActivities.Count;
            if (countOfIfSelectedActivities == 1 && mustCompletedActivities == null)
            {
                if (ifSelectedActivities[0].Equals("all"))//判断前驱步骤是否完全完成
                {
                    object count = IBatisDbHelper.ExecuteScalar(CommandType.Text, string.Format("select count(0) from bwwf_Tracking_Activities where WorkflowInstanceId = '{0}'", workflowInstanceId.ToString()));
                    return Botwave.Commons.DbUtils.ToInt32(count) == 0;
                }
                throw new Botwave.Workflow.WorkflowException("流程合并条件配置错误");
            }
            int countOfMustCompletedActivities = mustCompletedActivities.Count;
            if (countOfIfSelectedActivities != countOfMustCompletedActivities)
            {
                throw new Botwave.Workflow.WorkflowException("流程合并条件配置错误");
            }
            bool canJoin = true;
            IDictionary<string, bool> selectedActivities = new Dictionary<string, bool>();
            string sql = String.Format(@"select a.ActivityName
from bwwf_Tracking_Activities tac 
  left join bwwf_Activities a on tac.ActivityId = a.ActivityId
where tac.WorkflowInstanceId = '{0}'
", workflowInstanceId.ToString().ToUpper());
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

                
            }
            sql = String.Format(@"select a.ActivityName
from bwwf_Tracking_Activities_Completed tac 
  left join bwwf_Activities a on tac.ActivityId = a.ActivityId
where tac.WorkflowInstanceId = '{0}' order by tac.CreatedTime", workflowInstanceId.ToString().ToUpper());
            using (IDataReader reader = IBatisDbHelper.ExecuteReader(CommandType.Text, sql))
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
