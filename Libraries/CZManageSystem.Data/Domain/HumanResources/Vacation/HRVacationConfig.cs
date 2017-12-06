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
		/// 主键
		/// <summary>
		public Guid ID { get; set;}
		/// <summary>
		/// 假期名称
		/// <summary>
		public string VacationName { get; set;}
		/// <summary>
		/// 假期限定天数
		/// <summary>
		public string Limit { get; set;}
		/// <summary>
		/// 假期天数
		/// <summary>
		public Nullable<decimal> SpanTime { get; set;}
		/// <summary>
		/// 范围
		/// <summary>
		public string Scope { get; set;}
		public string Daycalmethod { get; set;}

	}
}
