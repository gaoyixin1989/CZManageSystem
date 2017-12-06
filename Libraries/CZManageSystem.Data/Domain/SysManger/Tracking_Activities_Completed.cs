using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class Tracking_Activities_Completed
	{
		public Guid ActivityInstanceId { get; set;}
		public Nullable<Guid> PrevSetId { get; set;}
		public Guid WorkflowInstanceId { get; set;}
		public Nullable<Guid> ActivityId { get; set;}
		public bool IsCompleted { get; set;}
		public Nullable<int> OperateType { get; set;}
		public string Actor { get; set;}
		public Nullable<DateTime> CreatedTime { get; set;}
		public Nullable<DateTime> FinishedTime { get; set;}
		public string Command { get; set;}
		public string Reason { get; set;}
		public string ExternalEntityType { get; set;}
		public string ExternalEntityId { get; set;}
		public string ActorDescription { get; set;}
		public Nullable<int> PrintCount { get; set;}
        
        public virtual Activities Activities { get; set; }
    }
}
