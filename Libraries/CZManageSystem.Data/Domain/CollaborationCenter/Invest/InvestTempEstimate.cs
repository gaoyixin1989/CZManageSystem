using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.CollaborationCenter.Invest;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class InvestTempEstimate
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
		/// 供应商
		/// <summary>
		public string Supply { get; set;}
		/// <summary>
		/// 合同金额
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
		/// 是否锁定
		/// <summary>
		public string IsLock { get; set;}
		/// <summary>
		/// 暂估人员ID
		/// <summary>
		public Nullable<Guid> EstimateUserID { get; set;}
		/// <summary>
		/// 当前状态
		/// <summary>
		public string Status { get; set;}
		/// <summary>
		/// 状态操作时间
		/// <summary>
		public Nullable<DateTime> StatusTime { get; set;}

        public virtual Users ManagerObj { get; set; }
      

    }

    //已终止 暂估
    public class StopInvestTempEstimateQueryBuilder
    {
        public DateTime? StatusTime_Start { get; set; }//终止时间
        public DateTime? StatusTime_End { get; set; }
        public string EstimateUserName { get; set; }//暂估人       
        public string ProjectID { get; set; }//项目编号
        public string ProjectName { get; set; }//项目名称
        public string ContractID { get; set; }//合同编号
        public string ContractName { get; set; }//合同名称
        public string Status { get; set; }

    }
}
