using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Attendance
{
	public class HRTBcard
	{
		/// <summary>
		/// ����
		/// <summary>
		public Guid ID { get; set;}
        public Nullable<int> Tid { get; set; }
		/// <summary>
		/// �˺�
		/// <summary>
		public string EmployeeId { get; set;}
		/// <summary>
		/// ʱ��
		/// <summary>
		public Nullable<DateTime> SkTime { get; set;}
		/// <summary>
		/// ״̬
		/// <summary>
		public Nullable<int> ActionStatus { get; set;}
		/// <summary>
		/// �û�ID
		/// <summary>
		public string EmpNo { get; set;}
		/// <summary>
		/// ͨ������
		/// <summary>
		public string CardNo { get; set;}

	}
}
