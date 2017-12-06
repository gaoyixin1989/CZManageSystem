using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 已转资合同表
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Invest
{
	public class InvestTransferPayMap : EntityTypeConfiguration<InvestTransferPay>
	{
		public InvestTransferPayMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 项目ID
		/// <summary>
			  this.Property(t => t.ProjectID)

			.HasMaxLength(500);
		/// <summary>
		/// 合同ID
		/// <summary>
			  this.Property(t => t.ContractID)

			.HasMaxLength(100);
		/// <summary>
		/// 是否已转资
		/// <summary>
			  this.Property(t => t.IsTransfer)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("InvestTransferPay"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 项目ID
			this.Property(t => t.ProjectID).HasColumnName("ProjectID"); 
			// 合同ID
			this.Property(t => t.ContractID).HasColumnName("ContractID"); 
			// 是否已转资
			this.Property(t => t.IsTransfer).HasColumnName("IsTransfer"); 
			// 转资金额
			this.Property(t => t.TransferPay).HasColumnName("TransferPay"); 
			// 编辑时间
			this.Property(t => t.EditTime).HasColumnName("EditTime"); 
			// 编辑者ID
			this.Property(t => t.EditorID).HasColumnName("EditorID"); 
		 }
	 }
}
