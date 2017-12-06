using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 历史项目暂估修改日志
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.Invest
{
	public class InvestAgoEstimateLog
	{
		/// <summary>
		/// 编号
		/// <summary>
		public int ID { get; set;}
		/// <summary>
		/// 项目id
		/// <summary>
		public string ProjectID { get; set;}
		/// <summary>
		/// 合同id
		/// <summary>
		public string ContractID { get; set;}
		/// <summary>
		/// 操作：单个修改、批量修改、导入修改、导入插入、删除
		/// <summary>
		public string OpType { get; set;}
		/// <summary>
		/// 操作人
		/// <summary>
		public string OpName { get; set;}
		/// <summary>
		/// 修改时间
		/// <summary>
		public Nullable<DateTime> OpTime { get; set;}
		/// <summary>
		/// 修改前合同实际金额
		/// <summary>
		public Nullable<decimal> BfPayTotal { get; set;}
		/// <summary>
		/// 修改后合同实际金额
		/// <summary>
		public Nullable<decimal> PayTotal { get; set;}
		/// <summary>
		/// 修改前项目形象进度
		/// <summary>
		public Nullable<decimal> BfRate { get; set;}
		/// <summary>
		/// 修改后项目形象进度
		/// <summary>
		public Nullable<decimal> Rate { get; set;}
		/// <summary>
		/// 修改前已付金额
		/// <summary>
		public Nullable<decimal> BfPay { get; set;}
		/// <summary>
		/// 修改后已付金额
		/// <summary>
		public Nullable<decimal> Pay { get; set;}
		/// <summary>
		/// 修改前暂估金额
		/// <summary>
		public Nullable<decimal> BfNotPay { get; set;}
		/// <summary>
		/// 修改后暂估金额
		/// <summary>
		public Nullable<decimal> NotPay { get; set;}

	}
}
