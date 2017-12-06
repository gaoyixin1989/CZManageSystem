using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 营销订单-捆绑业务维护
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_Business
	{
		public Guid ID { get; set;}
		/// <summary>
		/// 捆绑业务
		/// <summary>
		public string Business { get; set;}
		/// <summary>
		/// 序号
		/// <summary>
		public Nullable<decimal> Order { get; set;}
		/// <summary>
		/// 说明
		/// <summary>
		public string Remark { get; set;}

	}
}
