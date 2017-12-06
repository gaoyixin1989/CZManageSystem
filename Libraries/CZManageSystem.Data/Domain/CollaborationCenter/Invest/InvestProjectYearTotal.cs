using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 年度投资金额
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.Invest
{
	public class InvestProjectYearTotal
	{
		public Guid ID { get; set;}
		/// <summary>
		/// 项目ID
		/// <summary>
		public string ProjectID { get; set;}
		/// <summary>
		/// 年份
		/// <summary>
		public Nullable<int> Year { get; set;}
		/// <summary>
		/// 年总计
		/// <summary>
		public Nullable<decimal> YearTotal { get; set;}

	}
}
