using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.CollaborationCenter.Payment
{
	public class PaymentRewardScale
	{
		/// <summary>
		/// ±ÈÀýID
		/// <summary>
		public Guid ScaleID { get; set;}
		public Nullable<decimal> Scale { get; set;}
		public Nullable<DateTime> BeginTime { get; set;}
		public Nullable<DateTime> EndTime { get; set;}
		public Nullable<Guid> CompanyID { get; set;}
		public string CompanyName { get; set;}

	}
}
