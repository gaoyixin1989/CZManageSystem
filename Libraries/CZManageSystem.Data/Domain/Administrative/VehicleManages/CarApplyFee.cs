using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.VehicleManages
{
	public class CarApplyFee
	{
		/// <summary>
		/// 主键
		/// <summary>
		public Guid ApplyFeeId { get; set;}
        /// <summary>
		/// 流程实例Id
		/// <summary>
		public Nullable<Guid> CarApplyId { get; set; }
        /// <summary>
        /// 流程单号
        /// </summary>
        public string ApplySn { get; set; }
        /// <summary>
        /// 标题
        /// <summary>
        public string ApplyTitle { get; set; }
        /// <summary>
        /// 车辆ID
        /// <summary>
        public Nullable<Guid> CarId { get; set;}
		/// <summary>
		/// 结算人
		/// <summary>
		public string BalUser { get; set;}
		/// <summary>
		/// 结算时间
		/// <summary>
		public Nullable<DateTime> BalTime { get; set;}
		/// <summary>
		/// 起始里程
		/// <summary>
		public Nullable<int> KmNum1 { get; set;}
		/// <summary>
		/// 终止里程
		/// <summary>
		public Nullable<int> KmNum2 { get; set;}
		/// <summary>
		/// 使用里程
		/// <summary>
		public Nullable<int> KmCount { get; set;}
		/// <summary>
		/// 单据数量
		/// <summary>
		public Nullable<int> BalCount { get; set;}
		/// <summary>
		/// 合计金额
		/// <summary>
		public Nullable<decimal> BalTotal { get; set;}
		/// <summary>
		/// 备注
		/// <summary>
		public string BalRemark { get; set;}

        public virtual CarApply CarApply { get; set; }

    }
}
