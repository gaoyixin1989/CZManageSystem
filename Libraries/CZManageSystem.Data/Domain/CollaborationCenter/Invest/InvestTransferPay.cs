using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// ��ת�ʺ�ͬ��
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.Invest
{
	public class InvestTransferPay
	{
		public Guid ID { get; set;}
		/// <summary>
		/// ��ĿID
		/// <summary>
		public string ProjectID { get; set;}
		/// <summary>
		/// ��ͬID
		/// <summary>
		public string ContractID { get; set;}
		/// <summary>
		/// �Ƿ���ת��
		/// <summary>
		public string IsTransfer { get; set;}
		/// <summary>
		/// ת�ʽ��
		/// <summary>
		public Nullable<decimal> TransferPay { get; set;}
		/// <summary>
		/// �༭ʱ��
		/// <summary>
		public Nullable<DateTime> EditTime { get; set;}
		/// <summary>
		/// �༭��ID
		/// <summary>
		public Nullable<Guid> EditorID { get; set;}

	}
}
