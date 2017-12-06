using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Employees
{
	public class GdPay
	{
		public string employerid { get; set;}
		public int billcyc { get; set;}
		public int payid { get; set;}
		public string je { get; set;}
		public Nullable<DateTime> updatetime { get; set;}
		public string value_str { get; set;}

	}
}
