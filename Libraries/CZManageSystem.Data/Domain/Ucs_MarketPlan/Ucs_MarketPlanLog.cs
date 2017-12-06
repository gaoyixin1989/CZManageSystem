using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.MarketPlan
{
	public class Ucs_MarketPlanLog
    {
		public Guid Id { get; set;}
		/// <summary>
		/// 营销方案编码
		/// <summary>
		public string Coding { get; set;}
		/// <summary>
		/// 营销方案名称
		/// <summary>
		public string Name { get; set;}
		/// <summary>
		/// 部门
		/// <summary>
		public string Department { get; set;}
		public string Creator { get; set;}
		public Nullable<System.DateTime> Creattime { get; set;}
		public string Remark { get; set;}

	}

    public class MarketPlanLogQueryBuilder
    {
        public string Name { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
    }
}
