using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

/// <summary>
/// �ݼ�����
/// </summary>
namespace CZManageSystem.Data.Domain.HumanResources.Vacation
{
    //�ݼ������ѯ
    public class VacationApplyQueryBuilder
    {
        public string ApplyTitle { get; set; }//����
        public int? State { get; set; }//״̬
        public Guid? Editor { get; set; }//�ύ��
        public DateTime? EditTime_start { get; set; }//�ύʱ��
        public DateTime? EditTime_end { get; set; }
        public List<int> WF_State { get; set; }
    }
    public class HRVacationApply
	{
		/// <summary>
		/// ����
		/// <summary>
		public Guid ApplyId { get; set;}
		/// <summary>
		/// ����ʵ��Id
		/// <summary>
		public Nullable<Guid> WorkflowInstanceId { get; set;}
		/// <summary>
		/// ���̵���
		/// <summary>
		public string ApplySn { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public string ApplyTitle { get; set;}
		/// <summary>
		/// �༭��
		/// <summary>
		public Nullable<Guid> Editor { get; set; }
        /// <summary>
        /// �༭ʱ�䡢����ʱ��
        /// <summary>
        public Nullable<DateTime> EditTime { get; set; }
        /// <summary>
        /// �ݼ�����
        /// <summary>
        public string VacationType { get; set;}
		/// <summary>
		/// ��������
		/// <summary>
		public string VacationClass { get; set;}
		/// <summary>
		/// ��ʼʱ��
		/// <summary>
		public Nullable<DateTime> StartTime { get; set;}
		/// <summary>
		/// ����ʱ��
		/// <summary>
		public Nullable<DateTime> EndTime { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public Nullable<decimal> PeriodTime { get; set;}
		/// <summary>
		/// �ݼ�ԭ��
		/// <summary>
		public string Reason { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public Nullable<short> CancelVacation { get; set;}
		/// <summary>
		/// ����������
		/// <summary>
		public Nullable<decimal> CancelDays { get; set;}
		public string Newpt { get; set;}
		public string Newst { get; set;}
		public string Newet { get; set;}
		/// <summary>
		/// ����ص�
		/// <summary>
		public string OutAddress { get; set;}
		/// <summary>
		/// �Ӱ�ʱ��
		/// <summary>
		public string OverTime { get; set;}
		/// <summary>
		/// ����IDs
		/// <summary>
		public string Attids { get; set;}

        /// <summary>
        /// ״̬��0δ�ύ��1���ύ
        /// <summary>
        public Nullable<int> State { get; set; }


        //���
        public virtual Tracking_Workflow Tracking_Workflow { get; set; }
        public virtual Users EditorObj { get; set; }
        
        public virtual ICollection<HRVacationMeeting> Meetings { get; set; }
        public virtual ICollection<HRVacationCourses> Courses { get; set; }
        public virtual ICollection<HRVacationTeaching> Teachings { get; set; }
        public virtual ICollection<HRVacationOther> Others { get; set; }

    }
}
