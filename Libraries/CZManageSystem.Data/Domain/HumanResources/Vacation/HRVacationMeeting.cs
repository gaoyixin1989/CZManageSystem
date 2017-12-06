using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 假期会议
/// </summary>
namespace CZManageSystem.Data.Domain.HumanResources.Vacation
{
	public class HRVacationMeeting
	{
		public Guid ID { get; set;}
		/// <summary>
		/// 会议名称
		/// <summary>
		public string MeetingName { get; set;}
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
		/// 备注
		/// <summary>
		public string Remark { get; set;}
		/// <summary>
		/// 假期表的ID
		/// <summary>
		public Nullable<Guid> VacationID { get; set;}
		/// <summary>
		/// 积分
		/// <summary>
		public Nullable<decimal> Integral { get; set;}
		/// <summary>
		/// 同意的标识
		/// <summary>
		public Nullable<int> AgreeFlag { get; set;}
		/// <summary>
		/// 销假ID
		/// <summary>
		public Nullable<Guid> ReVacationID { get; set;}

	}
}
