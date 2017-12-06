using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Attendance
{
	public class HRAttendanceConfig
	{
		public Guid ID { get; set;}
		/// <summary>
		/// �༭��ID
		/// <summary>
		public Nullable<Guid> EditorId { get; set;}
		/// <summary>
		/// �༭��
		/// <summary>
		public string  Editor { get; set;}
		/// <summary>
		/// �༭ʱ��
		/// <summary>
		public Nullable<DateTime> EditTime { get; set;}
		/// <summary>
		/// �����ϰ�ʱ��
		/// <summary>
		public Nullable<TimeSpan>AMOnDuty { get; set;}
		/// <summary>
		/// �����°�ʱ��
		/// <summary>
		public Nullable<TimeSpan> AMOffDuty { get; set;}
		/// <summary>
		/// �����ϰ�ʱ��
		/// <summary>
		public Nullable<TimeSpan> PMOnDuty { get; set;}
		/// <summary>
		/// �����°�ʱ��
		/// <summary>
		public Nullable<TimeSpan> PMOffDuty { get; set;}
		/// <summary>
		/// ʱ����
		/// <summary>
		public Nullable<decimal> SpanTime { get; set;}
		/// <summary>
		/// ��ע
		/// <summary>
		public string Remark { get; set;}
		/// <summary>
		/// �ϰ����ǰ��ʱ��
		/// <summary>
		public Nullable<int> LeadTime { get; set;}
        public Nullable<int> LatestTime { get; set; }
        
        /// <summary>
        /// ����id
        /// <summary>
        public string DeptIds { get; set;}

	}
}
