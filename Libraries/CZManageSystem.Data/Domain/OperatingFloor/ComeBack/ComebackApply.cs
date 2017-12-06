using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class ComebackApply
	{  
		public Guid ApplyId { get; set;} 
		public Nullable<Guid> WorkflowInstanceId { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public string Title { get; set;} 
		/// <summary>
		/// ���̵���
		/// <summary>
		public string Series { get; set;}
		/// <summary>
		/// �绰
		/// <summary>
		public string Mobile { get; set;}
		/// <summary>
		/// ״̬
		/// <summary>
		public int Status { get; set;}
		/// <summary>
		/// ����ʱ��
		/// <summary>
		public Nullable<DateTime> ApplyTime { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public string ApplyDept { get; set;}
		/// <summary>
		/// ������
		/// <summary>
		public string ApplyUser { get; set;}
		/// <summary>
		/// Ԥ��������
		/// <summary>
		public string BudgetDept { get; set;}
		/// <summary>
		/// ������Դ���
		/// <summary>
		public Nullable<Guid> SourceTypeID { get; set;}
		/// <summary>
		/// ��Ŀ��ʼʱ��
		/// <summary>
		public Nullable<DateTime> TimeStart { get; set;}
		/// <summary>
		/// ��Ŀ����ʱ��
		/// <summary>
		public Nullable<DateTime> TimeEnd { get; set;}
		public string SourceChildId { get; set;}
		/// <summary>
		/// �⿪չ��Ŀ����
		/// <summary>
		public string ProjName { get; set;}
		/// <summary>
		/// ����������Ԥ����Ŀ����
		/// <summary>
		public string PrevProjName { get; set;}
		/// <summary>
		/// ����������Ԥ����Ŀ���
		/// <summary>
		public string PrevProjCode { get; set;}
		/// <summary>
		/// ��Ŀ������
		/// <summary>
		public string ProjManager { get; set;}
		/// <summary>
		/// ����˰���������
		/// <summary>
		public Nullable<decimal> AppAmount { get; set;}
		/// <summary>
		/// ��Ŀ��չ��Ҫ�Լ�Ч���Է���
		/// <summary>
		public string ProjAnalysis { get; set;}
		/// <summary>
		/// Ԥ�����
		/// <summary>
		public Nullable<int> Year { get; set;}
		/// <summary>
		/// ��ע
		/// <summary>
		public string Remark { get; set;}
		/// <summary>
		/// ��˰���������
		/// <summary>
		public Nullable<decimal> AppAmountHanshui { get; set;}

	}

    public class ComebackInfoQueryBuilder
    {
        public int? YearStart { get; set; }
        public int? YearEnd { get; set; }
        public string Name { get; set; }

        public string BudgetDept { get; set; }
        /// <summary>
		/// ������
		/// <summary>
		public string ApplyUser { get; set; }

    }
}
