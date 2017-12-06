using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.CollaborationCenter.Payment
{
	public class PaymentPayMoney
	{
		/// <summary>
		/// 编号
		/// <summary>
		public Guid ID { get; set;}
		/// <summary>
		/// 工单ID
		/// <summary>
		public Nullable<Guid> ApplyID { get; set;}
		/// <summary>
		/// 付款人帐号
		/// <summary>
		public string PayerAccount { get; set;}
		/// <summary>
		/// 付款人名称
		/// <summary>
		public string PayerName { get; set;}
		/// <summary>
		/// 付款人分行代码
		/// <summary>
		public string PayerBranchCode { get; set;}
		/// <summary>
		/// 收款人帐号
		/// <summary>
		public string PayeeAccount { get; set;}
		/// <summary>
		/// 收款人帐号名称
		/// <summary>
		public string PayeeName { get; set;}
		/// <summary>
		/// 收款人所属分行
		/// <summary>
		public string PayeeBranch { get; set;}
		/// <summary>
		/// 收款人分行代码
		/// <summary>
		public string PayeeBranchCode { get; set;}
		/// <summary>
		/// 收款人开户行
		/// <summary>
		public string PayeeOpenBank { get; set;}
		/// <summary>
		/// 收款人所属银行
		/// <summary>
		public string PayeeBank { get; set;}
		/// <summary>
		/// 收款人银行代码
		/// <summary>
		public string PayeeBankCode { get; set;}
		/// <summary>
		/// 收款人属地代码
		/// <summary>
		public string PayeeAddressCode { get; set;}
		/// <summary>
		/// 金额
		/// <summary>
		public Nullable<decimal> Money { get; set;}
		/// <summary>
		/// 币别
		/// <summary>
		public string MoneyType { get; set;}
		/// <summary>
		/// 目的
		/// <summary>
		public string Purpose { get; set;}
		/// <summary>
		/// 收款人区域代码
		/// <summary>
		public string PayeeAreaCode { get; set;}

	}
}
