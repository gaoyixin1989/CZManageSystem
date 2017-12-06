using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.CollaborationCenter.Payment
{
	public class V_PaymentUser
	{
		public Guid UserID { get; set;}
		public string LoginID { get; set;}
		public string PassWord { get; set;}
		public string UserName { get; set;}
		public string Mobile { get; set;}
		public string Phone { get; set;}
		public Nullable<Guid> CompanyID { get; set;}
		public string Status { get; set;}
		public Nullable<Guid> DpId { get; set;}
		public string DpCode { get; set;}
		public Nullable<int> DpLv { get; set;}
		public string DpName { get; set;}
		public string DpFullName { get; set;}

	}
}
