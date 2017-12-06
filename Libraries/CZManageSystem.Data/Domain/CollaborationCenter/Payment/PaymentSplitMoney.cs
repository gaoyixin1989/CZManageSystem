using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.CollaborationCenter.Payment
{
	public class PaymentSplitMoney
	{
		/// <summary>
		/// 拆账ID
		/// <summary>
		public Guid SplitID { get; set;}
		/// <summary>
		/// 总金额ID
		/// <summary>
		public Nullable<Guid> PayMoneyID { get; set;}
		/// <summary>
		/// 拆帐金额ID
		/// <summary>
		public Nullable<decimal> SplitMoney { get; set;}
		/// <summary>
		/// 用途
		/// <summary>
		public string Purpose { get; set;}

	}
}
