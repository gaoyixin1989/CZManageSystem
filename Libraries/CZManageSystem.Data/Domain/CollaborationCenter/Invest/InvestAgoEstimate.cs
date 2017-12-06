using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

/// <summary>
/// 历史项目暂估
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.Invest
{

    /// <summary>
    /// 历史项目暂估查询
    /// </summary>
    public class AgoEstimateQueryBuilder
    {
        public string Dept_Text { get; set; }//暂估部门
        public decimal? NotPay_start { get; set; }//暂估金额
        public decimal? NotPay_end { get; set; }//暂估金额
        public string ProjectID { get; set; }//项目编号
        public string ProjectName { get; set; }//项目名称
        public string ContractID { get; set; }//合同编号
        public string ContractName { get; set; }//合同名称
        public decimal? SignTotal_start { get; set; }//合同金额
        public decimal? SignTotal_end { get; set; }//合同金额
        public decimal? PayTotal_start { get; set; }//实际合同金额
        public decimal? PayTotal_end { get; set; }//实际合同金额
        public string Supply { get; set; }//供应商
        public string Study { get; set; }//所属专业
        public string Course { get; set; }//科目
        public string ManagerID_Text { get; set; }//负责人
        public decimal? Rate_start { get; set; }//形象进度
        public decimal? Rate_end { get; set; }//形象进度
        public decimal? Pay_start { get; set; }//已付款金额
        public decimal? Pay_end { get; set; }//已付款金额

    }


    public class InvestAgoEstimate
	{
		/// <summary>
		/// 唯一ID
		/// <summary>
		public Guid ID { get; set;}
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
		/// 项目ID
		/// <summary>
		public string ProjectID { get; set;}
		/// <summary>
		/// 合同名称
		/// <summary>
		public string ContractName { get; set;}
		/// <summary>
		/// 合同ID
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


        //外键
        public virtual Users ManagerObj { get; set; }
        public virtual Users EstimateUserObj { get; set; }
    }
}
