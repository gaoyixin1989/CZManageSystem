using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 营销订单-项目编号维护
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_Project
	{
		public Guid ID { get; set;}
		/// <summary>
		/// 项目编号，不可重复
		/// <summary>
		public string ProjectID { get; set;}
		/// <summary>
		/// 序号，可重复
		/// <summary>
		public Nullable<decimal> Order { get; set;}

	}
}
