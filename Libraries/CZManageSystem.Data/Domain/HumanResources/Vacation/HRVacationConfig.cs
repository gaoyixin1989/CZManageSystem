using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Vacation
{
	public class HRVacationConfig
	{
		/// <summary>
		/// ����
		/// <summary>
		public Guid ID { get; set;}
		/// <summary>
		/// ��������
		/// <summary>
		public string VacationName { get; set;}
		/// <summary>
		/// �����޶�����
		/// <summary>
		public string Limit { get; set;}
		/// <summary>
		/// ��������
		/// <summary>
		public Nullable<decimal> SpanTime { get; set;}
		/// <summary>
		/// ��Χ
		/// <summary>
		public string Scope { get; set;}
		public string Daycalmethod { get; set;}

	}
}
