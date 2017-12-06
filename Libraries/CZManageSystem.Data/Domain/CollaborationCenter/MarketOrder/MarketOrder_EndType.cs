using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 营销订单-终端机型维护
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_EndType
	{
		public Guid ID { get; set;}
		/// <summary>
		/// 机型名称
		/// <summary>
		public string EndType { get; set;}
		/// <summary>
		/// 序号
		/// <summary>
		public Nullable<decimal> Order { get; set;}
		/// <summary>
		/// 所属方案
		/// <summary>
		public Nullable<Guid> MarketID { get; set;}
		/// <summary>
		/// 说明
		/// <summary>
		public string Remark { get; set;}

	}
}
