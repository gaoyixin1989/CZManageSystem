using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 营销订单-商品维护
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_Product
	{
		public Guid ID { get; set;}
		/// <summary>
		/// 序号
		/// <summary>
		public string ProductID { get; set;}
		/// <summary>
		/// 商品名称
		/// <summary>
		public string ProductName { get; set;}
		/// <summary>
		/// 商品机型
		/// <summary>
		public Nullable<Guid> ProductTypeID { get; set;}
		/// <summary>
		/// 说明
		/// <summary>
		public string Remark { get; set;}

	}
}
