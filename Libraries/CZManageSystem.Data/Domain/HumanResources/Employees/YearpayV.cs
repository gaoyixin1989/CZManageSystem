using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Employees
{
	public class YearPayV
	{
		public string deptname { get; set;}
		public string 姓名 { get; set;}
		public string  sjhm { get; set;}
		public string  pos { get; set;}
		public Nullable<short> postr { get; set;}
		public string  employerid { get; set;}
		public Nullable<int> billcyc { get; set;}
		public Nullable<decimal> 年终考核奖应发 { get; set;}
		public Nullable<decimal> 年终双薪奖应发 { get; set;}
		public Nullable<decimal> 年终考核奖实发 { get; set;}
		public Nullable<decimal> 年终双薪奖实发 { get; set;}
		public Nullable<decimal> 应发合计 { get; set;}
		public Nullable<decimal> 应扣个所 { get; set;}
		public Nullable<decimal> 实发奖金合计 { get; set;}
		public string bz { get; set;}

	}
}
