using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class CzUumPosts
	{
		public int ID { get; set;}
		public string UserName { get; set;}
		public string OUID { get; set;}
		public string EmployeeClass { get; set;}
		public string EmployeeLevel { get; set;}
		public Nullable<bool> IsSync { get; set;}

	}
}
