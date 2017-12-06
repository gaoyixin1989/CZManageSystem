using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.CollaborationCenter.Payment
{
	public class PaymentPayee
	{
		/// <summary>
		/// 收款人帐号ID
		/// <summary>
		public Guid PayeeID { get; set;}
		/// <summary>
		/// 收款人帐号
		/// <summary>
		public string Account { get; set;}
		/// <summary>
		/// 收款人名称
		/// <summary>
		public string Name { get; set;}
		/// <summary>
		/// 收款人所属分行代码
		/// <summary>
		public string BranchCode { get; set;}
		/// <summary>
		/// 开户行
		/// <summary>
		public string Branch { get; set;}
		/// <summary>
		/// 开户行名称
		/// <summary>
		public string OpenBank { get; set;}
		/// <summary>
		/// 所属银行名称
		/// <summary>
		public string Bank { get; set;}
		/// <summary>
		/// 所属银行代码
		/// <summary>
		public string BankCode { get; set;}
		/// <summary>
		/// 属地代码
		/// <summary>
		public string AddressCode { get; set;}
		/// <summary>
		/// 区域代码
		/// <summary>
		public string AreaCode { get; set;}
		/// <summary>
		/// 服营厅ID
		/// <summary>
		public Nullable<Guid> HallID { get; set;}

	}
}
