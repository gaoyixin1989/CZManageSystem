using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// ��ʷ��Ŀ�ݹ�������ϸ��
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.Invest
{
	public class InvestAgoEstimateApplyDetail
	{
		/// <summary>
		/// ���
		/// <summary>
		public Guid ID { get; set;}
		/// <summary>
		/// �������뵥id
		/// <summary>
		public Nullable<Guid> ApplyID { get; set;}
		/// <summary>
		/// ���
		/// <summary>
		public Nullable<int> Year { get; set;}
		/// <summary>
		/// �·�
		/// <summary>
		public Nullable<int> Month { get; set;}
		/// <summary>
		/// ��Ŀ����
		/// <summary>
		public string ProjectName { get; set;}
		/// <summary>
		/// ��Ŀ���
		/// <summary>
		public string ProjectID { get; set;}
		/// <summary>
		/// ��ͬ����
		/// <summary>
		public string ContractName { get; set;}
		/// <summary>
		/// ��ͬ���
		/// <summary>
		public string ContractID { get; set;}
		/// <summary>
		/// ��Ӧ��
		/// <summary>
		public string Supply { get; set;}
		/// <summary>
		/// ��ͬ�ܽ��
		/// <summary>
		public Nullable<decimal> SignTotal { get; set;}
		/// <summary>
		/// ��ͬʵ�ʽ��
		/// <summary>
		public Nullable<decimal> PayTotal { get; set;}
		/// <summary>
		/// ����רҵ
		/// <summary>
		public string Study { get; set;}
		/// <summary>
		/// ������ID
		/// <summary>
		public Nullable<Guid> ManagerID { get; set;}
		/// <summary>
		/// ��Ŀ
		/// <summary>
		public string Course { get; set;}
		/// <summary>
		/// �ϸ��½���
		/// <summary>
		public Nullable<decimal> BackRate { get; set;}
		/// <summary>
		/// ��Ŀ�������
		/// <summary>
		public Nullable<decimal> Rate { get; set;}
		/// <summary>
		/// �Ѹ����
		/// <summary>
		public Nullable<decimal> Pay { get; set;}
		/// <summary>
		/// �ݹ����
		/// <summary>
		public Nullable<decimal> NotPay { get; set;}
		/// <summary>
		/// �ݹ���ԱID
		/// <summary>
		public Nullable<Guid> EstimateUserID { get; set;}

	}
}
