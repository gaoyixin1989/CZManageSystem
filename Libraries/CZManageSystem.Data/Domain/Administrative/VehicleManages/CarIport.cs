using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.VehicleManages
{
	public class CarIport
	{
		/// <summary>
		/// 主键
		/// <summary>
		public Guid CarIportId { get; set;}
		/// <summary>
		/// 结束时间
		/// <summary>
		public Nullable<DateTime> EditTime { get; set;}
		/// <summary>
		/// 用户名
		/// <summary>
		public string UserName { get; set;}
		/// <summary>
		/// 手机
		/// <summary>
		public string Mobile { get; set;}
		/// <summary>
		/// 口？
		/// <summary>
		public Nullable<int> Iport { get; set;}
		/// <summary>
		/// 车辆申请ID
		/// <summary>
		public Nullable<int> CarApplyId { get; set;}
		/// <summary>
		/// 车辆ID
		/// <summary>
		public Nullable<Guid> CarId { get; set;}
		/// <summary>
		/// 状态
		/// <summary>
		public Nullable<int> Status { get; set;}

	}
}
