using System;
using System.ComponentModel.DataAnnotations;

namespace CZManageSystem.Data.Domain.HumanResources.Attendance
{
	public class HRHolidays
	{
		public Guid ID { get; set;}
		/// <summary>
		/// �༭��ID
		/// <summary>
		public Nullable<Guid> EditorId { get; set;}
		/// <summary>
		/// �༭��
		/// <summary>
		public string Editor { get; set;}
		/// <summary>
		/// �༭ʱ��
		/// <summary>
		public Nullable<DateTime> EditTime { get; set;}
        /// <summary>
        /// ��������
        /// <summary>
        [Required(ErrorMessage = "�������Ʋ���Ϊ��")]
        public string HolidayName { get; set;}
        /// <summary>
        /// ���
        /// <summary>
        [Required(ErrorMessage = "��Ȳ���Ϊ����Ϊ����")]
        public Nullable<DateTime> HolidayYear { get; set;}
        /// <summary>
        /// ��ʼʱ��
        /// <summary>
        [Required(ErrorMessage = "��ʼʱ�䲻��Ϊ����Ϊ���ڸ�ʽ")]
        public Nullable<DateTime> StartTime { get; set;}
        /// <summary>
        /// ����ʱ�� 
        /// <summary>
        [Required(ErrorMessage = "����ʱ�䲻��Ϊ����Ϊ���ڸ�ʽ")]
        public Nullable<DateTime> EndTime { get; set;}
		/// <summary>
		/// ��ע
		/// <summary>
		public string Remark { get; set;}
        /// <summary>
        /// �������1�����ݼ��գ�2����������
        /// <summary>
        [Required(ErrorMessage = "���������Ϊ��")]
        public string HolidayClass { get; set;}

	}
}
