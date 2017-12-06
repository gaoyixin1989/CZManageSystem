using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 年度投资金额视图
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.Invest
{
	public class V_InvestProjectYearTotal
	{
		public Guid ID { get; set;}
		public string ProjectID { get; set;}
		public Nullable<int> Year { get; set;}
		public Nullable<decimal> YearTotal { get; set;}
		public string ProjectName { get; set;}
		public Nullable<decimal> Total { get; set;}
		public Nullable<Guid> ManagerID { get; set;}

	}
}
