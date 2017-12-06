using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.CollaborationCenter.Payment
{
	public class PaymentPayMoney
	{
		/// <summary>
		/// ���
		/// <summary>
		public Guid ID { get; set;}
		/// <summary>
		/// ����ID
		/// <summary>
		public Nullable<Guid> ApplyID { get; set;}
		/// <summary>
		/// �������ʺ�
		/// <summary>
		public string PayerAccount { get; set;}
		/// <summary>
		/// ����������
		/// <summary>
		public string PayerName { get; set;}
		/// <summary>
		/// �����˷��д���
		/// <summary>
		public string PayerBranchCode { get; set;}
		/// <summary>
		/// �տ����ʺ�
		/// <summary>
		public string PayeeAccount { get; set;}
		/// <summary>
		/// �տ����ʺ�����
		/// <summary>
		public string PayeeName { get; set;}
		/// <summary>
		/// �տ�����������
		/// <summary>
		public string PayeeBranch { get; set;}
		/// <summary>
		/// �տ��˷��д���
		/// <summary>
		public string PayeeBranchCode { get; set;}
		/// <summary>
		/// �տ��˿�����
		/// <summary>
		public string PayeeOpenBank { get; set;}
		/// <summary>
		/// �տ�����������
		/// <summary>
		public string PayeeBank { get; set;}
		/// <summary>
		/// �տ������д���
		/// <summary>
		public string PayeeBankCode { get; set;}
		/// <summary>
		/// �տ������ش���
		/// <summary>
		public string PayeeAddressCode { get; set;}
		/// <summary>
		/// ���
		/// <summary>
		public Nullable<decimal> Money { get; set;}
		/// <summary>
		/// �ұ�
		/// <summary>
		public string MoneyType { get; set;}
		/// <summary>
		/// Ŀ��
		/// <summary>
		public string Purpose { get; set;}
		/// <summary>
		/// �տ����������
		/// <summary>
		public string PayeeAreaCode { get; set;}

	}
}
