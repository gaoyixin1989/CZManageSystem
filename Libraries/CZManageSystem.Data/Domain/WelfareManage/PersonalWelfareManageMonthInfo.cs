using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class PersonalWelfareManageMonthInfo
	{
		/// <summary>
		/// 月福利ID
		/// <summary>
		public Guid MID { get; set;}
		/// <summary>
		/// 员工编号
		/// <summary>
		public string Employee { get; set;}
		/// <summary>
		/// 员工名字
		/// <summary>
		public string EmployeeName { get; set;}
		/// <summary>
		/// 年月
		/// <summary>
		public string CYearAndMonth { get; set;}
		/// <summary>
		/// 福利套餐
		/// <summary>
		public string WelfarePackage { get; set;}
		/// <summary>
		/// 总额度
		/// <summary>
		public Nullable<decimal> WelfareMonthTotalAmount { get; set;}
		/// <summary>
		/// 已用额度
		/// <summary>
		public Nullable<decimal> WelfareMonthAmountUsed { get; set;}
		/// <summary>
		/// 创建时间
		/// <summary>
		public Nullable<DateTime> CreateTime { get; set;}
		/// <summary>
		/// 编辑人
		/// <summary>
		public Nullable<int> Editor { get; set;}
		/// <summary>
		/// 编辑时间
		/// <summary>
		public Nullable<DateTime> EditTime { get; set;}

	}
}
