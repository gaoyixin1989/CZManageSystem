using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.VehicleManages
{
	public class CarFeeRent
	{
		/// <summary>
		/// 主键
		/// <summary>
		public Guid CarFeeRentId { get; set;}
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
		/// 缴费种类
		/// <summary>
		public Nullable<int> SortId { get; set;}
		/// <summary>
		/// 租赁费用
		/// <summary>
		public Nullable<decimal> RentFee { get; set;}
		/// <summary>
		/// 租赁公里
		/// <summary>
		public Nullable<decimal> RentCount { get; set;}
		/// <summary>
		/// 实际行驶公里
		/// <summary>
		public Nullable<decimal> RoadCount { get; set;}
		/// <summary>
		/// 超包租里程
		/// <summary>
		public Nullable<decimal> MoreRoad { get; set;}
		/// <summary>
		/// 超包租里程费用
		/// <summary>
		public Nullable<decimal> MoreFee { get; set;}
		/// <summary>
		/// 汽油费
		/// <summary>
		public Nullable<decimal> GasFee { get; set;}
		/// <summary>
		/// 路桥/停车费
		/// <summary>
		public Nullable<decimal> RoadFee { get; set;}
		/// <summary>
		/// 驾驶员补贴
		/// <summary>
		public Nullable<decimal> DriverFee { get; set;}
		/// <summary>
		/// 费用小计
		/// <summary>
		public Nullable<decimal> TotalFee { get; set;}
		/// <summary>
		/// 开始日期
		/// <summary>
		public Nullable<DateTime> StartTime { get; set;}
		/// <summary>
		/// 结束日期
		/// <summary>
		public Nullable<DateTime> EndTime { get; set;}
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
