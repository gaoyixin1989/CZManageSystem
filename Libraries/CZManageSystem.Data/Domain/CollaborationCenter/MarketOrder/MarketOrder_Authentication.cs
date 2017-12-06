using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 营销订单-鉴权方式维护
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_Authentication
	{
		public Guid ID { get; set;}
		/// <summary>
		/// 鉴权方式,不可重复
		/// <summary>
		public string Authentication { get; set;}
		/// <summary>
		/// 序号,可以重复
		/// <summary>
		public Nullable<decimal> Order { get; set;}

	}
}
