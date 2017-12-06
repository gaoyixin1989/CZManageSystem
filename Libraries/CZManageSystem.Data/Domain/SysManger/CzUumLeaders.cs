using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class CzUumLeaders
	{
		public int ID { get; set;}
		public string DepartmentID { get; set;}
		public string EmployeeID { get; set;}
		public string UserName { get; set;}
		public string EmployeeClass { get; set;}
		public string JobType { get; set;}
		public string UserType { get; set;}
		public string CMCCAccount { get; set;}

	}
}
