using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Attendance
{
	public class V_HRHaveAHolidayList
	{
		public Nullable<Guid> UserId { get; set;}
		public Nullable<DateTime> StartTime { get; set;}
		public Nullable<DateTime> EndTime { get; set;}
		public string EmployeeId { get; set;}
		public string RealName { get; set;}
		public string DpId { get; set;}
		public string DpName { get; set;}
		public string DpFullName { get; set;}

	}
}
