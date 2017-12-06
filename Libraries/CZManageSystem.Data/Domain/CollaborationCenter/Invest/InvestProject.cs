using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

/// <summary>
/// Ͷ����Ŀ��Ϣ
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.Invest
{
    //Ͷ����Ŀ��ѯ����
    public class InvestProjectQueryBuilder
    {
        public int? Year { get; set; }
        public string ProjectID { get; set; }//��Ŀ���
        public string ProjectName { get; set; }//��Ŀ����
        public string DpCode_Text { get; set; }//������
        public string ManagerID_Text { get; set; }//��Ŀ����
    }

    public class InvestProject
    {
        public Guid ID { get; set; }
        /// <summary>
        /// ��Ŀ��ţ�Ψһ��
        /// <summary>
        public string ProjectID { get; set;}
		/// <summary>
		/// �´����,Ҳ�ǵ���ʱ������
		/// <summary>
		public Nullable<int> Year { get; set;}
		/// <summary>
		/// �ƻ��������ĺ�
		/// <summary>
		public string TaskID { get; set;}
		/// <summary>
		/// ��Ŀ����
		/// <summary>
		public string ProjectName { get; set;}
		/// <summary>
		/// ��ֹ����
		/// <summary>
		public string BeginEnd { get; set;}
		/// <summary>
		/// ��Ŀ��Ͷ��
		/// <summary>
		public Nullable<decimal> Total { get; set;}
		/// <summary>
		/// �����ĿͶ��
		/// <summary>
		public Nullable<decimal> YearTotal { get; set;}
		/// <summary>
		/// ��Ƚ�������
		/// <summary>
		public string Content { get; set;}
		/// <summary>
		/// Ҫ�����ʱ��
		/// <summary>
		public string FinishDate { get; set;}
		/// <summary>
		/// ����רҵ��
		/// <summary>
		public string DpCode { get; set;}
		/// <summary>
		/// �Ҹ�����
		/// <summary>
		public Nullable<Guid> UserID { get; set;}
		/// <summary>
		/// ��Ŀ����
		/// <summary>
		public Nullable<Guid> ManagerID { get; set;}


        //���
        //public virtual Depts DeptObj { get; set; }
        //public virtual Users UserObj { get; set; }
        //public virtual Users ManagerObj { get; set; }

    }
}
