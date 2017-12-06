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
		/// Ӫ����������
		/// <summary>
			  this.Property(t => t.Coding)

			.HasMaxLength(64);
		/// <summary>
		/// Ӫ����������
		/// <summary>
			  this.Property(t => t.Name)

			.HasMaxLength(100);
		/// <summary>
		/// ����
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
			// Ӫ����������
			this.Property(t => t.Coding).HasColumnName("Coding"); 
			// Ӫ����������
			this.Property(t => t.Name).HasColumnName("Name"); 
			// ����
			this.Property(t => t.Department).HasColumnName("Department"); 
			this.Property(t => t.Creator).HasColumnName("Creator"); 
			this.Property(t => t.Creattime).HasColumnName("Creattime"); 
			this.Property(t => t.Remark).HasColumnName("Remark"); 
		 }
	 }
}
