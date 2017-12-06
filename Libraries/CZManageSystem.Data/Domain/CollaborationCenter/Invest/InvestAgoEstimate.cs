using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

/// <summary>
/// ��ʷ��Ŀ�ݹ�
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.Invest
{

    /// <summary>
    /// ��ʷ��Ŀ�ݹ���ѯ
    /// </summary>
    public class AgoEstimateQueryBuilder
    {
        public string Dept_Text { get; set; }//�ݹ�����
        public decimal? NotPay_start { get; set; }//�ݹ����
        public decimal? NotPay_end { get; set; }//�ݹ����
        public string ProjectID { get; set; }//��Ŀ���
        public string ProjectName { get; set; }//��Ŀ����
        public string ContractID { get; set; }//��ͬ���
        public string ContractName { get; set; }//��ͬ����
        public decimal? SignTotal_start { get; set; }//��ͬ���
        public decimal? SignTotal_end { get; set; }//��ͬ���
        public decimal? PayTotal_start { get; set; }//ʵ�ʺ�ͬ���
        public decimal? PayTotal_end { get; set; }//ʵ�ʺ�ͬ���
        public string Supply { get; set; }//��Ӧ��
        public string Study { get; set; }//����רҵ
        public string Course { get; set; }//��Ŀ
        public string ManagerID_Text { get; set; }//������
        public decimal? Rate_start { get; set; }//�������
        public decimal? Rate_end { get; set; }//�������
        public decimal? Pay_start { get; set; }//�Ѹ�����
        public decimal? Pay_end { get; set; }//�Ѹ�����

    }


    public class InvestAgoEstimate
	{
		/// <summary>
		/// ΨһID
		/// <summary>
		public Guid ID { get; set;}
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
		/// ��ĿID
		/// <summary>
		public string ProjectID { get; set;}
		/// <summary>
		/// ��ͬ����
		/// <summary>
		public string ContractName { get; set;}
		/// <summary>
		/// ��ͬID
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


        //���
        public virtual Users ManagerObj { get; set; }
        public virtual Users EstimateUserObj { get; set; }
    }
}
