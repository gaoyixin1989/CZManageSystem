using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.CollaborationCenter.Payment
{
	public class V_PaymentHall
	{
		public Guid HallID { get; set;}
		public string HallName { get; set;}
		public string DpName { get; set;}
		public string DpFullName { get; set;}

	}
}
