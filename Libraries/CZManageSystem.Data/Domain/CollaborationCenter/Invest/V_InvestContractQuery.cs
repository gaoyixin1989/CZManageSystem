using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 合同查询
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.Invest
{
    public class V_InvestContractQuery
    {
        /// <summary>
        /// 
        /// </summary>
		public Guid ID { get; set; }
        /// <summary>
        /// 导入时间
        /// </summary>
        public Nullable<DateTime> ImportTime { get; set; }
        /// <summary>
        /// 项目编号
        /// </summary>
        public string ProjectID { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 合同编号
        /// </summary>
        public string ContractID { get; set; }
        /// <summary>
        /// 合同名称
        /// </summary>
        public string ContractName { get; set; }
        /// <summary>
        /// 签订时间
        /// </summary>
        public string SignTime { get; set; }
        /// <summary>
        /// 合同主办部门
        /// </summary>
        public string DpCode { get; set; }
        /// <summary>
        /// 主办人
        /// </summary>
        public Nullable<Guid> UserID { get; set; }
        /// <summary>
        /// 合同项目金额、合同不含税金额(元)
        /// </summary>
        public Nullable<decimal> SignTotal { get; set; }
        /// <summary>
        /// 实际合同金额
        /// </summary>
        public Nullable<decimal> PayTotal { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 项目经理ID
        /// </summary>
        public Nullable<Guid> ManagerID { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public string Supply { get; set; }
        /// <summary>
        /// 合同总金额
        /// </summary>
        public Nullable<decimal> AllTotal { get; set; }
        /// <summary>
        /// 是否MIS单类
        /// </summary>
        public string IsMIS { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public string IsDel { get; set; }
        /// <summary>
        /// 合同状态
        /// </summary>
        public string ContractState { get; set; }
        /// <summary>
        /// 合同属性
        /// </summary>
        public string Attribute { get; set; }
        /// <summary>
        /// 项目金额
        /// </summary>
        public Nullable<decimal> ProjectTotal { get; set; }
        /// <summary>
        /// 审批开始时间
        /// </summary>
        public Nullable<DateTime> ApproveStartTime { get; set; }
        /// <summary>
        /// 审批结束时间
        /// </summary>
        public Nullable<DateTime> ApproveEndTime { get; set; }
        /// <summary>
        /// 合同档案号
        /// </summary>
        public string ContractFilesNum { get; set; }
        /// <summary>
        /// 印花税率
        /// </summary>
        public string StampTaxrate { get; set; }
        /// <summary>
        /// 印花税金
        /// </summary>
        public string Stamptax { get; set; }
        /// <summary>
        /// 合同对方
        /// </summary>
        public string ContractOpposition { get; set; }
        /// <summary>
        /// 需求部门
        /// </summary>
        public string RequestDp { get; set; }
        /// <summary>
        /// 相关部门
        /// </summary>
        public string RelevantDp { get; set; }
        /// <summary>
        /// 项目开展原因
        /// </summary>
        public string ProjectCause { get; set; }
        /// <summary>
        /// 合同类型
        /// </summary>
        public string ContractType { get; set; }
        /// <summary>
        /// 合同类型
        /// </summary>
        public string ContractOppositionFrom { get; set; }
        /// <summary>
        /// 合同对方选择方式
        /// </summary>
        public string ContractOppositionType { get; set; }
        /// <summary>
        /// 采购方式
        /// </summary>
        public string Purchase { get; set; }
        /// <summary>
        /// 付款方式
        /// </summary>
        public string PayType { get; set; }
        /// <summary>
        /// 付款说明
        /// </summary>
        public string PayRemark { get; set; }
        /// <summary>
        /// 合同有效区间起始
        /// </summary>
        public Nullable<DateTime> ContractStartTime { get; set; }
        /// <summary>
        /// 合同有效区间终止
        /// </summary>
        public Nullable<DateTime> ContractEndTime { get; set; }
        /// <summary>
        /// 框架合同
        /// </summary>
        public string IsFrameContract { get; set; }
        /// <summary>
        /// 起草时间
        /// </summary>
        public Nullable<DateTime> DraftTime { get; set; }
        /// <summary>
        /// 已签署项目总额
        /// </summary>
        public Nullable<decimal> ProjectAllTotal { get; set; }
        /// <summary>
        /// MIS接收金额
        /// </summary>
        public Nullable<decimal> MISMoney { get; set; }
        /// <summary>
        /// 已付款金额
        /// </summary>
        public Nullable<decimal> Pay { get; set; }
        /// <summary>
        /// 暂估金额
        /// </summary>
        public Nullable<decimal> NotPay { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Nullable<decimal> MustPay { get; set; }

    }
}
