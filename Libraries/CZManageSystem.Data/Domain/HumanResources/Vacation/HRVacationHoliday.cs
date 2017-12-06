using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Data.Domain.HumanResources.Vacation
{
	public class HRVacationHoliday
	{
		public Guid ID { get; set;}
		/// <summary>
		/// �û�ID
		/// <summary>
		public Nullable<Guid> UserId { get; set;}
		/// <summary>
		/// ���
		/// <summary>
		public Nullable<int> YearDate { get; set;}
		/// <summary>
		/// �ݼ�����
		/// <summary>
		public string VacationType { get; set;}
		/// <summary>
		/// ��������
		/// <summary>
		public string VacationClass { get; set;}
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
		/// ԭ��
		/// <summary>
		public string Reason { get; set;}

        public virtual Users UserObj { get; set; }
        //public virtual Depts DeptObj { get; set; }

    }
}
