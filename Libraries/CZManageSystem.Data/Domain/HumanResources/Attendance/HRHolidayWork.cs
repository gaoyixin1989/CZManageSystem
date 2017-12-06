using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Attendance
{
	public class HRHolidayWork
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
		/// 开始时间
		/// <summary>
		public Nullable<DateTime> StartTime { get; set;}
		/// <summary>
		/// 结束时间
		/// <summary>
		public Nullable<DateTime> EndTime { get; set;}

	}
}
