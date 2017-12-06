using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Attendance
{
	public class HRTBcard
	{
		/// <summary>
		/// 主键
		/// <summary>
		public Guid ID { get; set;}
        public Nullable<int> Tid { get; set; }
		/// <summary>
		/// 账号
		/// <summary>
		public string EmployeeId { get; set;}
		/// <summary>
		/// 时间
		/// <summary>
		public Nullable<DateTime> SkTime { get; set;}
		/// <summary>
		/// 状态
		/// <summary>
		public Nullable<int> ActionStatus { get; set;}
		/// <summary>
		/// 用户ID
		/// <summary>
		public string EmpNo { get; set;}
		/// <summary>
		/// 通宝卡号
		/// <summary>
		public string CardNo { get; set;}

	}
}
