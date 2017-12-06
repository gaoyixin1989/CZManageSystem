using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.CollaborationCenter.Payment
{
	public class PaymentUser
	{
		/// <summary>
		/// ÓÃ»§ID
		/// <summary>
		public Guid UserID { get; set;}
		public string LoginID { get; set;}
		public string PassWord { get; set;}
		public string UserName { get; set;}
		public string Mobile { get; set;}
		public string Phone { get; set;}
		public Nullable<Guid> CompanyID { get; set;}
		public string Status { get; set;}

	}
}
