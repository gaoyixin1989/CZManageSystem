using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Data.Domain.HumanResources.Attendance
{
	public class HRCheckAttendanceHistoryNo1
	{
		public Guid AttendanceHistoryNOId { get; set;}
		public Nullable<Guid> HistoryId { get; set;}
		/// <summary>
		/// �û�ID
		/// <summary>
		public Nullable<Guid> UserId { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public Nullable<DateTime> AtDate { get; set;}
        public Nullable<DateTime> OffDate { get; set; }
        /// <summary>
        /// �ϰ�ʱ��
        /// <summary>
        public Nullable<TimeSpan> DoTime { get; set;}
		public Nullable<TimeSpan> OffTime { get; set;}
		public Nullable<TimeSpan> DoReallyTime { get; set;}
		public Nullable<TimeSpan> OffReallyTime { get; set; }
        public Nullable<DateTime> DoReallyDate { get; set; }
        public Nullable<DateTime> OffReallyDate { get; set; }
        /// <summary>
        /// �ϰ�Ǽ�IP
        /// <summary>
        public string IpOn { get; set;}
		/// <summary>
		/// �°�Ǽ�IP
		/// <summary>
		public string IpOff { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public Nullable<int> Minute { get; set;}
		/// <summary>
		/// 1�����걨��2���ݼ٣�3�����
		/// <summary>
		public Nullable<int> DoFlag { get; set;}
		/// <summary>
		/// ���ݱ�־��1��Ϊ����״̬
		/// <summary>
		public Nullable<int> RotateDaysOffFlag { get; set;}
		/// <summary>
		/// �ϰࣺ1��ָ�ƵǼǣ�2���ֻ��Ǽǣ�3��ͨ�����Ǽǣ�4���Ž����Ǽ�
		/// <summary>
		public Nullable<int> FlagOn { get; set;}
		/// <summary>
		/// �°ࣺ1��ָ�ƵǼǣ�2���ֻ��Ǽǣ�3��ͨ�����Ǽǣ�4���Ž����Ǽ�
		/// <summary>
		public Nullable<int> FlagOff { get; set;}
		public Nullable<int> TypeRecord { get; set; }
        public virtual Users Users { get; set; }

    }
}
