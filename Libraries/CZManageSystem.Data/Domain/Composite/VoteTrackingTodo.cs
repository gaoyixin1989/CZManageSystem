using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Composite
{
	public class VoteTrackingTodo
	{
		public Guid ActivityInstanceId { get; set;}
		public string UserName { get; set;}
		public Nullable<int> State { get; set;}
		public string ProxyName { get; set;}
		public Nullable<int> OperateType { get; set;}
		public string ActorName { get; set;}
		public Nullable<bool> IsCompleted { get; set;}
		public Nullable<DateTime> CreatedTime { get; set;}
        public Nullable<DateTime> StartedTime { get; set; }
        public Nullable<DateTime> FinishedTime { get; set;}
		public string Title { get; set;}
		public string CreatorName { get; set;}
		public string SheetId { get; set;}
		public string Creator { get; set;}
        public string ExternalEntityId { get; set; }
        public string ExternalEntityType { get; set; }

    }
}
