using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.VehicleManages
{
	public class CarStatus
	{
		public Guid Id { get; set;}
		/// <summary>
		/// 车辆ID
		/// <summary>
		public Nullable<Guid> CarId { get; set;}
		/// <summary>
		/// 车辆申请ID
		/// <summary>
		public Nullable<Guid> CarApplyId { get; set;}
		/// <summary>
		/// 预计结束时间
		/// <summary>
		public Nullable<DateTime> TimeOut { get; set;}
		/// <summary>
		/// 用车结束时间
		/// <summary>
		public Nullable<DateTime> FinishTime { get; set;}

	}
}
