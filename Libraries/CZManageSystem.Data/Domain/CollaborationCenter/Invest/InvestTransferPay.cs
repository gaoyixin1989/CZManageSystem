using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 已转资合同表
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.Invest
{
	public class InvestTransferPay
	{
		public Guid ID { get; set;}
		/// <summary>
		/// 项目ID
		/// <summary>
		public string ProjectID { get; set;}
		/// <summary>
		/// 合同ID
		/// <summary>
		public string ContractID { get; set;}
		/// <summary>
		/// 是否已转资
		/// <summary>
		public string IsTransfer { get; set;}
		/// <summary>
		/// 转资金额
		/// <summary>
		public Nullable<decimal> TransferPay { get; set;}
		/// <summary>
		/// 编辑时间
		/// <summary>
		public Nullable<DateTime> EditTime { get; set;}
		/// <summary>
		/// 编辑者ID
		/// <summary>
		public Nullable<Guid> EditorID { get; set;}

	}
}
