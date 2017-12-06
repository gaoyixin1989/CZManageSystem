using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 营销订单-回访情况
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_OrderVisit
	{
		public Guid ID { get; set;}
		/// <summary>
		/// 申请单ID
		/// <summary>
		public Nullable<Guid> ApplyID { get; set;}
		/// <summary>
		/// 用户ID
		/// <summary>
		public Nullable<Guid> UserID { get; set;}
		public Nullable<DateTime> Time { get; set;}
		/// <summary>
		/// 满意度ID
		/// <summary>
		public Nullable<Guid> SatID { get; set;}
		/// <summary>
		/// 成功备注
		/// <summary>
		public string SuccessRemark { get; set;}
		/// <summary>
		/// 失败回访ID
		/// <summary>
		public Nullable<Guid> VisitID { get; set;}
		/// <summary>
		/// 失败备注
		/// <summary>
		public string FailRemark { get; set;}
		public string IsAgain { get; set;}

	}
}
