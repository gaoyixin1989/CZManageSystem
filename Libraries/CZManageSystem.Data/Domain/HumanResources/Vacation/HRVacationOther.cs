using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// �����ݼ�
/// </summary>
namespace CZManageSystem.Data.Domain.HumanResources.Vacation
{
	public class HRVacationOther
	{
		public Guid ID { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public string OtherName { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public Nullable<decimal> PeriodTime { get; set;}
		/// <summary>
		/// ��ʼʱ��
		/// <summary>
		public Nullable<DateTime> StartTime { get; set;}
		/// <summary>
		/// ����ʱ��
		/// <summary>
		public Nullable<DateTime> EndTime { get; set;}
		/// <summary>
		/// ��ע
		/// <summary>
		public string Remark { get; set;}
		/// <summary>
		/// ���ڱ��ID
		/// <summary>
		public Nullable<Guid> VacationID { get; set;}
		/// <summary>
		/// ͬ��ı�ʶ
		/// <summary>
		public Nullable<int> AgreeFlag { get; set;}
		/// <summary>
		/// ����ID
		/// <summary>
		public Nullable<Guid> ReVacationID { get; set;}

	}
}
