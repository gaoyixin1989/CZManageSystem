using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Attendance
{
	public class HRYKTData
	{
		public Guid ID { get; set;}
		public Nullable<int> Tid { get; set;}
		public Nullable<long> CardID { get; set;}
		public Nullable<long> BusinessCardID { get; set;}
		public string loginId { get; set;}
		public string Employee { get; set;}
		public string Name { get; set;}
		public string DepartmentName { get; set;}
		public Nullable<DateTime> ReaderTime { get; set;}
		public string DoorName { get; set;}
		public string Path { get; set;}
		public Nullable<int> ActionStatus { get; set;}

	}
}
