using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Attendance
{
	public class HRTimeConfig
	{
		public Guid ID { get; set;}
		/// <summary>
		/// ʱ������
		/// <summary>
		public Nullable<decimal> SpanTime { get; set;}
		/// <summary>
		/// �ϰ����ǰ��ʱ��
		/// <summary>
		public Nullable<int> LeadTime { get; set;}
		/// <summary>
		/// ָ�ƿ����°���Ƴ�ʱ��
		/// <summary>
		public Nullable<int> LatestTime { get; set;}

	}
}
