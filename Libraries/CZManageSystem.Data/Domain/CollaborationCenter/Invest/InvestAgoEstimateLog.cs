using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// ��ʷ��Ŀ�ݹ��޸���־
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.Invest
{
	public class InvestAgoEstimateLog
	{
		/// <summary>
		/// ���
		/// <summary>
		public int ID { get; set;}
		/// <summary>
		/// ��Ŀid
		/// <summary>
		public string ProjectID { get; set;}
		/// <summary>
		/// ��ͬid
		/// <summary>
		public string ContractID { get; set;}
		/// <summary>
		/// �����������޸ġ������޸ġ������޸ġ�������롢ɾ��
		/// <summary>
		public string OpType { get; set;}
		/// <summary>
		/// ������
		/// <summary>
		public string OpName { get; set;}
		/// <summary>
		/// �޸�ʱ��
		/// <summary>
		public Nullable<DateTime> OpTime { get; set;}
		/// <summary>
		/// �޸�ǰ��ͬʵ�ʽ��
		/// <summary>
		public Nullable<decimal> BfPayTotal { get; set;}
		/// <summary>
		/// �޸ĺ��ͬʵ�ʽ��
		/// <summary>
		public Nullable<decimal> PayTotal { get; set;}
		/// <summary>
		/// �޸�ǰ��Ŀ�������
		/// <summary>
		public Nullable<decimal> BfRate { get; set;}
		/// <summary>
		/// �޸ĺ���Ŀ�������
		/// <summary>
		public Nullable<decimal> Rate { get; set;}
		/// <summary>
		/// �޸�ǰ�Ѹ����
		/// <summary>
		public Nullable<decimal> BfPay { get; set;}
		/// <summary>
		/// �޸ĺ��Ѹ����
		/// <summary>
		public Nullable<decimal> Pay { get; set;}
		/// <summary>
		/// �޸�ǰ�ݹ����
		/// <summary>
		public Nullable<decimal> BfNotPay { get; set;}
		/// <summary>
		/// �޸ĺ��ݹ����
		/// <summary>
		public Nullable<decimal> NotPay { get; set;}

	}
}
