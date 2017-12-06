using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Vacation
{
	public class HRVacationAnnualLeaveConfig
	{
		public Guid ID { get; set;}
		/// <summary>
		/// 年度
		/// <summary>
		public Nullable<int> Annual { get; set;}
		/// <summary>
		/// 假期天数
		/// <summary>
		public Nullable<decimal> SpanTime { get; set;}
		/// <summary>
		/// 本年份最晚使用月份
		/// <summary>
		public string LimitMonth { get; set;}

	}

    public class HRVacationAnnualLeaveConfigQueryBuilder
    {
        
        public Nullable<int> Annual { get; set; }

    }
}
