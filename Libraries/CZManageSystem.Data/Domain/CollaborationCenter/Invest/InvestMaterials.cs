using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// 物资采购
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.Invest
{
	public class InvestMaterials
	{
		/// <summary>
		/// 唯一键
		/// <summary>
		public Guid ID { get; set;}
        /// <summary>
        /// 项目编号
        /// <summary>
         [Required(ErrorMessage = "项目编号不能为空")]
        public string ProjectID { get; set;}
        /// <summary>
        /// 项目名称
        /// <summary>
        [Required(ErrorMessage = "项目名称不能为空")]
        public string ProjectName { get; set;}
        /// <summary>
        /// 订单编号
        /// <summary>
        [Required(ErrorMessage = "订单编号不能为空")]
        public string OrderID { get; set;}
		/// <summary>
		/// 订单说明
		/// <summary>
		public string OrderDesc { get; set;}
		/// <summary>
		/// 订单录入公司
		/// <summary>
		public string OrderInCompany { get; set;}
		/// <summary>
		/// 审核状态(批准)
		/// <summary>
		public string AuditStatus { get; set;}
        /// <summary>
        /// 订单录入金额
        /// <summary>
        [Required(ErrorMessage = "订单录入金额不能为空")]
        public Nullable<decimal> OrderInPay { get; set;}
		/// <summary>
		/// 订单接收公司
		/// <summary>
		public string OrderOutCompany { get; set;}
        /// <summary>
        /// 订单接收金额
        /// <summary>
        [Required(ErrorMessage = "订单接收金额不能为空")]
        public Nullable<decimal> OrderOutSum { get; set;}
		/// <summary>
		/// 订单创建时间
		/// <summary>
		public Nullable<DateTime> OrderCreateTime { get; set;}
		/// <summary>
		/// 合同编号
		/// <summary>
		public string ContractID { get; set;}
		/// <summary>
		/// 合同名称
		/// <summary>
		public string ContractName { get; set;}
		/// <summary>
		/// 外围系统合同编号
		/// <summary>
		public string OutContractID { get; set;}
		/// <summary>
		/// 订单标题
		/// <summary>
		public string OrderTitle { get; set;}
		/// <summary>
		/// 订单备注
		/// <summary>
		public string OrderNote { get; set;}
		/// <summary>
		/// 供应商
		/// <summary>
		public string Apply { get; set;}
		/// <summary>
		/// 订单接收百分比 SUM
		/// <summary>
		public Nullable<decimal> OrderOutRate { get; set;}
		/// <summary>
		/// 未接收设备（元）
		/// <summary>
		public Nullable<decimal> OrderUnReceived { get; set;}

	}
}
