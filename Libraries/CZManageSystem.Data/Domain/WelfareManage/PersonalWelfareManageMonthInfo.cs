using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class PersonalWelfareManageMonthInfo
	{
		/// <summary>
		/// �¸���ID
		/// <summary>
		public Guid MID { get; set;}
		/// <summary>
		/// Ա�����
		/// <summary>
		public string Employee { get; set;}
		/// <summary>
		/// Ա������
		/// <summary>
		public string EmployeeName { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public string CYearAndMonth { get; set;}
		/// <summary>
		/// �����ײ�
		/// <summary>
		public string WelfarePackage { get; set;}
		/// <summary>
		/// �ܶ��
		/// <summary>
		public Nullable<decimal> WelfareMonthTotalAmount { get; set;}
		/// <summary>
		/// ���ö��
		/// <summary>
		public Nullable<decimal> WelfareMonthAmountUsed { get; set;}
		/// <summary>
		/// ����ʱ��
		/// <summary>
		public Nullable<DateTime> CreateTime { get; set;}
		/// <summary>
		/// �༭��
		/// <summary>
		public Nullable<int> Editor { get; set;}
		/// <summary>
		/// �༭ʱ��
		/// <summary>
		public Nullable<DateTime> EditTime { get; set;}

	}
}
