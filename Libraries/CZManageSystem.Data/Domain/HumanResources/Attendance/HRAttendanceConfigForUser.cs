using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Attendance
{
	public class HRAttendanceConfigForUser
	{
		public Guid ID { get; set;}
		/// <summary>
		/// 编辑者ID
		/// <summary>
		public Nullable<Guid> EditorId { get; set;}
		/// <summary>
		/// 编辑者
		/// <summary>
		public string Editor { get; set;}
		/// <summary>
		/// 编辑时间
		/// <summary>
		public Nullable<DateTime> EditTime { get; set;}
		/// <summary>
		/// 上午上班时间
		/// <summary>
		public Nullable<DateTime> AMOnDuty { get; set;}
		/// <summary>
		/// 上午下班时间
		/// <summary>
		public Nullable<DateTime> AMOffDuty { get; set;}
		/// <summary>
		/// 下午上班时间
		/// <summary>
		public Nullable<DateTime> PMOnDuty { get; set;}
		/// <summary>
		/// 下午下班时间
		/// <summary>
		public Nullable<DateTime> PMOffDuty { get; set;}
		/// <summary>
		/// 时间跨度
		/// <summary>
		public Nullable<decimal> SpanTime { get; set;}
		/// <summary>
		/// 备注
		/// <summary>
		public string Remark { get; set;}
		/// <summary>
		/// 上班可提前打卡时间
		/// <summary>
		public Nullable<DateTime> LimitTime { get; set;}
		/// <summary>
		/// 用户id
		/// <summary>
		public Nullable<Guid> UserId { get; set;}

	}
}
