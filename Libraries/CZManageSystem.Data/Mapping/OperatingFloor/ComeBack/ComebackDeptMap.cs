using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class ComebackDeptMap : EntityTypeConfiguration<ComebackDept>
	{
		public ComebackDeptMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
			  this.Property(t => t.BudgetDept)

			.HasMaxLength(100);
			  this.Property(t => t.Remark)

			.HasMaxLength(500);
			// Table & Column Mappings
 			 this.ToTable("ComebackDept"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			this.Property(t => t.Year).HasColumnName("Year"); 
			this.Property(t => t.BudgetDept).HasColumnName("BudgetDept"); 
			this.Property(t => t.Amount).HasColumnName("Amount"); 
			this.Property(t => t.Remark).HasColumnName("Remark"); 
		 }
	 }
}
