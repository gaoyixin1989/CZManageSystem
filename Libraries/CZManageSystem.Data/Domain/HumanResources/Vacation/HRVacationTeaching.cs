using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// �ڲ���ʦ�ڿ�
/// </summary>
namespace CZManageSystem.Data.Domain.HumanResources.Vacation
{
	public class HRVacationTeaching
	{
        public Guid ID { get; set; }

        /// <summary>
        /// �ڿογ�����
        /// <summary>
        public string TeachingPlan { get; set;}
		/// <summary>
		/// ��ʦ����
		/// <summary>
		public string TeacherType { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public Nullable<decimal> PeriodTime { get; set;}
		/// <summary>
		/// ��ʼʱ��
		/// <summary>
		public Nullable<DateTime> StartTime { get; set;}
		/// <summary>
		/// ����ʱ��
		/// <summary>
		public Nullable<DateTime> EndTime { get; set;}
		/// <summary>
		/// ��ע
		/// <summary>
		public string Remark { get; set;}
		/// <summary>
		/// ���ڱ��ID
		/// <summary>
		public Nullable<Guid> VacationID { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public Nullable<decimal> Integral { get; set;}
		/// <summary>
		/// ͬ��ı�ʶ
		/// <summary>
		public Nullable<int> AgreeFlag { get; set;}
		/// <summary>
		/// ����ID
		/// <summary>
		public Nullable<Guid> ReVacationID { get; set;}
		public Nullable<Guid> UserId { get; set;}
		public string UserName { get; set;}
		public string Ftst { get; set;}
		public string Ftet { get; set;}
		public string Hispt { get; set;}

	}

    public class HRVacationTeachingQueryBuilder
    {
        public string EmployeeID { get; set; }//�ص�
        public string[] DpId { get; set; }//����ID
        public string RealName { get; set; }//����

        public string TeacherType { get; set; }//�ص�
        public string Year { get; set; }
    }
}
