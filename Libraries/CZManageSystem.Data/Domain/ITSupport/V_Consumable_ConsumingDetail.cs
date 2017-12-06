using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.ITSupport
{
	public class V_Consumable_ConsumingDetail
	{
		public Nullable<Guid> ID { get; set;}
		public Nullable<int> AllotNumber { get; set;}
		public Nullable<int> ClaimsNumber { get; set;}
		public Nullable<Guid> WorkflowInstanceId { get; set;}
		public string Series { get; set;}
		public Nullable<DateTime> ApplyTime { get; set;}
		public string ApplyDpFullName { get; set;}
		public string ApplyDpCode { get; set;}
		public string ApplicantName { get; set;}
		public string Applicant { get; set;}
		public string Mobile { get; set;}
		public string Title { get; set;}
		public string Content { get; set;}
		public string Type { get; set;}
		public string Name { get; set;}
		public string Model { get; set;}
		public Nullable<int> State { get; set;}

	}
}
