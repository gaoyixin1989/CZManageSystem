using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 营销订单-失败回访维护
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_Visit
	{
		public Guid ID { get; set;}
		/// <summary>
		/// 失败回访情况
		/// <summary>
		public string Visit { get; set;}
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
