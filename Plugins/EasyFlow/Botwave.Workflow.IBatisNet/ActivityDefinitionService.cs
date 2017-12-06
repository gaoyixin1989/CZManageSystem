using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Commons;
using Botwave.Entities;
using Botwave.Workflow.Allocator;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Extension.IBatisNet;

namespace Botwave.Workflow.IBatisNet
{
    public class ActivityDefinitionService : IActivityDefinitionService
    {
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ActivityDefinitionService));

        #region IActivityDefinitionService Members

        public ActivityDefinition GetInitailActivityDefinition(Guid workflowId)
        {
            IList<ActivityDefinition> list = IBatisMapper.Select<ActivityDefinition>("bwwf_Activity_Select_Initial_By_WorkflowId", workflowId);
            return GetFirstActivityDefinition(list);
        }

        public IList<ActivityDefinition> GetInitailActivityDefinitionList(Guid workflowId, string activityName)
        {
            Hashtable ha = new Hashtable();
            ha.Add("workflowId", workflowId.ToString ());
            ha.Add("activityName", activityName);
            IList<ActivityDefinition> list = IBatisMapper.Select<ActivityDefinition>("bwwf_Activity_Select_Initial_By_WorkflowList", ha);
            return list;
        }

        public DataTable GetInitailActivityDefinitionList(Guid workflowInstanceId, Guid activityId)
        {
            string sql = string.Format(@"select distinct  cast( ta.ActivityId as varchar(50))+'_'+Actor as Names, a.ActivityName, u.RealName, a.SortOrder
                    from bwwf_Tracking_Activities_Completed ta
	                    left join bwwf_Activities a on a.ActivityId = ta.ActivityId
	                    left join bw_Users u on u.UserName = Actor
                    where (IsCompleted=1 and workflowinstanceid='{0}' and ta.ActivityId<>'{1}')
                    order by a.SortOrder", workflowInstanceId, activityId);

            return SqlHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }


        public IList<ActivityDefinition> GetStartActivities(Guid workflowId)
        {
            return IBatisMapper.Select<ActivityDefinition>("bwwf_Activity_Select_Start_By_WorkflowId", workflowId);
        }

        public IList<ActivityDefinition> GetStartActivitiesByWorkflowInstanceId(Guid workflowInstanceId)
        {
            return IBatisMapper.Select<ActivityDefinition>("bwwf_Activity_Select_Start_By_WorkflowInstanceId", workflowInstanceId);
        }

        public ActivityDefinition GetActivityDefinition(Guid activityId)
        {
            IList<ActivityDefinition> list = IBatisMapper.Select<ActivityDefinition>("bwwf_Activity_Select", activityId);
            return GetFirstActivityDefinition(list);
        }

        public ActivityDefinition GetActivityDefinitionByInstanceId(Guid activityInstanceId)
        {
            IList<ActivityDefinition> list = IBatisMapper.Select<ActivityDefinition>("bwwf_Activity_Select_By_ActivityInstanceId", activityInstanceId);
            return GetFirstActivityDefinition(list);
        }

        public IList<ActivityDefinition> GetPrevActivityDefinitions(Guid activityId)
        {
            return IBatisMapper.Select<ActivityDefinition>("bwwf_Activity_Select_PrevDefinitions", activityId);
        }

        public IList<ActivityDefinition> GetPrevActivityDefinitionsByInstanceId(Guid activityInstaceId)
        {
            return IBatisMapper.Select<ActivityDefinition>("bwwf_Activity_Select_Prev_By_ActivityInstanceId", activityInstaceId);
        }

        public IList<ActivityDefinition> GetNextActivityDefinitionsByInstanceId(Guid activityInstaceId)
        {
            return IBatisMapper.Select<ActivityDefinition>("bwwf_Activity_Select_Next_By_ActivityInstanceId", activityInstaceId);
        }

        public IList<ActivityDefinition> GetActivityDefinitionsByActivityNames(Guid workflowId, string[] activityNames)
        {
            if (null == activityNames || activityNames.Length == 0)
            {
                return null;
            }

            IDictionary<string, string> dict = new Dictionary<string, string>();
            for (int i = 0, icount = activityNames.Length; i < icount; i++)
            {
                if (!dict.ContainsKey(activityNames[i]))
                {
                    dict.Add(activityNames[i], null);
                }
            }

            IList<ActivityDefinition> list = IBatisMapper.Select<ActivityDefinition>("bwwf_Activity_Select_By_WorkflowId", workflowId);

            IList<ActivityDefinition> results = new List<ActivityDefinition>();
            foreach (ActivityDefinition ad in list)
            {
                if (dict.ContainsKey(ad.ActivityName))
                {
                    results.Add(ad);
                }
            }
            return results;
        }
        public IList<ActivityDefinition> GetActivitiesByWorkflowId(Guid workflowId)
        {
            return IBatisMapper.Select<ActivityDefinition>("bwwf_Activity_Select_By_WorkflowId", workflowId);
        }

        public IList<ActivityDefinition> GetPartActivities(Guid workflowId, Guid currentActivityId)
        {
            IList<ActivityDefinition> source = this.GetSortedActivitiesByWorkflowId(workflowId);
            IList<ActivityDefinition> activities = new List<ActivityDefinition>();
            foreach (ActivityDefinition item in source)
            {
                activities.Add(item);
                if (item.ActivityId == currentActivityId)
                    break;
            }

            return activities;
        }
        
        public IList<ActivityDefinition> GetSortedActivitiesByWorkflowId(Guid workflowId)
        {
            IList<SortActivity> sortedActivities = new List<SortActivity>();
            try
            {
                string sql = @" SELECT WorkflowId, ActivityId, ActivityName, State, SortOrder,PrevActivitySetId, NextActivitySetId,JoinCondition,SplitCondition,CommandRules,ExecutionHandler,PostHandler,AllocatorResource,AllocatorUsers,ExtendAllocators,ExtendAllocatorArgs,DefaultAllocator,DecisionType,DecisionParser,CountersignedCondition,RejectOption 
                                        FROM bwwf_Activities WHERE WorkflowId = '{0}' order by State, SortOrder, AllocatorResource";
                sql = string.Format(sql, workflowId);
                DataSet ds = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql);
                DataTable activityTable = ds.Tables[0];

                int count = activityTable.Rows.Count;
                // 取的初始步骤和结束步骤
                DataRow[] startRows = activityTable.Select(" PrevActivitySetId = '00000000-0000-0000-0000-000000000000'");
                DataRow[] endRows = activityTable.Select(" NextActivitySetId = '00000000-0000-0000-0000-000000000000'");

                if (startRows == null || startRows.Length != 1)
                    throw new DataException(string.Format("{0}:流程的初始步骤不存在或者有多个初始步骤！", workflowId));
                if (endRows == null || endRows.Length != 1)
                    throw new DataException(string.Format("{0}:流程的结束步骤不存在或者有多个结束步骤！", workflowId));
                SortActivity startActivity = (SortActivity)ToSortActivity(startRows[0]);
                SortActivity endActivity = (SortActivity)ToSortActivity(endRows[0]);

                // 获取 ActivitySet
                sql = @"SELECT SetId, ActivityId FROM bwwf_ActivitySet 
                        WHERE (SetId IN (SELECT PrevActivitySetId FROM bwwf_Activities WHERE WorkflowId = '{0}'))
                        union all 
                        SELECT SetId, ActivityId FROM bwwf_ActivitySet 
                        where (SetId IN (SELECT NextActivitySetId FROM bwwf_Activities WHERE WorkflowId = '{0}')) ";
                sql = string.Format(sql, workflowId);
                ds = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql);
                DataTable activitySetTable = ds.Tables[0];

                foreach (DataRow row in activityTable.Rows)
                {
                    SortActivity current = ToSortActivity(row);
                    DataRow[] prevActivitySet = activitySetTable.Select(string.Format(" SetId = '{0}'", current.PrevActivitySetId));
                    DataRow[] nextActivitySet = activitySetTable.Select(string.Format(" SetId = '{0}'", current.NextActivitySetId));
                    if (prevActivitySet != null && prevActivitySet.Length > 0)
                    {
                        foreach (DataRow prevItem in prevActivitySet)
                        {
                            DataRow[] prevActivities = activityTable.Select(string.Format(" ActivityId = '{0}'", prevItem["ActivityId"]));
                            if (prevActivities != null && prevActivities.Length > 0)
                            {
                                string prevActivityName = DbUtils.ToString(prevActivities[0]["ActivityName"]);
                                current.PrevActivityNames.Add(prevActivityName);
                            }
                        }
                    }
                    if (nextActivitySet != null && nextActivitySet.Length > 0)
                    {
                        foreach (DataRow nextItem in nextActivitySet)
                        {
                            DataRow[] nextActivities = activityTable.Select(string.Format(" ActivityId = '{0}'", nextItem["ActivityId"]));
                            if (nextActivities != null && nextActivities.Length > 0)
                            {
                                string nextActivityName = DbUtils.ToString(nextActivities[0]["ActivityName"]);
                                current.NextActivityNames.Add(nextActivityName);
                            }
                        }
                    }
                    sortedActivities.Add(current);
                }

                int sortedCount = sortedActivities.Count;

                // 将完成步骤移到最后位置.
                if (sortedActivities[sortedCount - 1].State != 2)
                {
                    // 没将完成节点放在列表的位置
                    for (int i = 0; i < sortedActivities.Count; i++)
                    {
                        if (sortedActivities[i].State == 2)
                        {
                            sortedActivities.RemoveAt(i);
                            i--;
                            break;
                        }
                    }
                    endActivity.Index = sortedActivities.Count - 1;
                    sortedActivities.Add(endActivity);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            IList<ActivityDefinition> activities = new List<ActivityDefinition>();
            foreach (SortActivity item in sortedActivities)
            {
                activities.Add(item);
            }
            return activities;
        }

        [Obsolete]
        public IList<ActivityDefinition> GetSortedActivitiesByWorkflowId_Obsolete(Guid workflowId)
        {
            IList<SortActivity> sortedActivities = new List<SortActivity>();
            try
            {
                string sql = "SELECT WorkflowId, ActivityId, ActivityName, State, SortOrder,PrevActivitySetId, NextActivitySetId,JoinCondition,SplitCondition,CommandRules,ExecutionHandler,PostHandler,AllocatorResource,AllocatorUsers,ExtendAllocators,ExtendAllocatorArgs,DefaultAllocator,DecisionType,DecisionParser,CountersignedCondition,RejectOption FROM bwwf_Activities ";
                sql += string.Format(" WHERE WorkflowId = '{0}' order by State, SortOrder, AllocatorResource", workflowId.ToString());
                DataSet ds = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql);
                DataTable activityTable = ds.Tables[0];

                // 获取 ActivitySet
                sql = @"SELECT SetId, ActivityId FROM bwwf_ActivitySet WHERE SetId IN(
                                    (SELECT PrevActivitySetId FROM bwwf_Activities WHERE WorkflowId = '{0}')
                                    UNION
                                    (SELECT NextActivitySetId FROM bwwf_Activities WHERE WorkflowId = '{0}')
                             )";
                sql = string.Format(sql, workflowId);
                ds = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql);
                DataTable setTable = ds.Tables[0];

                int count = activityTable.Rows.Count;
                // 取的初始步骤和结束步骤
                DataRow[] startRows = activityTable.Select(" PrevActivitySetId = '00000000-0000-0000-0000-000000000000'");
                DataRow[] endRows = activityTable.Select(" NextActivitySetId = '00000000-0000-0000-0000-000000000000'");
                
                if (startRows == null || startRows.Length != 1)
                    throw new DataException(string.Format("{0}:流程的初始步骤不存在或者有多个初始步骤！", workflowId));
                if (endRows == null || endRows.Length != 1)
                    throw new DataException(string.Format("{0}:流程的结束步骤不存在或者有多个结束步骤！", workflowId));

                SortActivity current = (SortActivity)ToSortActivity(startRows[0]);
                SortActivity endActivity = (SortActivity)ToSortActivity(endRows[0]);

                int sortIndex = 0;
                current.Index = sortIndex;
                SetNextActivities(sortedActivities, activityTable, setTable, current, sortIndex);
                int sortedCount = sortedActivities.Count;
                sortIndex = sortedCount;
                SetPrevActivities(sortedActivities, activityTable, setTable, endActivity, sortIndex);

                // 将完成步骤移到最后位置.
                if (sortedActivities[sortedCount - 1].State != 2)
                {
                    // 没将完成节点放在列表的位置
                    for (int i = 0; i < sortedActivities.Count; i++)
                    {
                        if (sortedActivities[i].State == 2)
                        {
                            sortedActivities.RemoveAt(i);
                            i--;
                            break;
                        }
                    }
                    endActivity.Index = sortedActivities.Count - 1;
                    sortedActivities.Add(endActivity);
                }
            }
            catch (Exception ex)
            {
                log.Info(ex);
            }

            IList<ActivityDefinition> activities = new List<ActivityDefinition>();
            foreach (SortActivity item in sortedActivities)
            {
                activities.Add(item);
            }
            return activities;
        }

        public IList<ActivityDefinition> GetAllActivityDefinition(Guid activityInstanceId)
        {
            return IBatisMapper.Select<ActivityDefinition>("bwwf_Activity_Select_All_By_ActivityInstanceId", activityInstanceId);
        }

        public int GetActivityCountByWorkflowId(Guid workflowId)
        {
            return IBatisMapper.Mapper.QueryForObject<int>("bwwf_Activity_Select_Count_By_WorkflowId", workflowId);
        }

        public int UpdateActivityAllocators(ActivityDefinition activity)
        {
            return IBatisMapper.Update("bwwf_Activities_Update_Allocators", activity);
        }

        #region 流程路由

        public IList<WorkflowRoute> GetWorkflowRoute(Guid workflowId, Guid startActivityId)
        {
            IList<WorkflowRoute> routes = new List<WorkflowRoute>();
            IList<ActivitySet> activitySets = IBatisMapper.Select<ActivitySet>("bwwf_ActivitySet_Select_By_WorkflowId", workflowId);    // 获取全部步骤的关系集合
            IDictionary<Guid, ActivityDefinition> activityDict = IBatisMapper.Mapper.QueryForDictionary<Guid, ActivityDefinition>("bwwf_Activity_Select_By_WorkflowId", workflowId, "ActivityId");

            if (!activityDict.ContainsKey(startActivityId))
                return routes;

            int setCount = activitySets.Count;
            IList<SortActivity> sortActivities = new List<SortActivity>();
            int index = 1;
            this.SetWorkflowRoute(activityDict, startActivityId, sortActivities, activitySets, setCount, index);

            StringBuilder builder = new StringBuilder();
            foreach (SortActivity item in sortActivities)
            {
                builder.AppendLine(item.Index.ToString() + "-" + item.ActivityName + "  ");
            }
            log.Info(builder.ToString());

            // 依排序步骤列表中的步骤排序规律，通过对其序号进行比较获取流程路由列表
            int sortCount = sortActivities.Count;
            index = 0;
            WorkflowRoute route = new WorkflowRoute();
            for (int sortIndex = 0; sortIndex < sortCount; sortIndex++)
            {
                SortActivity item = sortActivities[sortIndex];
                if (item.Index > index)
                {
                    route.Add(item);
                    // 保存最后一个路由
                    if (sortIndex == sortCount - 1)
                        routes.Add(route);
                }
                else
                {
                    // 新分支（路由）开始
                    if (route != null && route.Count > 0)
                        routes.Add(route);
                    int endIndex = item.Index;
                    // 从排序列表中序号规律来看,需要将其前面几个一同复制到新流程路由中，才能形成完整的路由路径
                    route = route.SubRoute(0, endIndex - 1); 
                    route.Add(item);
                }
                index = item.Index;
            }
            return routes;
        }

        private void SetWorkflowRoute(IDictionary<Guid, ActivityDefinition> dict, Guid currentId, IList<SortActivity> sortActivities, IList<ActivitySet> activitySets, int activitySetCount, int index)
        {
            SortActivity current = new SortActivity(dict[currentId]);
            current.Index = index;
            sortActivities.Add(current);
            index++;

            IList<Guid> nextActivityIds = GetNextActivityIds(activitySets, activitySetCount, current.NextActivitySetId);
            if (current.NextActivitySetId == Guid.Empty)
            {
                return;
            }

            foreach (Guid item in nextActivityIds)
            {
                this.SetWorkflowRoute(dict, item, sortActivities, activitySets, activitySetCount, index);
            }
        }

        private static IList<Guid> GetNextActivityIds(IList<ActivitySet> activitySets, int count, Guid setId)
        {
            IList<Guid> results = new List<Guid>();
            for (int i = 0; i < count; i++)
            {
                if (activitySets[i].SetId == setId)
                    results.Add(activitySets[i].ActivityId);
            }
            return results;
        }

        #endregion

        #endregion

        #region 辅助方法

        private static ActivityDefinition GetFirstActivityDefinition(IList<ActivityDefinition> list)
        {
            if (null == list || list.Count == 0)
                return null;
            return list[0];
        }

        #region 排序

        /// <summary>
        /// 将 ActivityDefinition 的数据行转换为 ActivityDefinition 实例对象.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static SortActivity ToSortActivity(DataRow row)
        {
            SortActivity item = new SortActivity();
            item.WorkflowId = new Guid(row["WorkflowId"].ToString());
            item.ActivityId = new Guid(row["ActivityId"].ToString());
            item.ActivityName = DbUtils.ToString(row["ActivityName"]);
            item.State = DbUtils.ToInt32(row["State"]);
            item.SortOrder = DbUtils.ToInt32(row["SortOrder"]);
            item.PrevActivitySetId = (row["PrevActivitySetId"] == DBNull.Value ? Guid.Empty : new Guid(row["PrevActivitySetId"].ToString()));
            item.NextActivitySetId = (row["NextActivitySetId"] == DBNull.Value ? Guid.Empty : new Guid(row["NextActivitySetId"].ToString()));
            item.JoinCondition = DbUtils.ToString(row["JoinCondition"]);
            item.SplitCondition = DbUtils.ToString(row["SplitCondition"]);
            item.CommandRules = DbUtils.ToString(row["CommandRules"]);
            item.ExecutionHandler = DbUtils.ToString(row["ExecutionHandler"]);
            item.PostHandler = DbUtils.ToString(row["PostHandler"]);
            item.AllocatorResource = DbUtils.ToString(row["AllocatorResource"]);
            item.AllocatorUsers = DbUtils.ToString(row["AllocatorUsers"]);
            item.ExtendAllocators = DbUtils.ToString(row["ExtendAllocators"]);
            item.ExtendAllocatorArgs = DbUtils.ToString(row["ExtendAllocatorArgs"]);
            item.DefaultAllocator = DbUtils.ToString(row["DefaultAllocator"]);
            item.DecisionType = DbUtils.ToString(row["DecisionType"]);
            item.CountersignedCondition = DbUtils.ToString(row["CountersignedCondition"]);
            item.RejectOption = DbUtils.ToString(row["RejectOption"]);

            return item;
        }

        private static void SetPrevActivities(IList<SortActivity> sortedActivities, DataTable activityTable, DataTable activitySetTable, SortActivity current, int sortIndex)
        {
            current.Index = sortIndex;
            sortIndex--;
            AppendActivity(sortedActivities, current);
            if (current.PrevActivitySetId == Guid.Empty)
                return;
            DataRow[] prevSets = activitySetTable.Select(string.Format(" SetId = '{0}'", current.PrevActivitySetId.ToString()));
            if (prevSets != null && prevSets.Length > 0)
            {
                IList<SortActivity> prevActivities = GetActivityRows(activityTable, prevSets);
                int count = prevActivities.Count;
                for (int i = 0; i < count; i++)
                {
                    current.PrevActivityNames.Add(prevActivities[i].ActivityName); 
                }
                for (int i = 0; i < count; i++)
                {
                    SortActivity prevActivity = prevActivities[i];
                    if (prevActivity.ActivityId != current.ActivityId)
                        SetPrevActivities(sortedActivities, activityTable, activitySetTable, prevActivity, sortIndex);
                }
            }
        }

        private static void SetNextActivities(IList<SortActivity> sortedActivities, DataTable activityTable, DataTable activitySetTable, SortActivity current, int sortIndex)
        {
            current.Index = sortIndex;
            sortIndex++;
            AppendActivity(sortedActivities, current);
            if (current.NextActivitySetId == Guid.Empty)
                return;
            DataRow[] nextSets = activitySetTable.Select(string.Format(" SetId = '{0}'", current.NextActivitySetId.ToString()));

            if (nextSets != null && nextSets.Length > 0)
            {
                IList<SortActivity> nextActivities = GetActivityRows(activityTable, nextSets);
                int count = nextActivities.Count;
                for (int i = 0; i < count; i++)
                {
                    current.NextActivityNames.Add(nextActivities[i].ActivityName);
                }

                for (int i = 0; i < count; i++)
                {
                    SetNextActivities(sortedActivities, activityTable, activitySetTable, nextActivities[i], sortIndex);
                }
            }
        }

        private static bool AppendActivity(IList<SortActivity> definitions, SortActivity item)
        {
            int count = definitions.Count;
            for (int i = 0; i < count; i++)
            {
                if (definitions[i].ActivityId == item.ActivityId)
                    return true;
            }

            int index = -1;
            for (int i = 0; i < count; i++)
            {
                if (definitions[i].Index == item.Index)
                {
                    index = i;
                }
            }
            if (index == -1)
                definitions.Add(item);
            else
                definitions.Insert(index, item);

            return false;
        }

        /// <summary>
        /// 获取活动数据行
        /// </summary>
        /// <param name="activityTable"></param>
        /// <param name="activitySetRows"></param>
        /// <returns></returns>
        private static IList<SortActivity> GetActivityRows(DataTable activityTable, DataRow[] activitySetRows)
        {
            IList<SortActivity> acitivities = new List<SortActivity>();
            int count = activitySetRows.Length;

            for (int i = 0; i < count; i++)
            {
                DataRow[] results = activityTable.Select(string.Format(" ActivityId = '{0}'", activitySetRows[i]["ActivityId"].ToString()));
                if (results != null)
                    acitivities.Add(ToSortActivity(results[0]));
            }

            return acitivities;
        }

        #endregion

        #endregion

        #region 被排序的活动类

        /// <summary>
        /// 被排序的活动类
        /// </summary>
        private class SortActivity : ActivityDefinition
        {
            private int _index;

            /// <summary>
            /// 构造方法.
            /// </summary>
            public SortActivity()
                : this(0)
            { }

            /// <summary>
            /// 构造方法.
            /// </summary>
            /// <param name="index"></param>
            public SortActivity(int index)
                : base()
            {
                this._index = index;
            }

            /// <summary>
            /// 构造方法.
            /// </summary>
            /// <param name="activity"></param>
            public SortActivity(ActivityDefinition activity)
            {
                this.ActivityId = activity.ActivityId;
                this.ActivityName = activity.ActivityName;
                this.WorkflowId = activity.WorkflowId;
                this.State = activity.State;
                this.AllocatorResource = activity.AllocatorResource;
                this.PrevActivitySetId = activity.PrevActivitySetId;
                this.NextActivitySetId = activity.NextActivitySetId;
                this.CommandRules = activity.CommandRules;
                this.ExecutionHandler = activity.ExecutionHandler;
                this.PostHandler = activity.PostHandler;
                this.JoinCondition = activity.JoinCondition;
                this.SplitCondition = activity.SplitCondition;
            }

            /// <summary>
            /// 活动索引.
            /// </summary>
            public int Index
            {
                get { return _index; }
                set { _index = value; }
            }
        }

        #endregion
    }
}
