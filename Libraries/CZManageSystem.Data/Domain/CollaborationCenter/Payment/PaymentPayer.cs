using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.CollaborationCenter.Payment
{
	public class PaymentPayer
	{
		public Guid PayerID { get; set;}
		/// <summary>
		/// 付款人账户
		/// <summary>
		public string Account { get; set;}
		/// <summary>
		/// 付款人账户名称
		/// <summary>
		public string Name { get; set;}
		/// <summary>
		/// 付款人分行代码
		/// <summary>
		public string BranchCode { get; set;}
		/// <summary>
		/// 付款公司ID
		/// <summary>
		public Nullable<Guid> CompanyID { get; set;}

	}
}
