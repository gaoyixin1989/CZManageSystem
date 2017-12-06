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
		/// �༭��ID
		/// <summary>
		public Nullable<Guid> EditorId { get; set;}
		/// <summary>
		/// �༭��
		/// <summary>
		public string Editor { get; set;}
		/// <summary>
		/// �༭ʱ��
		/// <summary>
		public Nullable<DateTime> EditTime { get; set;}
		/// <summary>
		/// �����ϰ�ʱ��
		/// <summary>
		public Nullable<DateTime> AMOnDuty { get; set;}
		/// <summary>
		/// �����°�ʱ��
		/// <summary>
		public Nullable<DateTime> AMOffDuty { get; set;}
		/// <summary>
		/// �����ϰ�ʱ��
		/// <summary>
		public Nullable<DateTime> PMOnDuty { get; set;}
		/// <summary>
		/// �����°�ʱ��
		/// <summary>
		public Nullable<DateTime> PMOffDuty { get; set;}
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
		public Nullable<DateTime> LimitTime { get; set;}
		/// <summary>
		/// �û�id
		/// <summary>
		public Nullable<Guid> UserId { get; set;}

	}
}
