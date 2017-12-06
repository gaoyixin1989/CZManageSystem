using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class InvestEstimate
	{
		public Guid ID { get; set;}
		public Nullable<int> Year { get; set;}
		public Nullable<int> Month { get; set;}
		public string ProjectName { get; set;}
		public string ProjectID { get; set;}
		public string ContractName { get; set;}
		public string ContractID { get; set;}
		public string Supply { get; set;}
		public Nullable<decimal> SignTotal { get; set;}
		public Nullable<decimal> PayTotal { get; set;}
		public string Study { get; set;}
		public Nullable<Guid> ManagerID { get; set;}
		public string Course { get; set;}
		public Nullable<decimal> BackRate { get; set;}
		public Nullable<decimal> Rate { get; set;}
		public Nullable<decimal> Pay { get; set;}
		public Nullable<decimal> NotPay { get; set;}
		public Nullable<Guid> EstimateUserID { get; set;}
        public virtual Users ManagerObj { get; set; }
        public virtual Users UserObj { get; set; }

    }
    //�ݹ���ѯ
    public class InvestEstimateQueryBuilder
    {
        public Nullable<int> Year { get; set; }//��
        public Nullable<int> Month { get; set; }//��
        public string ProjectID { get; set; }//��Ŀ���
        public string ProjectName { get; set; }//��Ŀ����
        public string ContractID { get; set; }//��ͬ���
        public string ContractName { get; set; }//��ͬ����
        public string Dpfullname { get; set; }//����
        public string Supply { get; set; }//��Ӧ��
        public string Study { get; set; }//����רҵ
        public string Course { get; set; }//��Ŀ
        public string ManagerName { get; set; }//������
        public Nullable<decimal> SignTotal_start { get; set; }//��ͬ���
        public Nullable<decimal> SignTotal_end { get; set; }//��ͬ���
        public Nullable<decimal> PayTotal_start { get; set; }//ʵ�ʺ�ͬ���
        public Nullable<decimal> PayTotal_end { get; set; }//ʵ�ʺ�ͬ���
        public Nullable<decimal> Pay_start { get; set; }//�Ѹ�����
        public Nullable<decimal> Pay_end { get; set; }//�Ѹ�����
        public Nullable<decimal> BackRate_start { get; set; }//�����������
        public Nullable<decimal> BackRate_end { get; set; }//�����������
        public Nullable<decimal> Rate_start { get; set; }//�����������
        public Nullable<decimal> Rate_end { get; set; }//�����������
        public Nullable<decimal> NotPay_start { get; set; }//�ݹ����
        public Nullable<decimal> NotPay_end { get; set; }//�ݹ����
        public Nullable<decimal> Tax_start { get; set; }//��ͬ˰���
        public Nullable<decimal> Tax_end { get; set; }//��ͬ˰���
    }
}
