using CZManageSystem.Data.Domain.MarketPlan;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class Ucs_MarketPlanLogMap : EntityTypeConfiguration<Ucs_MarketPlanLog>
	{
		public Ucs_MarketPlanLogMap()
		{
			// Primary Key
			 this.HasKey(t => t.Id);
		/// <summary>
		/// 营销方案编码
		/// <summary>
			  this.Property(t => t.Coding)

			.HasMaxLength(64);
		/// <summary>
		/// 营销方案名称
		/// <summary>
			  this.Property(t => t.Name)

			.HasMaxLength(100);
		/// <summary>
		/// 部门
		/// <summary>
			  this.Property(t => t.Department)

			.HasMaxLength(100);
			  this.Property(t => t.Creator)

			.HasMaxLength(100);

			  this.Property(t => t.Remark)

			.HasMaxLength(100);
			// Table & Column Mappings
 			 this.ToTable("Ucs_MarketPlanLog"); 
			this.Property(t => t.Id).HasColumnName("Id"); 
			// 营销方案编码
			this.Property(t => t.Coding).HasColumnName("Coding"); 
			// 营销方案名称
			this.Property(t => t.Name).HasColumnName("Name"); 
			// 部门
			this.Property(t => t.Department).HasColumnName("Department"); 
			this.Property(t => t.Creator).HasColumnName("Creator"); 
			this.Property(t => t.Creattime).HasColumnName("Creattime"); 
			this.Property(t => t.Remark).HasColumnName("Remark"); 
		 }
	 }
}
