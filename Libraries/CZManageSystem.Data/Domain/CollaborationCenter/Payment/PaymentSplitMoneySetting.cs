using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.CollaborationCenter.Payment
{
	public class PaymentSplitMoneySetting
	{
		/// <summary>
		/// ID
		/// <summary>
		public Guid ID { get; set;}
		/// <summary>
		/// ²ðÕÊ½ð¶î
		/// <summary>
		public Nullable<decimal> SplitMoney { get; set;}

	}
}
