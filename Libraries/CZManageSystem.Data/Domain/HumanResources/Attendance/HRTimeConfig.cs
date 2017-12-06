using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Attendance
{
	public class HRTimeConfig
	{
		public Guid ID { get; set;}
		/// <summary>
		/// 时间数量
		/// <summary>
		public Nullable<decimal> SpanTime { get; set;}
		/// <summary>
		/// 上班可提前打卡时间
		/// <summary>
		public Nullable<int> LeadTime { get; set;}
		/// <summary>
		/// 指纹考勤下班可推迟时间
		/// <summary>
		public Nullable<int> LatestTime { get; set;}

	}
}
