using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// ��ͬ�Ѹ�����
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.Invest
{
    //��ͬ�Ѹ�����ѯ����
    public class InvestContractPayQueryBuilder
    {
        public string ProjectID { get; set; }//��Ŀ���
        public string ProjectName { get; set; }//��Ŀ����
        public string ContractID { get; set; }//��ͬ���
        public string ContractName { get; set; }//��ͬ����
        public string RowNote { get; set; }//��˵��(Ψһ��
        public string Batch { get; set; }//��
        public string DateAccount { get; set; }//�ռ��ʷ�¼
        public string Supply { get; set; }//��Ӧ��
        public decimal? Pay_start { get; set; }//�Ѹ�����
        public decimal? Pay_end { get; set; }//�Ѹ�����
    }

    public class InvestContractPay
	{
		public Guid ID { get; set;}
		/// <summary>
		/// ��
		/// <summary>
		public string Batch { get; set;}
		/// <summary>
		/// �ռ��ʷ�¼
		/// <summary>
		public string DateAccount { get; set;}
		/// <summary>
		/// ��˵��(Ψһ��
		/// <summary>
		public string RowNote { get; set;}
		/// <summary>
		/// ��Ŀ���
		/// <summary>
		public string ProjectID { get; set;}
		/// <summary>
		/// ��ͬ���
		/// <summary>
		public string ContractID { get; set;}
		/// <summary>
		/// ��Ӧ��
		/// <summary>
		public string Supply { get; set;}
		/// <summary>
		/// �Ѹ�����
		/// <summary>
		public Nullable<decimal> Pay { get; set;}
		/// <summary>
		/// ¼��ʱ��
		/// <summary>
		public Nullable<DateTime> Time { get; set;}
		/// <summary>
		/// ¼����
		/// <summary>
		public Nullable<Guid> UserID { get; set;}

	}
}
