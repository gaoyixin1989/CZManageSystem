using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.CollaborationCenter.Payment
{
	public class PaymentReportReward
	{
		/// <summary>
		/// �ճ��ID
		/// <summary>
		public Guid RewardID { get; set;}
		/// <summary>
		/// �ϱ�ʱ��
		/// <summary>
		public Nullable<DateTime> ApplyTime { get; set;}
		/// <summary>
		/// ��Ӫ��ID
		/// <summary>
		public Nullable<Guid> HallID { get; set;}
		/// <summary>
		/// �տ��ʺ�
		/// <summary>
		public string PayeeAccount { get; set;}
		/// <summary>
		/// ���
		/// <summary>
		public Nullable<decimal> PayMoney { get; set;}
		/// <summary>
		/// ���
		/// <summary>
		public Nullable<decimal> Reward { get; set;}
		/// <summary>
		/// ��˾ID
		/// <summary>
		public Nullable<Guid> CompanyID { get; set;}

	}
}
