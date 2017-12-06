using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 营销订单-回收结果信息
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_OrderReclaim
	{
		/// <summary>
		/// 编号
		/// <summary>
		public Guid ID { get; set;}
		/// <summary>
		/// 申请单ID
		/// <summary>
		public Nullable<Guid> ApplyID { get; set;}
		/// <summary>
		/// 操作用户ID
		/// <summary>
		public Nullable<Guid> UserID { get; set;}
		/// <summary>
		/// 操作时间
		/// <summary>
		public Nullable<DateTime> Time { get; set;}
		/// <summary>
		/// 回收结果
		/// <summary>
		public string IsReclaim { get; set;}
		/// <summary>
		/// 备注
		/// <summary>
		public string ReclaimRemark { get; set;}

	}
}
