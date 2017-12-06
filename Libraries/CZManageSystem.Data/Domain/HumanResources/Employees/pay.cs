using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Employees
{
	public class Pay
	{
		public string  employerid { get; set;}
		public int billcyc { get; set;}
		public Nullable<decimal> 固定收入 { get; set;}
		public Nullable<decimal> 工龄工资 { get; set;}
		public Nullable<decimal> 月度考核奖 { get; set;}
		public Nullable<decimal> 话费补助 { get; set;}
		public Nullable<decimal> 交通补贴 { get; set;}
		public Nullable<decimal> 值夜夜班津贴 { get; set;}
		public Nullable<decimal> 节假日加班工资 { get; set;}
		public Nullable<decimal> 其它 { get; set;}
		public Nullable<decimal> 机动奖合计 { get; set;}
		public Nullable<decimal> 总收入 { get; set;}
		public Nullable<decimal> 社保扣款 { get; set;}
		public Nullable<decimal> 医保扣款 { get; set;}
		public Nullable<decimal> 住房公积金 { get; set;}
		public Nullable<decimal> 宿舍房租及水电费 { get; set;}
		public Nullable<decimal> 其它扣款 { get; set;}
		public Nullable<decimal> 社保企 { get; set;}
		public Nullable<decimal> 医保企 { get; set;}
		public Nullable<decimal> 住房公积金企 { get; set;}
		public Nullable<decimal> 应纳税所得额 { get; set;}
		public Nullable<decimal> 个人所得税 { get; set;}
		public Nullable<decimal> 实发 { get; set;}
		public string 备注 { get; set;}
		public Nullable<DateTime> 更新时间 { get; set;}

	}
}
