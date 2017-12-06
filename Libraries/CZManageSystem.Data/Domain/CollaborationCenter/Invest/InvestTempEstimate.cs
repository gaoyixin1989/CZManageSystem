using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.CollaborationCenter.Invest;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class InvestTempEstimate
	{
		public Guid ID { get; set;}
		/// <summary>
		/// ��ĿID
		/// <summary>
		public string ProjectID { get; set;}
		/// <summary>
		/// ��ͬID
		/// <summary>
		public string ContractID { get; set;}
		/// <summary>
		/// ��Ӧ��
		/// <summary>
		public string Supply { get; set;}
		/// <summary>
		/// ��ͬ���
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
		/// �Ƿ�����
		/// <summary>
		public string IsLock { get; set;}
		/// <summary>
		/// �ݹ���ԱID
		/// <summary>
		public Nullable<Guid> EstimateUserID { get; set;}
		/// <summary>
		/// ��ǰ״̬
		/// <summary>
		public string Status { get; set;}
		/// <summary>
		/// ״̬����ʱ��
		/// <summary>
		public Nullable<DateTime> StatusTime { get; set;}

        public virtual Users ManagerObj { get; set; }
      

    }

    //����ֹ �ݹ�
    public class StopInvestTempEstimateQueryBuilder
    {
        public DateTime? StatusTime_Start { get; set; }//��ֹʱ��
        public DateTime? StatusTime_End { get; set; }
        public string EstimateUserName { get; set; }//�ݹ���       
        public string ProjectID { get; set; }//��Ŀ���
        public string ProjectName { get; set; }//��Ŀ����
        public string ContractID { get; set; }//��ͬ���
        public string ContractName { get; set; }//��ͬ����
        public string Status { get; set; }

    }
}
