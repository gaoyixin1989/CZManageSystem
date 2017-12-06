using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 历史项目暂估修改日志
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Invest
{
	public class InvestAgoEstimateLogMap : EntityTypeConfiguration<InvestAgoEstimateLog>
	{
		public InvestAgoEstimateLogMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 项目id
		/// <summary>
			  this.Property(t => t.ProjectID)

			.HasMaxLength(500);
		/// <summary>
		/// 合同id
		/// <summary>
			  this.Property(t => t.ContractID)

			.HasMaxLength(100);
		/// <summary>
		/// 操作：单个修改、批量修改、导入修改、导入插入、删除
		/// <summary>
			  this.Property(t => t.OpType)

			.HasMaxLength(50);
		/// <summary>
		/// 操作人
		/// <summary>
			  this.Property(t => t.OpName)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("InvestAgoEstimateLog"); 
			// 编号
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 项目id
			this.Property(t => t.ProjectID).HasColumnName("ProjectID"); 
			// 合同id
			this.Property(t => t.ContractID).HasColumnName("ContractID"); 
			// 操作：单个修改、批量修改、导入修改、导入插入、删除
			this.Property(t => t.OpType).HasColumnName("OpType"); 
			// 操作人
			this.Property(t => t.OpName).HasColumnName("OpName"); 
			// 修改时间
			this.Property(t => t.OpTime).HasColumnName("OpTime"); 
			// 修改前合同实际金额
			this.Property(t => t.BfPayTotal).HasColumnName("BfPayTotal"); 
			// 修改后合同实际金额
			this.Property(t => t.PayTotal).HasColumnName("PayTotal"); 
			// 修改前项目形象进度
			this.Property(t => t.BfRate).HasColumnName("BfRate"); 
			// 修改后项目形象进度
			this.Property(t => t.Rate).HasColumnName("Rate"); 
			// 修改前已付金额
			this.Property(t => t.BfPay).HasColumnName("BfPay"); 
			// 修改后已付金额
			this.Property(t => t.Pay).HasColumnName("Pay"); 
			// 修改前暂估金额
			this.Property(t => t.BfNotPay).HasColumnName("BfNotPay"); 
			// 修改后暂估金额
			this.Property(t => t.NotPay).HasColumnName("NotPay"); 
		 }
	 }
}
