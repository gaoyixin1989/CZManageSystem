using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.CollaborationCenter.Payment
{
	public class PaymentSplitMoney
	{
		/// <summary>
		/// ����ID
		/// <summary>
		public Guid SplitID { get; set;}
		/// <summary>
		/// �ܽ��ID
		/// <summary>
		public Nullable<Guid> PayMoneyID { get; set;}
		/// <summary>
		/// ���ʽ��ID
		/// <summary>
		public Nullable<decimal> SplitMoney { get; set;}
		/// <summary>
		/// ��;
		/// <summary>
		public string Purpose { get; set;}

	}
}
