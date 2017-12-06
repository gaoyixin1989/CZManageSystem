using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Extension.IBatisNet;
using System.Data;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// 流程活动实例结果类(潮州).
    /// </summary>
    public class CZActivityInstance : Botwave.Workflow.Domain.ActivityInstance
    {
        private string _previousActors;
        private int _printCount;

        public string PreviousActors
        {
            get { return _previousActors; }
            set { _previousActors = value; }
        }

        /// <summary>
        /// 步骤打印次数
        /// </summary>
        public int PrintCount
        {
            get { return _printCount; }
            set { _printCount = value; }
        }

        public CZActivityInstance()
            : base()
        {   }

        /// <summary>
        /// 获取指定流程实例编号的流程步骤实例列表.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public static IList<CZActivityInstance> GetWorkflowActivities(Guid workflowInstanceId)
        {
            return Botwave.Extension.IBatisNet.IBatisMapper.Select<CZActivityInstance>("cz_bwwf_ActivityInstance_Select_WorkflowInstanceId", workflowInstanceId);
        }

        /// <summary>
        /// 获取指定流程步骤实例编号的流程步骤实例.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public static CZActivityInstance GetWorkflowActivity(Guid activityInstanceId)
        {
            return Botwave.Extension.IBatisNet.IBatisMapper.Load<CZActivityInstance>("cz_bwwf_ActivityInstance_Select_ActivityInstanceId", activityInstanceId);
        }

        /// <summary>
        /// 更新当前活动实例
        /// </summary>
        /// <param name="activityInstance"></param>
        /// <returns></returns>
        public static int WorkflowActivitiesUpdate(CZActivityInstance activityInstance)
        {
            return Botwave.Extension.IBatisNet.IBatisMapper.Update("cz_bwwf_ActivityInstance_Update", activityInstance);
        }

        /// <summary>
        /// 获取指定流程步骤实例 PrevSetId 的上一活动实例列表.
        /// </summary>
        /// <param name="prevSetId"></param>
        /// <returns></returns>
        public static IList<CZActivityInstance> GetPrevActivitiesByPrevSetId(Guid prevSetId)
        {
            return Botwave.Extension.IBatisNet.IBatisMapper.Select<CZActivityInstance>("bwwf_ActivityInstance_Select_PrevActivities_By_PrevSetId", prevSetId);
        }
        //public static int CompletedWorkflowActivitiesPrintCountUpdate()
        //{
        //    string sql = "update bwwf_tracking_activities_completed set printcount=@Number where activityinstnaceid=@Aiid";
        //}

        /// <summary>
        /// 获取过程控制步骤的处理人
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <param name="extendAllocatorArgs"></param>
        /// <returns></returns>
        public static string GetPssorActor(Guid workflowInstanceId, string extendAllocatorArgs)
        {
            string actor = "";
            try
            {
                if (!string.IsNullOrEmpty(extendAllocatorArgs))//
                {
                    foreach (string allocatorArg in extendAllocatorArgs.Replace(" ", "").Split(';', '；'))
                    {
                        string[] ocatorArray = allocatorArg.Split(':', '：');
                        int lengthOfAllocatorArray = ocatorArray.Length;
                        if (lengthOfAllocatorArray == 0)
                            continue;
                        if (ocatorArray[0] == "processctl")
                        {
                            string sql = string.Format(@"select top 1 actor from bwwf_tracking_activities_completed ta
                    inner join bwwf_activities ba 
                    on ta.activityid = ba.activityid 
                    where workflowInstanceId = '{0}' and activityname = '{1}' order by finishedtime asc", workflowInstanceId, ocatorArray[1]);
                            object actor1 = IBatisDbHelper.ExecuteScalar(CommandType.Text, sql);
                            actor = actor1 == null ? actor : actor1.ToString();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Botwave.XQP.Commons.LogWriter.Write("未知用户", ex);
            }
            return actor;
        }

        /// <summary>
        /// 获取过程控制步骤的处理人
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <param name="extendAllocatorArgs"></param>
        /// <returns></returns>
        public static string GetPssorActorByAiid(Guid activityInstanceId, string extendAllocatorArgs)
        {
            string actor = "";
            try
            {
                if (!string.IsNullOrEmpty(extendAllocatorArgs))//
                {
                    foreach (string allocatorArg in extendAllocatorArgs.Replace(" ", "").Split(';', '；'))
                    {
                        string[] ocatorArray = allocatorArg.Split(':', '：');
                        int lengthOfAllocatorArray = ocatorArray.Length;
                        if (lengthOfAllocatorArray == 0)
                            continue;
                        if (ocatorArray[0] == "processctl")
                        {
                            string sql = string.Format(@"select top 1 actor from bwwf_tracking_activities_completed ta
                    inner join bwwf_activities ba 
                    on ta.activityid = ba.activityid 
                    where workflowInstanceId = (select workflowinstanceid from vw_bwwf_tracking_activities_all where activityinstanceid='{0}') and activityname = '{1}' order by finishedtime desc", activityInstanceId, ocatorArray[1]);
                            object actor1 = IBatisDbHelper.ExecuteScalar(CommandType.Text, sql);
                            actor = actor1 == null ? actor : actor1.ToString();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Botwave.XQP.Commons.LogWriter.Write("未知用户", ex);
            }
            return actor;
        }
    }
}
