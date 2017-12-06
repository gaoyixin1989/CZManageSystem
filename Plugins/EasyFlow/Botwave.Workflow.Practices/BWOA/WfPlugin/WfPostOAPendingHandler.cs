using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Workflow.Domain;
using Botwave.Extension.IBatisNet;
using System.Collections;

namespace Botwave.Workflow.Practices.BWOA.WfPlugin
{
    public class WfPostOAPendingHandler
    {
        public void InsertOAActivityInstance_Completed(ActivityInstance activityInstance)
        {
            Hashtable ht = new Hashtable();

            ht.Add("ActivityInstanceId", activityInstance.ActivityInstanceId);
            ht.Add("PrevSetId", activityInstance.PrevSetId );
            ht.Add("WorkflowInstanceId", activityInstance.WorkflowInstanceId );
            ht.Add("ActivityId", activityInstance.ActivityId );
            ht.Add("OperateType", activityInstance.OperateType );
            ht.Add("CreatedTime", activityInstance.CreatedTime );
            ht.Add("Actor", activityInstance.Actor );
            ht.Add("ActorDescription", activityInstance.ActorDescription);
            ht.Add("Command", activityInstance.Command );
            ht.Add("Reason", activityInstance.Reason );
            ht.Add("ExternalEntityType", activityInstance.ExternalEntityType );
            ht.Add("ExternalEntityId", activityInstance.ExternalEntityId);

            IBatisMapper.Insert("bwwf_OAActivityInstance_Completed_Insert", ht);
        }

        public Guid SelectActivitiesByActivityId(Guid workflowId)
        {
            object obj = IBatisMapper.Mapper.QueryForObject("oa_WorkflowActivityID_Select_WorkflowID", workflowId);
            if (obj == null)
                return Guid.Empty;
            else
                return new Guid(obj.ToString());
        }
    }
}
