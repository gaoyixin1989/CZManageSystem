using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Attendance
{
	public class HRUnattendLink
	{
		public Guid ID { get; set;}
		public Nullable<Guid> ApplyRecordId { get; set;}
		public Nullable<Guid> AttendanceId { get; set;}

	}
}
