using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Vacation
{
	public class HRVacationAnnualLeaveConfig
	{
		public Guid ID { get; set;}
		/// <summary>
		/// ���
		/// <summary>
		public Nullable<int> Annual { get; set;}
		/// <summary>
		/// ��������
		/// <summary>
		public Nullable<decimal> SpanTime { get; set;}
		/// <summary>
		/// ���������ʹ���·�
		/// <summary>
		public string LimitMonth { get; set;}

	}

    public class HRVacationAnnualLeaveConfigQueryBuilder
    {
        
        public Nullable<int> Annual { get; set; }

    }
}
