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
		/// �������˻�
		/// <summary>
		public string Account { get; set;}
		/// <summary>
		/// �������˻�����
		/// <summary>
		public string Name { get; set;}
		/// <summary>
		/// �����˷��д���
		/// <summary>
		public string BranchCode { get; set;}
		/// <summary>
		/// ���˾ID
		/// <summary>
		public Nullable<Guid> CompanyID { get; set;}

	}
}
