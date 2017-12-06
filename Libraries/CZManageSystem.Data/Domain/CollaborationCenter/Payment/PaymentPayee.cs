using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.CollaborationCenter.Payment
{
	public class PaymentPayee
	{
		/// <summary>
		/// �տ����ʺ�ID
		/// <summary>
		public Guid PayeeID { get; set;}
		/// <summary>
		/// �տ����ʺ�
		/// <summary>
		public string Account { get; set;}
		/// <summary>
		/// �տ�������
		/// <summary>
		public string Name { get; set;}
		/// <summary>
		/// �տ����������д���
		/// <summary>
		public string BranchCode { get; set;}
		/// <summary>
		/// ������
		/// <summary>
		public string Branch { get; set;}
		/// <summary>
		/// ����������
		/// <summary>
		public string OpenBank { get; set;}
		/// <summary>
		/// ������������
		/// <summary>
		public string Bank { get; set;}
		/// <summary>
		/// �������д���
		/// <summary>
		public string BankCode { get; set;}
		/// <summary>
		/// ���ش���
		/// <summary>
		public string AddressCode { get; set;}
		/// <summary>
		/// �������
		/// <summary>
		public string AreaCode { get; set;}
		/// <summary>
		/// ��Ӫ��ID
		/// <summary>
		public Nullable<Guid> HallID { get; set;}

	}
}
