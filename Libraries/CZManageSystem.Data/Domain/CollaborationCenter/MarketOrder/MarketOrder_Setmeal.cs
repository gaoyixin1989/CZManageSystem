using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 营销订单-基本套餐维护
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_Setmeal
	{
		public Guid ID { get; set;}
		/// <summary>
		/// 套餐名称
		/// <summary>
		public string Setmeal { get; set;}
		/// <summary>
		/// 序号
		/// <summary>
		public Nullable<decimal> Order { get; set;}
		/// <summary>
		/// 说明备注
		/// <summary>
		public string Remark { get; set;}

	}
}
