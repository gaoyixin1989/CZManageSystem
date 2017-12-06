using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 营销订单-受理单状态维护
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_OrderStatus
	{
		public Guid ID { get; set;}
		/// <summary>
		/// 序号
		/// <summary>
		public Nullable<int> Order { get; set;}
		/// <summary>
		/// 受理单状态
		/// <summary>
		public string OrderStatus { get; set;}
		/// <summary>
		/// 说明
		/// <summary>
		public string Remark { get; set;}

	}
}
