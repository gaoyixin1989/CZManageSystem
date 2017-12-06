using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

/// <summary>
/// ��ͬ��Ϣ
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.Invest
{
    //��ͬ��Ϣ��ѯ����
    public class InvestContractQueryBuilder
    {
        public string ProjectID { get; set; }//��Ŀ���
        public string ProjectName { get; set; }//��Ŀ����
        public string ContractID { get; set; }//��ͬ���
        public string ContractName { get; set; }//��ͬ����
        public string IsMIS { get; set; }//�Ƿ�MIS����
        public string IsDel { get; set; }//�Ƿ�ɾ��

        public string DpCode_Text { get; set; }//���첿��
        public string User_Text { get; set; }//������
        public DateTime? SignTime_start { get; set; }//ǩ��ʱ��
        public DateTime? SignTime_end { get; set; }
        public string Content { get; set; }//��ע
        public decimal? SignTotal_start { get; set; }//��ͬ��Ŀ����ͬ����˰���
        public decimal? SignTotal_end { get; set; }
        public decimal? AllTotal_start { get; set; }//��ͬ�ܽ��
        public decimal? AllTotal_end { get; set; }
        public decimal? PayTotal_start { get; set; }//ʵ�ʺ�ͬ���
        public decimal? PayTotal_end { get; set; }
    }

    public class InvestContract
	{
		public Guid ID { get; set;}
		/// <summary>
		/// ����ʱ�䣬ͬ�����ò���֮һ
		/// <summary>
		public Nullable<DateTime> ImportTime { get; set;}
		/// <summary>
		/// ��Ŀ���
		/// <summary>
		public string ProjectID { get; set;}
		/// <summary>
		/// ��ͬ���
		/// <summary>
		public string ContractID { get; set;}
		/// <summary>
		/// ��ͬ����
		/// <summary>
		public string ContractName { get; set;}
		/// <summary>
		/// ��Ӧ��
		/// <summary>
		public string Supply { get; set;}
		/// <summary>
		/// ǩ��ʱ��
		/// <summary>
		public Nullable<DateTime> SignTime { get; set;}
		/// <summary>
		/// ��ͬ���첿��
		/// <summary>
		public string DpCode { get; set;}
		/// <summary>
		/// ������
		/// <summary>
		public Nullable<Guid> UserID { get; set;}
		/// <summary>
		/// ��ͬ��Ŀ����ͬ����˰���(Ԫ)
		/// <summary>
		public Nullable<decimal> SignTotal { get; set;}
		/// <summary>
		/// ��ͬ�ܽ��
		/// <summary>
		public Nullable<decimal> AllTotal { get; set;}
		/// <summary>
		/// ʵ�ʺ�ͬ���
		/// <summary>
		public Nullable<decimal> PayTotal { get; set;}
		/// <summary>
		/// ��ע
		/// <summary>
		public string Content { get; set;}
		/// <summary>
		/// �Ƿ�MIS����
		/// <summary>
		public string IsMIS { get; set;}
		/// <summary>
		/// �Ƿ�ɾ��
		/// <summary>
		public string IsDel { get; set;}
		/// <summary>
		/// ��ͬ��ˮ��
		/// <summary>
		public string ContractSeries { get; set;}
		/// <summary>
		/// ��ͬ˰��
		/// <summary>
		public Nullable<decimal> Tax { get; set;}
		/// <summary>
		/// ��ͬ��˰���(Ԫ
		/// <summary>
		public Nullable<decimal> SignTotalTax { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public string Currency { get; set;}
		/// <summary>
		/// ��ͬ״̬
		/// <summary>
		public string ContractState { get; set;}
		/// <summary>
		/// ��ͬ����
		/// <summary>
		public string Attribute { get; set;}
		/// <summary>
		/// ������ʼʱ��
		/// <summary>
		public Nullable<DateTime> ApproveStartTime { get; set;}
		/// <summary>
		/// ��������ʱ��
		/// <summary>
		public Nullable<DateTime> ApproveEndTime { get; set;}
		/// <summary>
		/// ��ͬ������
		/// <summary>
		public string ContractFilesNum { get; set;}
		/// <summary>
		/// ӡ��˰��
		/// <summary>
		public string StampTaxrate { get; set;}
		/// <summary>
		/// ӡ��˰��
		/// <summary>
		public string Stamptax { get; set;}
		public string ContractOpposition { get; set;}
		/// <summary>
		/// ������
		/// <summary>
		public string RequestDp { get; set;}
		/// <summary>
		/// ��ز���
		/// <summary>
		public string RelevantDp { get; set;}
		/// <summary>
		/// ��Ŀ��չԭ��
		/// <summary>
		public string ProjectCause { get; set;}
		/// <summary>
		/// ��ͬ����
		/// <summary>
		public string ContractType { get; set;}
		/// <summary>
		/// ��ͬ�Է���Դ
		/// <summary>
		public string ContractOppositionFrom { get; set;}
		/// <summary>
		/// ��ͬ�Է�ѡ��ʽ
		/// <summary>
		public string ContractOppositionType { get; set;}
		/// <summary>
		/// �ɹ���ʽ
		/// <summary>
		public string Purchase { get; set;}
		/// <summary>
		/// ���ʽ
		/// <summary>
		public string PayType { get; set;}
		/// <summary>
		/// ����˵��
		/// <summary>
		public string PayRemark { get; set;}
		/// <summary>
		/// ��ͬ��Ч������ʼ
		/// <summary>
		public Nullable<DateTime> ContractStartTime { get; set;}
		/// <summary>
		/// ��ͬ��Ч������ֹ
		/// <summary>
		public Nullable<DateTime> ContractEndTime { get; set;}
		/// <summary>
		/// ��ܺ�ͬ
		/// <summary>
		public string IsFrameContract { get; set;}
		/// <summary>
		/// ���ʱ��
		/// <summary>
		public Nullable<DateTime> DraftTime { get; set;}
		/// <summary>
		/// ��Ŀ���
		/// <summary>
		public Nullable<decimal> ProjectTotal { get; set;}
		/// <summary>
		/// ��ǩ����Ŀ�ܶ�
		/// <summary>
		public Nullable<decimal> ProjectAllTotal { get; set;}

        
        //���
        //public virtual Depts DeptObj { get; set; }
        //public virtual Users UserObj { get; set; }

    }



}
