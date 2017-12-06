using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.ITSupport
{
	public class V_Consumable_CancellingDetail
	{
		public Guid ID { get; set;}
		public Nullable<int> CancelNumber { get; set;}
		public Nullable<Guid> WorkflowInstanceId { get; set;}
		public string Series { get; set;}
		public Nullable<DateTime> ApplyTime { get; set;}
		public string ApplyDpFullName { get; set;}
		public string AppDept { get; set;}
		public string ApplicantName { get; set;}
		public string AppPerson { get; set;}
		public string Mobile { get; set;}
		public string Title { get; set;}
		public string Content { get; set;}
		public string Type { get; set;}
		public string Name { get; set;}
		public string Model { get; set;}
		public Nullable<int> State { get; set;}

	}
}
