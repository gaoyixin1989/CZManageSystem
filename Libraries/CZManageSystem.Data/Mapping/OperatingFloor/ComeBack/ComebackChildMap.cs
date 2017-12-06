using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class ComebackChildMap : EntityTypeConfiguration<ComebackChild>
	{
		public ComebackChildMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
			  this.Property(t => t.Name)

			.HasMaxLength(50);
			  this.Property(t => t.Remark)

			.HasMaxLength(500);
			// Table & Column Mappings
 			 this.ToTable("ComebackChild"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			this.Property(t => t.PID).HasColumnName("PID"); 
			this.Property(t => t.Year).HasColumnName("Year"); 
			this.Property(t => t.Name).HasColumnName("Name"); 
			this.Property(t => t.Amount).HasColumnName("Amount"); 
			this.Property(t => t.Remark).HasColumnName("Remark");
            this.HasOptional(t => t.ComebackType).WithMany().HasForeignKey(d => d.PID);
            this.HasOptional(t => t.ComebackType )
               .WithMany(t => t.ComebackChilds )
               .HasForeignKey(d => d.PID);
        }
	 }
}
