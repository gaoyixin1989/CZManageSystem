using System;
using System.Collections.Generic;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Extension.IBatisNet;

namespace Botwave.Workflow.IBatisNet
{
    public class ActivitySetService : IActivitySetService
    {
        #region IActivitySetService ≥…‘±

        public IList<ActivitySet> GetActivitySets(Guid activitySetId)
        {
            return IBatisMapper.Select<ActivitySet>("bwwf_ActivitySet_Select", activitySetId);
        }

        public IList<Guid> GetActivityIdSets(Guid activitySetId)
        {
            IList<ActivitySet> activitySets = this.GetActivitySets(activitySetId);
            IList<Guid> activityIdSet = new List<Guid>();
            foreach (ActivitySet item in activitySets)
            {
                activityIdSet.Add(item.ActivityId);
            }
            return activityIdSet;
        }

        public IList<ActivitySet> GetNextActivitySets(Guid workflowId)
        {
            return IBatisMapper.Select<ActivitySet>("bwwf_ActivitySet_Select_Next_By_WorkflowId", workflowId);
        }

        #endregion
    }
}
