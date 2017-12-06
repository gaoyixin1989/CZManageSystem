using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 营销订单-营销方案维护
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_Market
	{
		/// <summary>
		/// 编号
		/// <summary>
		public Guid ID { get; set;}
		/// <summary>
		/// 营销方案名称
		/// <summary>
		public string Market { get; set;}
        /// <summary>
        /// 营销方案编号
        /// <summary>
        public string Order { get; set;}
		/// <summary>
		/// 生效时间
		/// <summary>
		public Nullable<DateTime> AbleTime { get; set;}
		/// <summary>
		/// 失效时间
		/// <summary>
		public Nullable<DateTime> DisableTime { get; set;}
		/// <summary>
		/// 备注说明
		/// <summary>
		public string Remark { get; set;}
		/// <summary>
		/// 优惠费用
		/// <summary>
		public Nullable<decimal> PlanPay { get; set;}
		/// <summary>
		/// 实收费用
		/// <summary>
		public Nullable<decimal> MustPay { get; set;}
		/// <summary>
		/// 是否家宽业务
		/// <summary>
		public Nullable<bool> isJK { get; set;}

	}
}
