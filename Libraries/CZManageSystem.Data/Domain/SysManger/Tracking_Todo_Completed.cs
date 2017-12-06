using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.SysManger
{
    public partial class Tracking_Todo_Completed
    {
        public Nullable<System.Guid> WorkflowInstanceId { get; set; }
        public Nullable<System.DateTime> startedtime { get; set; }
        public Nullable<System.DateTime> finishedtime { get; set; }
        public string Actor { get; set; }
        public string SheetId { get; set; }
        public string Title { get; set; }
        public Nullable<byte> Urgency { get; set; }
        public System.Guid WORKFLOWID { get; set; }
        public string WorkflowName { get; set; }
        public string WorkflowAlias { get; set; }
        public string AliasImage { get; set; }
        public string Creator { get; set; }
        public Nullable<int> state { get; set; }
        public string CreatorName { get; set; }
        public string ActivityName { get; set; }
        public string CurrentActivityNames { get; set; }
        public string CurrentActors { get; set; }
        public int TrackingType { get; set; }
        public string ExternalEntityType { get; set; }
        public string ExternalEntityId { get; set; }
     
    }
}
