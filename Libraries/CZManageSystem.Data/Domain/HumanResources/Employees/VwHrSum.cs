using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Employees
{
	public class VWHRSum
	{
		public Nullable<double> billcyc { get; set;}
		public Nullable<decimal> 总收入 { get; set;}
		public Nullable<decimal> 实发 { get; set;}
		public Nullable<decimal> 应扣除 { get; set;}
		public string  employerid { get; set;}
		public string name { get; set;}
		public string  sjhm { get; set;}
		public string acctno { get; set;}
		public Nullable<DateTime> updatetime { get; set;}

	}
}
