using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Entities;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.XQP.Domain;

namespace Botwave.XQP.Service.Support
{
    public class ReviewPending : IReviewPending
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ReviewPending));

        private IActivityDefinitionService activityDefinitionService;
        private IActivityService activityService;

        public IActivityDefinitionService ActivityDefinitionService
        {
            set { activityDefinitionService = value; }
        }

        public IActivityService ActivityService
        {
            set { activityService = value; }
        }

        #region IReviewPending 成员

        public bool Pending(WorkflowProfile workflowProfile, BasicUser sender, string workflowTitle, Guid workflowId, Guid currentActivityInstanceId, 
            bool isManual, ICollection<Guid> selectedActivities, IList<ToReview.ReviewActor> actors)
        {
            if (workflowProfile == null || workflowProfile.IsReview == false
                || (isManual && (actors == null || actors.Count == 0)))
                return false;

            string senderName = sender.UserName;
            string senderRealName = sender.RealName;
            selectedActivities = (selectedActivities == null ? new List<Guid>() : selectedActivities);

            // 消息内容.
            string message = workflowProfile.ReviewNotifyMessage;
            if (!string.IsNullOrEmpty(message))
            {
                message = message.ToLower().Replace("#title#", workflowTitle).Replace("#from#", senderRealName);
            }

            IList<ActivityInstance> nextActivityInstances = activityService.GetNextActivities(currentActivityInstanceId);  // 获取下行活动实例.
            IDictionary<Guid, ActivityInstance> nextActivityDictionary = new Dictionary<Guid, ActivityInstance>();  // 下行活动实例字典.

            IList<Guid> nextActivityDefinitionId = new List<Guid>();   // 无下行实例时取得的下行活动定义编号列表.
            if (nextActivityInstances == null || nextActivityInstances.Count == 0)
            {
                log.Info("未能找到下行步骤列表(发送给当前步骤). ActivityInstanceId:" + currentActivityInstanceId.ToString());
                // 获取下行定义编号列表.
                if (selectedActivities.Count > 0)
                {
                    foreach (Guid key in selectedActivities)
                    {
                        if (!nextActivityDefinitionId.Contains(key))
                            nextActivityDefinitionId.Add(key);
                    }
                }
                else
                {
                    IList<ActivityDefinition> nextActivityDefinitions = GetNextActivities(currentActivityInstanceId);
                    foreach (ActivityDefinition nextDefinition in nextActivityDefinitions)
                    {
                        if (!nextActivityDefinitionId.Contains(nextDefinition.ActivityId))
                            nextActivityDefinitionId.Add(nextDefinition.ActivityId);
                    }
                }
            }
            else
            {
                nextActivityDefinitionId = GetActivityIdList(nextActivityInstances);
                nextActivityDictionary = GetActivityInstanceDictionary(nextActivityInstances);
            }

            if ((actors == null || actors.Count == 0) && !isManual && nextActivityDefinitionId.Count > 0)
            {
                actors = ToReview.GetReviewActors(workflowId, nextActivityDefinitionId);
            }

            if (actors == null || actors.Count == 0)
                return false;

            foreach (ToReview.ReviewActor item in actors)
            {
                if (nextActivityDictionary.ContainsKey(item.ActivityId))
                    item.ActivityInstanceId = nextActivityDictionary[item.ActivityId].ActivityInstanceId;
                else
                    item.ActivityInstanceId = currentActivityInstanceId;
            }

            return ToReview.OnPendingReview(currentActivityInstanceId, message, senderName, workflowId, workflowTitle, actors);
        }

        public bool Pending(WorkflowProfile workflowProfile, BasicUser sender, string workflowTitle, Guid workflowId, Guid activityInstanceId)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// 获取指定活动实例列表的流程活动定义编号列表.
        /// </summary>
        /// <param name="activities"></param>
        /// <returns></returns>
        public IList<Guid> GetActivityIdList(IList<ActivityInstance> activities)
        {
            if (activities == null || activities.Count == 0)
                return new List<Guid>();

            IList<Guid> results = new List<Guid>();
            foreach (ActivityInstance item in activities)
            {
                if (!results.Contains(item.ActivityId))
                    results.Add(item.ActivityId);
            }
            return results;
        }

        /// <summary>
        /// 获取下行流程定义编号集合.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        public IList<ActivityDefinition> GetNextActivities(Guid activityInstanceId)
        {
            IList<ActivityDefinition> nextActivities = activityDefinitionService.GetNextActivityDefinitionsByInstanceId(activityInstanceId);
            if (nextActivities != null && nextActivities.Count > 1)
            {
                ActivityDefinition currentActivity = activityDefinitionService.GetActivityDefinitionByInstanceId(activityInstanceId);
                for (int i = 0; i < nextActivities.Count; i++)
                {
                    if (nextActivities[i].ActivityId == currentActivity.ActivityId)
                    {
                        nextActivities.RemoveAt(i);
                        i--;
                    }
                }
            }
            return ((nextActivities == null || nextActivities.Count == 0) ? new List<ActivityDefinition>() : nextActivities);
        }

        /// <summary>
        /// 获取流程步骤实例字典(key: ActivityID).
        /// </summary>
        /// <param name="activities"></param>
        /// <returns></returns>
        public IDictionary<Guid, ActivityInstance> GetActivityInstanceDictionary(IList<ActivityInstance> activities)
        {
            IDictionary<Guid, ActivityInstance> results = new Dictionary<Guid, ActivityInstance>();
            if (activities == null || activities.Count == 0)
                return results;

            foreach (ActivityInstance item in activities)
            {
                if (!results.ContainsKey(item.ActivityId))
                    results.Add(item.ActivityId, item);
            }
            return results;
        }
    }
}
