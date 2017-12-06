using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.CollaborationCenter.Payment
{
	public class PaymentReportReward
	{
		/// <summary>
		/// 日酬金ID
		/// <summary>
		public Guid RewardID { get; set;}
		/// <summary>
		/// 上报时间
		/// <summary>
		public Nullable<DateTime> ApplyTime { get; set;}
		/// <summary>
		/// 服营厅ID
		/// <summary>
		public Nullable<Guid> HallID { get; set;}
		/// <summary>
		/// 收款帐号
		/// <summary>
		public string PayeeAccount { get; set;}
		/// <summary>
		/// 金额
		/// <summary>
		public Nullable<decimal> PayMoney { get; set;}
		/// <summary>
		/// 酬金
		/// <summary>
		public Nullable<decimal> Reward { get; set;}
		/// <summary>
		/// 公司ID
		/// <summary>
		public Nullable<Guid> CompanyID { get; set;}

	}
}
