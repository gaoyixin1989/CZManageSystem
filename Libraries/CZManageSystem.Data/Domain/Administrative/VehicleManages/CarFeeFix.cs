using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.VehicleManages
{
	public class CarFeeFix
	{
		/// <summary>
		/// 主键
		/// <summary>
		public Guid CarFeeFixId { get; set;}
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
		/// 车辆ID
		/// <summary>
		public Nullable<Guid> CarId { get; set;}
		/// <summary>
		/// 缴费日期
		/// <summary>
		public Nullable<DateTime> PayTime { get; set;}
		/// <summary>
		/// 保险费
		/// <summary>
		public Nullable<decimal> FolicyFee { get; set;}
		/// <summary>
		/// 车船税
		/// <summary>
		public Nullable<decimal> TaxFee { get; set;}
		/// <summary>
		/// 公路基金
		/// <summary>
		public Nullable<decimal> RoadFee { get; set;}
		/// <summary>
		/// 其它杂费
		/// <summary>
		public Nullable<decimal> OtherFee { get; set;}
		/// <summary>
		/// 费用小计
		/// <summary>
		public Nullable<decimal> TotalFee { get; set;}
		/// <summary>
		/// 计费开始日期
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
