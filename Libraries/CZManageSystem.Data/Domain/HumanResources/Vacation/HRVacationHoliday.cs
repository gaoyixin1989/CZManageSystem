using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Data.Domain.HumanResources.Vacation
{
	public class HRVacationHoliday
	{
		public Guid ID { get; set;}
		/// <summary>
		/// 用户ID
		/// <summary>
		public Nullable<Guid> UserId { get; set;}
		/// <summary>
		/// 年度
		/// <summary>
		public Nullable<int> YearDate { get; set;}
		/// <summary>
		/// 休假类型
		/// <summary>
		public string VacationType { get; set;}
		/// <summary>
		/// 公假类型
		/// <summary>
		public string VacationClass { get; set;}
		/// <summary>
		/// 天数
		/// <summary>
		public Nullable<decimal> PeriodTime { get; set;}
		/// <summary>
		/// 开始时间
		/// <summary>
		public Nullable<DateTime> StartTime { get; set;}
		/// <summary>
		/// 结束时间
		/// <summary>
		public Nullable<DateTime> EndTime { get; set;}
		/// <summary>
		/// 原因
		/// <summary>
		public string Reason { get; set;}

        public virtual Users UserObj { get; set; }
        //public virtual Depts DeptObj { get; set; }

    }
}
