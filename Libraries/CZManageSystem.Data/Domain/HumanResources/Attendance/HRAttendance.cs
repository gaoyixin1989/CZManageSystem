using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Attendance
{
	public class HRAttendance
	{
		public Guid ID { get; set;}
		/// <summary>
		/// 员工ID
		/// <summary>
		public Nullable<Guid> EditorId { get; set;}
		/// <summary>
		/// 员工姓名
		/// <summary>
		public string Editor { get; set;}
		/// <summary>
		/// 时间
		/// <summary>
		public Nullable<DateTime> EditTime { get; set;}
		/// <summary>
		/// 日期
		/// <summary>
		public Nullable<DateTime> AtDate { get; set;}
		/// <summary>
		/// 上午上班时间
		/// <summary>
		public Nullable<DateTime> AmOnTime { get; set;}
		/// <summary>
		/// 上午上班IP
		/// <summary>
		public string AmOnIP { get; set;}
		/// <summary>
		/// 上午下班时间
		/// <summary>
		public Nullable<DateTime> AmOffTime { get; set;}
		/// <summary>
		/// 上午下班IP
		/// <summary>
		public string AmOffIP { get; set;}
		/// <summary>
		/// 下午上班时间
		/// <summary>
		public Nullable<DateTime> PmOnTime { get; set;}
		/// <summary>
		/// 下午上班IP
		/// <summary>
		public string PmOnIP { get; set;}
		/// <summary>
		/// 下午下班时间
		/// <summary>
		public Nullable<DateTime> PmOffTime { get; set;}
		/// <summary>
		/// 下午下班IP
		/// <summary>
		public string PmOffIP { get; set;}
		/// <summary>
		/// 备注
		/// <summary>
		public string Remark { get; set;}

	}
}
