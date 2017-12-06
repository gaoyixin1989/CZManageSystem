using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.CollaborationCenter.Payment
{
	public class PaymentSplitAccount
	{
		/// <summary>
		/// 帐户ID
		/// <summary>
		public Guid AccountID { get; set;}
		/// <summary>
		/// 拆帐收款人帐户
		/// <summary>
		public string Account { get; set;}

	}
}
