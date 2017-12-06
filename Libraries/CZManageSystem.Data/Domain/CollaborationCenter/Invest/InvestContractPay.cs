using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 合同已付金额表
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.Invest
{
    //合同已付款导入查询条件
    public class InvestContractPayQueryBuilder
    {
        public string ProjectID { get; set; }//项目编号
        public string ProjectName { get; set; }//项目名称
        public string ContractID { get; set; }//合同编号
        public string ContractName { get; set; }//合同名称
        public string RowNote { get; set; }//行说明(唯一）
        public string Batch { get; set; }//批
        public string DateAccount { get; set; }//日记帐分录
        public string Supply { get; set; }//供应商
        public decimal? Pay_start { get; set; }//已付款金额
        public decimal? Pay_end { get; set; }//已付款金额
    }

    public class InvestContractPay
	{
		public Guid ID { get; set;}
		/// <summary>
		/// 批
		/// <summary>
		public string Batch { get; set;}
		/// <summary>
		/// 日记帐分录
		/// <summary>
		public string DateAccount { get; set;}
		/// <summary>
		/// 行说明(唯一）
		/// <summary>
		public string RowNote { get; set;}
		/// <summary>
		/// 项目编号
		/// <summary>
		public string ProjectID { get; set;}
		/// <summary>
		/// 合同编号
		/// <summary>
		public string ContractID { get; set;}
		/// <summary>
		/// 供应商
		/// <summary>
		public string Supply { get; set;}
		/// <summary>
		/// 已付款金额
		/// <summary>
		public Nullable<decimal> Pay { get; set;}
		/// <summary>
		/// 录入时间
		/// <summary>
		public Nullable<DateTime> Time { get; set;}
		/// <summary>
		/// 录入人
		/// <summary>
		public Nullable<Guid> UserID { get; set;}

	}
}
