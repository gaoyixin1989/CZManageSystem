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
		/// Ա��ID
		/// <summary>
		public Nullable<Guid> EditorId { get; set;}
		/// <summary>
		/// Ա������
		/// <summary>
		public string Editor { get; set;}
		/// <summary>
		/// ʱ��
		/// <summary>
		public Nullable<DateTime> EditTime { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public Nullable<DateTime> AtDate { get; set;}
		/// <summary>
		/// �����ϰ�ʱ��
		/// <summary>
		public Nullable<DateTime> AmOnTime { get; set;}
		/// <summary>
		/// �����ϰ�IP
		/// <summary>
		public string AmOnIP { get; set;}
		/// <summary>
		/// �����°�ʱ��
		/// <summary>
		public Nullable<DateTime> AmOffTime { get; set;}
		/// <summary>
		/// �����°�IP
		/// <summary>
		public string AmOffIP { get; set;}
		/// <summary>
		/// �����ϰ�ʱ��
		/// <summary>
		public Nullable<DateTime> PmOnTime { get; set;}
		/// <summary>
		/// �����ϰ�IP
		/// <summary>
		public string PmOnIP { get; set;}
		/// <summary>
		/// �����°�ʱ��
		/// <summary>
		public Nullable<DateTime> PmOffTime { get; set;}
		/// <summary>
		/// �����°�IP
		/// <summary>
		public string PmOffIP { get; set;}
		/// <summary>
		/// ��ע
		/// <summary>
		public string Remark { get; set;}

	}
}
