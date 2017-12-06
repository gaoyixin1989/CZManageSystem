using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 营销订单-开户结果信息
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_OrderBossDeal
	{
		public Guid ID { get; set;}
		/// <summary>
		/// 申请单ID
		/// <summary>
		public Nullable<Guid> ApplyID { get; set;}
		/// <summary>
		/// 操作人ID
		/// <summary>
		public Nullable<Guid> UserID { get; set;}
		/// <summary>
		/// 操作时间
		/// <summary>
		public Nullable<DateTime> Time { get; set;}
		/// <summary>
		/// 是否开户成功
		/// <summary>
		public string IsSuccess { get; set;}
		/// <summary>
		/// 是否回访
		/// <summary>
		public string IsVisit { get; set;}

	}
}
