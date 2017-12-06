using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Attendance
{
	public class HRCheckAttendanceHistory
	{
		public Guid AttendanceHistoryId { get; set;}
		/// <summary>
		/// 用户ID
		/// <summary>
		public Nullable<Guid> UserId { get; set;}
		/// <summary>
		/// 日期
		/// <summary>
		public Nullable<DateTime> AtDate { get; set;}
        public Nullable<DateTime> OffDate { get; set; }
        /// <summary>
        /// 上班时间
        /// <summary>
        public Nullable<DateTime> DoTime { get; set;}
		public Nullable<DateTime> OffTime { get; set;}
		public Nullable<DateTime> DoReallyTime { get; set;}
		public Nullable<DateTime> OffReallyTime { get; set;}
        public Nullable<DateTime> DoReallyDate { get; set; }
        public Nullable<DateTime> OffReallyDate { get; set; }
        /// <summary>
        /// 上班登记IP
        /// <summary>
        public string IpOn { get; set;}
		/// <summary>
		/// 下班登记IP
		/// <summary>
		public string IpOff { get; set;}
		/// <summary>
		/// 分钟
		/// <summary>
		public Nullable<int> Minute { get; set;}
		/// <summary>
		/// 1、已申报；2、休假；3、外出
		/// <summary>
		public Nullable<int> DoFlag { get; set;}
		/// <summary>
		/// 轮休标志，1是为轮休状态
		/// <summary>
		public Nullable<int> RotateDaysOffFlag { get; set;}
		/// <summary>
		/// 上班：1、指纹登记；2、手机登记；3、通宝卡登记；4、门禁卡登记
		/// <summary>
		public Nullable<int> FlagOn { get; set;}
		/// <summary>
		/// 下班：1、指纹登记；2、手机登记；3、通宝卡登记；4、门禁卡登记
		/// <summary>
		public Nullable<int> FlagOff { get; set;}

	}
}
