using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 合同已付金额表
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Invest
{
	public class InvestContractPayMap : EntityTypeConfiguration<InvestContractPay>
	{
		public InvestContractPayMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 批
		/// <summary>
			  this.Property(t => t.Batch)

			.HasMaxLength(500);
		/// <summary>
		/// 日记帐分录
		/// <summary>
			  this.Property(t => t.DateAccount)

			.HasMaxLength(500);
		/// <summary>
		/// 行说明(唯一）
		/// <summary>
			  this.Property(t => t.RowNote)

			.HasMaxLength(500);
		/// <summary>
		/// 项目编号
		/// <summary>
			  this.Property(t => t.ProjectID)

			.HasMaxLength(50);
		/// <summary>
		/// 合同编号
		/// <summary>
			  this.Property(t => t.ContractID)

			.HasMaxLength(100);
		/// <summary>
		/// 供应商
		/// <summary>
			  this.Property(t => t.Supply)

			.HasMaxLength(500);
			// Table & Column Mappings
 			 this.ToTable("InvestContractPay"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 批
			this.Property(t => t.Batch).HasColumnName("Batch"); 
			// 日记帐分录
			this.Property(t => t.DateAccount).HasColumnName("DateAccount"); 
			// 行说明(唯一）
			this.Property(t => t.RowNote).HasColumnName("RowNote"); 
			// 项目编号
			this.Property(t => t.ProjectID).HasColumnName("ProjectID"); 
			// 合同编号
			this.Property(t => t.ContractID).HasColumnName("ContractID"); 
			// 供应商
			this.Property(t => t.Supply).HasColumnName("Supply"); 
			// 已付款金额
			this.Property(t => t.Pay).HasColumnName("Pay"); 
			// 录入时间
			this.Property(t => t.Time).HasColumnName("Time"); 
			// 录入人
			this.Property(t => t.UserID).HasColumnName("UserID"); 
		 }
	 }
}
