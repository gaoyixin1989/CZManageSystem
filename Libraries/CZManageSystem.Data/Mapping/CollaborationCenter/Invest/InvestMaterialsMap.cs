using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 物资采购
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Invest
{
	public class InvestMaterialsMap : EntityTypeConfiguration<InvestMaterials>
	{
		public InvestMaterialsMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 项目编号
		/// <summary>
			  this.Property(t => t.ProjectID)

			.HasMaxLength(50);
		/// <summary>
		/// 项目名称
		/// <summary>
			  this.Property(t => t.ProjectName)

			.HasMaxLength(1000);
		/// <summary>
		/// 订单编号
		/// <summary>
			  this.Property(t => t.OrderID)

			.HasMaxLength(50);
		/// <summary>
		/// 订单说明
		/// <summary>
			  this.Property(t => t.OrderDesc)

			.HasMaxLength(2147483647);
		/// <summary>
		/// 订单录入公司
		/// <summary>
			  this.Property(t => t.OrderInCompany)

			.HasMaxLength(1000);
		/// <summary>
		/// 审核状态(批准)
		/// <summary>
			  this.Property(t => t.AuditStatus)

			.HasMaxLength(50);
		/// <summary>
		/// 订单接收公司
		/// <summary>
			  this.Property(t => t.OrderOutCompany)

			.HasMaxLength(1000);
		/// <summary>
		/// 合同编号
		/// <summary>
			  this.Property(t => t.ContractID)

			.HasMaxLength(100);
		/// <summary>
		/// 合同名称
		/// <summary>
			  this.Property(t => t.ContractName)

			.HasMaxLength(1000);
		/// <summary>
		/// 外围系统合同编号
		/// <summary>
			  this.Property(t => t.OutContractID)

			.HasMaxLength(100);
		/// <summary>
		/// 订单标题
		/// <summary>
			  this.Property(t => t.OrderTitle)

			.HasMaxLength(1000);
		/// <summary>
		/// 订单备注
		/// <summary>
			  this.Property(t => t.OrderNote)

			.HasMaxLength(2147483647);
		/// <summary>
		/// 供应商
		/// <summary>
			  this.Property(t => t.Apply)

			.HasMaxLength(1000);
			// Table & Column Mappings
 			 this.ToTable("InvestMaterials"); 
			// 唯一键
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 项目编号
			this.Property(t => t.ProjectID).HasColumnName("ProjectID"); 
			// 项目名称
			this.Property(t => t.ProjectName).HasColumnName("ProjectName"); 
			// 订单编号
			this.Property(t => t.OrderID).HasColumnName("OrderID"); 
			// 订单说明
			this.Property(t => t.OrderDesc).HasColumnName("OrderDesc"); 
			// 订单录入公司
			this.Property(t => t.OrderInCompany).HasColumnName("OrderInCompany"); 
			// 审核状态(批准)
			this.Property(t => t.AuditStatus).HasColumnName("AuditStatus"); 
			// 订单录入金额
			this.Property(t => t.OrderInPay).HasColumnName("OrderInPay"); 
			// 订单接收公司
			this.Property(t => t.OrderOutCompany).HasColumnName("OrderOutCompany"); 
			// 订单接收金额
			this.Property(t => t.OrderOutSum).HasColumnName("OrderOutSum"); 
			// 订单创建时间
			this.Property(t => t.OrderCreateTime).HasColumnName("OrderCreateTime"); 
			// 合同编号
			this.Property(t => t.ContractID).HasColumnName("ContractID"); 
			// 合同名称
			this.Property(t => t.ContractName).HasColumnName("ContractName"); 
			// 外围系统合同编号
			this.Property(t => t.OutContractID).HasColumnName("OutContractID"); 
			// 订单标题
			this.Property(t => t.OrderTitle).HasColumnName("OrderTitle"); 
			// 订单备注
			this.Property(t => t.OrderNote).HasColumnName("OrderNote"); 
			// 供应商
			this.Property(t => t.Apply).HasColumnName("Apply"); 
			// 订单接收百分比 SUM
			this.Property(t => t.OrderOutRate).HasColumnName("OrderOutRate"); 
			// 未接收设备（元）
			this.Property(t => t.OrderUnReceived).HasColumnName("OrderUnReceived"); 
		 }
	 }
}
