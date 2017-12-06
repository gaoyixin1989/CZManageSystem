using CZManageSystem.Core;
using CZManageSystem.Data.Domain.Composite;
using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.SysManger
{
    public partial class Tracking_Todo
    { 
        public Guid ActivityInstanceId { get; set; }
        public string UserName { get; set; }
        public int State { get; set; }
        public string ProxyName { get; set; }
        public int OperateType { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedTime { get; set; }
        public Nullable<DateTime> FinishedTime { get; set; }
        public string Actor { get; set; }
        public string ActivityName { get; set; }
        public string Title { get; set; }
        public string WorkflowAlias { get; set; }
        public string WorkflowName { get; set; }
        public Guid WorkflowInstanceId { get; set; }
        public string SheetId { get; set; }
        public DateTime StartedTime { get; set; }
        public byte Urgency { get; set; }
        public byte Importance { get; set; }
        public string Creator { get; set; }
        public string CreatorName { get; set; }
        public string AliasImage { get; set; }
        public string TodoActors { get; set; }
        public string ActorName { get; set; }
        public Guid ActivityId { get; set; }
        public string ExternalEntityType { get; set; }
        public string ExternalEntityId { get; set; }
        public int Secrecy { get; set; }
        public int TrackingType { get; set; }

    }
}
