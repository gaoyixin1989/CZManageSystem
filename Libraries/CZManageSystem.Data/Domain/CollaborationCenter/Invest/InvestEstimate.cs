using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class InvestEstimate
	{
		public Guid ID { get; set;}
		public Nullable<int> Year { get; set;}
		public Nullable<int> Month { get; set;}
		public string ProjectName { get; set;}
		public string ProjectID { get; set;}
		public string ContractName { get; set;}
		public string ContractID { get; set;}
		public string Supply { get; set;}
		public Nullable<decimal> SignTotal { get; set;}
		public Nullable<decimal> PayTotal { get; set;}
		public string Study { get; set;}
		public Nullable<Guid> ManagerID { get; set;}
		public string Course { get; set;}
		public Nullable<decimal> BackRate { get; set;}
		public Nullable<decimal> Rate { get; set;}
		public Nullable<decimal> Pay { get; set;}
		public Nullable<decimal> NotPay { get; set;}
		public Nullable<Guid> EstimateUserID { get; set;}
        public virtual Users ManagerObj { get; set; }
        public virtual Users UserObj { get; set; }

    }
    //暂估查询
    public class InvestEstimateQueryBuilder
    {
        public Nullable<int> Year { get; set; }//年
        public Nullable<int> Month { get; set; }//月
        public string ProjectID { get; set; }//项目编号
        public string ProjectName { get; set; }//项目名称
        public string ContractID { get; set; }//合同编号
        public string ContractName { get; set; }//合同名称
        public string Dpfullname { get; set; }//部门
        public string Supply { get; set; }//供应商
        public string Study { get; set; }//所属专业
        public string Course { get; set; }//科目
        public string ManagerName { get; set; }//负责人
        public Nullable<decimal> SignTotal_start { get; set; }//合同金额
        public Nullable<decimal> SignTotal_end { get; set; }//合同金额
        public Nullable<decimal> PayTotal_start { get; set; }//实际合同金额
        public Nullable<decimal> PayTotal_end { get; set; }//实际合同金额
        public Nullable<decimal> Pay_start { get; set; }//已付款金额
        public Nullable<decimal> Pay_end { get; set; }//已付款金额
        public Nullable<decimal> BackRate_start { get; set; }//上月形象进度
        public Nullable<decimal> BackRate_end { get; set; }//上月形象进度
        public Nullable<decimal> Rate_start { get; set; }//本月形象进度
        public Nullable<decimal> Rate_end { get; set; }//本月形象进度
        public Nullable<decimal> NotPay_start { get; set; }//暂估金额
        public Nullable<decimal> NotPay_end { get; set; }//暂估金额
        public Nullable<decimal> Tax_start { get; set; }//合同税金额
        public Nullable<decimal> Tax_end { get; set; }//合同税金额
    }
}
