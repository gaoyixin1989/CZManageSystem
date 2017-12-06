using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 项目查询
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.Invest
{
    //项目信息查询条件
    public class VInvestProjectQueryQueryBuilder
    {
        public int? Year { get; set; }//下达年份
        public string TaskID { get; set; }//计划任务书文号
        public string ProjectID { get; set; }//项目编号
        public string ProjectName { get; set; }//项目名称
        public decimal? Total_start { get; set; }//项目总投资
        public decimal? Total_end { get; set; }//项目总投资
        public decimal? YearTotal_start { get; set; }//年度项目投资
        public decimal? YearTotal_end { get; set; }//年度项目投资
        public string BeginEnd { get; set; }//起止年限
        public string Content { get; set; }//年度建设内容
        public string FinishDate { get; set; }//要求完成时限
        public string DpCode_Text { get; set; }//负责专业室
        public string UserID_Text { get; set; }//室负责人
        public string ManagerID_Text { get; set; }//项目经理

    }

    public class V_InvestProjectQuery
    {
        /// <summary>
        /// 下达年份,导入年份
        /// </summary>
		public Nullable<int> Year { get; set; }
        /// <summary>
        /// 计划任务书文号
        /// </summary>
		public string TaskID { get; set; }
        /// <summary>
        /// 项目编号
        /// </summary>
		public string ProjectID { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
		public string ProjectName { get; set; }
        /// <summary>
        /// 起止年限
        /// </summary>
		public string BeginEnd { get; set; }
        /// <summary>
        /// 项目总投资
        /// </summary>
		public Nullable<decimal> Total { get; set; }
        /// <summary>
        /// 年度项目投资
        /// </summary>
		public Nullable<decimal> YearTotal { get; set; }
        /// <summary>
        /// 年度建设内容
        /// </summary>
		public string Content { get; set; }
        /// <summary>
        /// 要求完成时限
        /// </summary>
		public string FinishDate { get; set; }
        /// <summary>
        /// 负责专业室
        /// </summary>
		public string DpCode { get; set; }
        /// <summary>
        /// 室负责人
        /// </summary>
		public Nullable<Guid> UserID { get; set; }
        /// <summary>
        /// 项目经理
        /// </summary>
		public Nullable<Guid> ManagerID { get; set; }
        /// <summary>
        /// 合同项目金额、合同不含税金额(元)
        /// </summary>
		public Nullable<decimal> SignTotal { get; set; }
        /// <summary>
        /// 暂估金额
        /// </summary>
		public Nullable<decimal> NotPay { get; set; }
        /// <summary>
        /// 已付金额
        /// </summary>
		public Nullable<decimal> Pay { get; set; }
        /// <summary>
        /// mis金额
        /// </summary>
		public Nullable<decimal> MISMoney { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public Nullable<decimal> MustPay { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public Nullable<decimal> ProjectRate { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public Nullable<decimal> TransferRate { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public Nullable<decimal> BackYearTotal { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public Nullable<decimal> YearMustPay { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public Nullable<decimal> YearRate { get; set; }

    }
}
