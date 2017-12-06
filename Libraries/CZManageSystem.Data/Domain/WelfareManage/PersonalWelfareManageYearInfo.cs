using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class PersonalWelfareManageYearInfo
	{
		/// <summary>
		/// �긣��ID
		/// <summary>
		public Guid YID { get; set;}
		/// <summary>
		/// Ա�����
		/// <summary>
		public string Employee { get; set;}
		/// <summary>
		/// Ա������
		/// <summary>
		public string EmployeeName { get; set;}
		/// <summary>
		/// ��
		/// <summary>
		public string CYear { get; set;}
		/// <summary>
		/// �긣���ܶ�
		/// <summary>
		public decimal WelfareYearTotalAmount { get; set;}
		/// <summary>
		/// ����ʱ��
		/// <summary>
		public DateTime CreateTime { get; set;}
		/// <summary>
		/// �༭��
		/// <summary>
		public int Editor { get; set;}
		/// <summary>
		/// �༭ʱ��
		/// <summary>
		public DateTime EditTime { get; set;}

	}
}
