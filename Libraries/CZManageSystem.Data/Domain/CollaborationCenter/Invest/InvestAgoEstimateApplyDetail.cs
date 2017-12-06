using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 历史项目暂估申请明细表
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.Invest
{
	public class InvestAgoEstimateApplyDetail
	{
		/// <summary>
		/// 编号
		/// <summary>
		public Guid ID { get; set;}
		/// <summary>
		/// 归属申请单id
		/// <summary>
		public Nullable<Guid> ApplyID { get; set;}
		/// <summary>
		/// 年份
		/// <summary>
		public Nullable<int> Year { get; set;}
		/// <summary>
		/// 月份
		/// <summary>
		public Nullable<int> Month { get; set;}
		/// <summary>
		/// 项目名称
		/// <summary>
		public string ProjectName { get; set;}
		/// <summary>
		/// 项目编号
		/// <summary>
		public string ProjectID { get; set;}
		/// <summary>
		/// 合同名称
		/// <summary>
		public string ContractName { get; set;}
		/// <summary>
		/// 合同编号
		/// <summary>
		public string ContractID { get; set;}
		/// <summary>
		/// 供应商
		/// <summary>
		public string Supply { get; set;}
		/// <summary>
		/// 合同总金额
		/// <summary>
		public Nullable<decimal> SignTotal { get; set;}
		/// <summary>
		/// 合同实际金额
		/// <summary>
		public Nullable<decimal> PayTotal { get; set;}
		/// <summary>
		/// 所属专业
		/// <summary>
		public string Study { get; set;}
		/// <summary>
		/// 负责人ID
		/// <summary>
		public Nullable<Guid> ManagerID { get; set;}
		/// <summary>
		/// 科目
		/// <summary>
		public string Course { get; set;}
		/// <summary>
		/// 上个月进度
		/// <summary>
		public Nullable<decimal> BackRate { get; set;}
		/// <summary>
		/// 项目形象进度
		/// <summary>
		public Nullable<decimal> Rate { get; set;}
		/// <summary>
		/// 已付金额
		/// <summary>
		public Nullable<decimal> Pay { get; set;}
		/// <summary>
		/// 暂估金额
		/// <summary>
		public Nullable<decimal> NotPay { get; set;}
		/// <summary>
		/// 暂估人员ID
		/// <summary>
		public Nullable<Guid> EstimateUserID { get; set;}

	}
}
