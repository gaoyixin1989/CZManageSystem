using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 营销订单-配送时限维护
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_Area
	{
		public Guid ID { get; set;}
		/// <summary>
		/// 地区编号
		/// <summary>
		public string DpCode { get; set;}
		/// <summary>
		/// 邮政所属区域
		/// <summary>
		public string DpName { get; set;}
		/// <summary>
		/// 配送时限（小时）
		/// <summary>
		public Nullable<int> LimitTime { get; set;}
		/// <summary>
		/// 序号
		/// <summary>
		public Nullable<int> Order { get; set;}

	}
}
