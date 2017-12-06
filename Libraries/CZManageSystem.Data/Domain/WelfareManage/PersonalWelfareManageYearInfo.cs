using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class PersonalWelfareManageYearInfo
	{
		/// <summary>
		/// 年福利ID
		/// <summary>
		public Guid YID { get; set;}
		/// <summary>
		/// 员工编号
		/// <summary>
		public string Employee { get; set;}
		/// <summary>
		/// 员工姓名
		/// <summary>
		public string EmployeeName { get; set;}
		/// <summary>
		/// 年
		/// <summary>
		public string CYear { get; set;}
		/// <summary>
		/// 年福利总额
		/// <summary>
		public decimal WelfareYearTotalAmount { get; set;}
		/// <summary>
		/// 创建时间
		/// <summary>
		public DateTime CreateTime { get; set;}
		/// <summary>
		/// 编辑者
		/// <summary>
		public int Editor { get; set;}
		/// <summary>
		/// 编辑时间
		/// <summary>
		public DateTime EditTime { get; set;}

	}
}
