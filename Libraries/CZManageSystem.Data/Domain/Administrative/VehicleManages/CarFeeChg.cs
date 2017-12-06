using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.VehicleManages
{
	public class CarFeeChg
	{
		public Guid CarFeeChgId { get; set;}
		/// <summary>
		/// 编辑人ID
		/// <summary>
		public Nullable<Guid> EditorId { get; set;}
		/// <summary>
		/// 编辑日期
		/// <summary>
		public Nullable<DateTime> EditTime { get; set;}
		/// <summary>
		/// 所属单位
		/// <summary>
		public Nullable<int> CorpId { get; set;}
		/// <summary>
		/// 使用单位
		/// <summary>
		public string CorpName { get; set;}
		/// <summary>
		/// 车辆ID
		/// <summary>
		public Nullable<Guid> CarId { get; set;}
		/// <summary>
		/// 缴费日期
		/// <summary>
		public Nullable<DateTime> PayTime { get; set;}
		/// <summary>
		/// 上月公里数
		/// <summary>
		public Nullable<decimal> RoadLast { get; set;}
		/// <summary>
		/// 本月公里数
		/// <summary>
		public Nullable<decimal> RoadThis { get; set;}
		/// <summary>
		/// 本月行驶公里数
		/// <summary>
		public Nullable<decimal> RoadCount { get; set;}
		/// <summary>
		/// 实用油量
		/// <summary>
		public Nullable<decimal> OilCount { get; set;}
		/// <summary>
		/// 油价
		/// <summary>
		public Nullable<decimal> OilPrice { get; set;}
		/// <summary>
		/// 汽油费
		/// <summary>
		public Nullable<decimal> OilFee { get; set;}
		/// <summary>
		/// 维修费
		/// <summary>
		public Nullable<decimal> FixFee { get; set;}
		/// <summary>
		/// 路桥/停车费
		/// <summary>
		public Nullable<decimal> RoadFee { get; set;}
		/// <summary>
		/// 住宿费
		/// <summary>
		public Nullable<decimal> LiveFee { get; set;}
		/// <summary>
		/// 餐费
		/// <summary>
		public Nullable<decimal> EatFee { get; set;}
		/// <summary>
		/// 其它杂费
		/// <summary>
		public Nullable<decimal> OtherFee { get; set;}
		/// <summary>
		/// 费用小计
		/// <summary>
		public Nullable<decimal> TotalFee { get; set;}
		/// <summary>
		/// 经手人
		/// <summary>
		public string Person { get; set;}
		/// <summary>
		/// 备注
		/// <summary>
		public string Remark { get; set;}

        public virtual CarInfo CarInfo { get; set; }

    }
}
