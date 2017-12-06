using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.CollaborationCenter.Payment
{
	public class V_PaymentPayer
	{
		public Guid  PayerID { get; set;}
		public string Account { get; set;}
		public string Name { get; set;}
		public string BranchCode { get; set;}
		public Nullable<Guid> CompanyID { get; set;}
		public string CompanyName { get; set;}

	}
}
