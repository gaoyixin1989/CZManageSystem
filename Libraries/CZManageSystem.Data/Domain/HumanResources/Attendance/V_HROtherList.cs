using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Attendance
{
	public class V_HROtherList
	{
		public Nullable<Guid> UserId { get; set;}
		public string RealName { get; set;}
		public string EmployeeId { get; set;}
		public string DpId { get; set;}
		public string DpName { get; set;}
		public string DpFullName { get; set;}
		public Nullable<DateTime> AtDate { get; set;}

	}
}
