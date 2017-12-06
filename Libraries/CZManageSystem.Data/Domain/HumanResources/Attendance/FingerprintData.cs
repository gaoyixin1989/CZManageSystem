using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Attendance
{
	public class FingerprintData
	{
		public Guid ID { get; set;}
		public string DevicePhyAddr { get; set;}
		public Nullable<int> RecType { get; set;}
		public Nullable<int> RecStatus { get; set;}
		public string RecTypes { get; set;}
		public string RecStatuss { get; set;}
		public string CardSerno { get; set;}
		public string EmpNumber { get; set;}
		public Nullable<DateTime> Rectime { get; set;}
		public Nullable<DateTime> EnsureTime { get; set;}
		public string Operate { get; set;}
		public Nullable<int> Status { get; set;}
		public Nullable<DateTime> OpDatetime { get; set;}
		public string DoorName { get; set;}
		public string DeviceserNo { get; set;}
		public Nullable<int> DoorId { get; set;}
		public Nullable<int> EmpId { get; set;}
		public Nullable<int> Tid { get; set;}
		public Nullable<int> ActionStatus { get; set;}

	}
}
