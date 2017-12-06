using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// ������ѵ
/// </summary>
namespace CZManageSystem.Data.Domain.HumanResources.Vacation
{
	public class HRVacationCourses
	{
		public Guid CoursesId { get; set;}
		/// <summary>
		/// �γ�����
		/// <summary>
		public string CoursesName { get; set;}
		/// <summary>
		/// �γ����
		/// <summary>
		public string CoursesType { get; set;}
		/// <summary>
		/// ���쵥λ
		/// <summary>
		public string ProvinceCity { get; set;}
		/// <summary>
		/// ��ʼʱ��
		/// <summary>
		public Nullable<DateTime> StartTime { get; set;}
		/// <summary>
		/// ����ʱ��
		/// <summary>
		public Nullable<DateTime> EndTime { get; set;}
		/// <summary>
		/// ��ѵ����
		/// <summary>
		public Nullable<decimal> PeriodTime { get; set;}
		/// <summary>
		/// ��ע
		/// <summary>
		public string Remark { get; set;}
		/// <summary>
		/// ���ڱ�ID
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
		public Nullable<Guid> UserId { get; set;}
		public Nullable<int> DaoId { get; set;}
		public string UserName { get; set;}
		public string Ftst { get; set;}
		public string Ftet { get; set;}
		public string Hispt { get; set;}
		/// <summary>
		/// ����ID
		/// <summary>
		public Nullable<Guid> ReVacationID { get; set;}

	}
    public class HRVacationCoursesQueryBuilder
    {
        public string EmployeeID { get; set; }//�ص�
        public string[] DpId { get; set; }//����ID
        public string RealName { get; set; }//����

        public string CoursesType { get; set; }//�ص�
        public string ProvinceCity { get; set; }//����
        public string Year { get; set; }
    }
}
