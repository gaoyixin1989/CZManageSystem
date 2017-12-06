using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Entities;
using Botwave.Workflow;
using Botwave.XQP.Domain;

namespace Botwave.XQP.Service
{
    public interface IReviewPending
    {
        bool Pending(WorkflowProfile workflowProfile, BasicUser sender, string workflowTitle, Guid workflowId, Guid currentActivityInstanceId, 
            bool isManual, ICollection<Guid> selectedActivities, IList<ToReview.ReviewActor> actors);
    }
}
