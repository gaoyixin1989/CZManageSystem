using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Plugin;

namespace Botwave.XQP.Service.Plugins
{
    public class PostCloseParallelActivityInstancesHandler : IPostCloseParallelActivityInstancesHandler
    {
        #region IPostCloseParallelActivityInstancesHandler Members

        public void Execute(ActivityInstance currentActivityInstance, ICollection<ActivityInstance> parallelInstances)
        {
            foreach (ActivityInstance instance in parallelInstances)
            {
                if (!instance.IsCompleted && instance.ActivityInstanceId != currentActivityInstance.ActivityInstanceId)
                {
                    WorkflowPostHelper.PostCloseActivityInstance(instance.ActivityInstanceId);
                }
            }
        }

        #endregion
    }
}
