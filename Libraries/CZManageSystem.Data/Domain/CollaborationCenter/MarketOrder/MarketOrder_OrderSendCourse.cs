using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 营销订单-配送过程反馈
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_OrderSendCourse
	{
		public string ID { get; set;}
		/// <summary>
		/// 申请单ID
		/// <summary>
		public string ApplyID { get; set;}
		/// <summary>
		/// 操作人ID
		/// <summary>
		public string UserID { get; set;}
		/// <summary>
		/// 操作时间
		/// <summary>
		public Nullable<DateTime> Time { get; set;}
		/// <summary>
		/// 反馈时间
		/// <summary>
		public Nullable<DateTime> SendTime { get; set;}
		/// <summary>
		/// 反馈内容
		/// <summary>
		public string SendCourse { get; set;}

	}
}
