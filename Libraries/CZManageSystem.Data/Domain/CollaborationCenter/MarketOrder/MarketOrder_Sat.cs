using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 营销订单-满意度维护
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_Sat
	{
		public Guid ID { get; set;}
		/// <summary>
		/// 满意度
		/// <summary>
		public string Sat { get; set;}
		/// <summary>
		/// 序号
		/// <summary>
		public Nullable<decimal> Order { get; set;}

	}
}
