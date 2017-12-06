using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Employees
{
	public class GdPayV
	{
		public string 部门 { get; set;}
		public Nullable<Int16> deptid { get; set;}
		public string 姓名 { get; set;}
		public string  员工编号 { get; set;}
		public string 账务周期 { get; set;}
		public int 收入编号 { get; set;}
		public string 收入 { get; set;}
		public string 固定收入项目 { get; set;}
		public Nullable<DateTime> 更新时间 { get; set;}
		public int billcyc { get; set;}
		public int pid { get; set;}
		public string 所属类型 { get; set;}
		public string value_str { get; set;}
		public string DataType { get; set;}

	}
}
