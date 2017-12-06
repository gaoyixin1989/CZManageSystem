using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.CollaborationCenter.Payment
{
	public class V_PaymentSplitMoney
	{
		public Guid SplitID { get; set;}
		public Nullable<Guid> PayMoneyID { get; set;}
		public Nullable<decimal> SplitMoney { get; set;}
		public string Purpose { get; set;}
		public Nullable<Guid> ApplyID { get; set;}
		public string PayerAccount { get; set;}
		public string PayerName { get; set;}
		public string PayerBranchCode { get; set;}
		public string PayeeAccount { get; set;}
		public string PayeeName { get; set;}
		public string PayeeBranch { get; set;}
		public string PayeeBranchCode { get; set;}
		public string PayeeOpenBank { get; set;}
		public string PayeeBank { get; set;}
		public string PayeeBankCode { get; set;}
		public string PayeeAddressCode { get; set;}
		public string MoneyType { get; set;}
		public string PayeeAreaCode { get; set;}

	}
}
