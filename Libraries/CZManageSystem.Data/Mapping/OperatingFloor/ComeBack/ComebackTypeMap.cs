using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class ComebackTypeMap : EntityTypeConfiguration<ComebackType>
	{
		public ComebackTypeMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
			  this.Property(t => t.BudgetDept)

			.HasMaxLength(100);
			  this.Property(t => t.Remark)

			.HasMaxLength(1073741823);
			// Table & Column Mappings
 			 this.ToTable("ComebackType"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			this.Property(t => t.PID).HasColumnName("PID"); 
			this.Property(t => t.BudgetDept).HasColumnName("BudgetDept"); 
			this.Property(t => t.Amount).HasColumnName("Amount"); 
			this.Property(t => t.Remark).HasColumnName("Remark");
            this.HasOptional(t => t.ComebackSource).WithMany().HasForeignKey(d => d.PID);
            // Relationships
            this.HasOptional (t => t.ComebackSource )
                .WithMany(t => t.ComebackTypes )
                .HasForeignKey(d => d.PID );
             
        }
	 }
}
