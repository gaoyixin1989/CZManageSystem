using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class InvestCorrectApplyMap : EntityTypeConfiguration<InvestCorrectApply>
	{
		public InvestCorrectApplyMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
			  this.Property(t => t.Series)

			.HasMaxLength(50);
			  this.Property(t => t.ApplyDpCode)

			.HasMaxLength(640);
			  this.Property(t => t.Applicant)

			.HasMaxLength(50);
			  this.Property(t => t.Mobile)

			.HasMaxLength(50);

			  this.Property(t => t.Title)

			.HasMaxLength(500);
			  this.Property(t => t.Content)

			.HasMaxLength(2147483647);
			// Table & Column Mappings
 			 this.ToTable("InvestCorrectApply"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			this.Property(t => t.Series).HasColumnName("Series"); 
			this.Property(t => t.ApplyTime).HasColumnName("ApplyTime"); 
			this.Property(t => t.ApplyDpCode).HasColumnName("ApplyDpCode"); 
			this.Property(t => t.Applicant).HasColumnName("Applicant"); 
			this.Property(t => t.Mobile).HasColumnName("Mobile"); 
			this.Property(t => t.State).HasColumnName("State"); 
			this.Property(t => t.Title).HasColumnName("Title"); 
			this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId");
        }
	 }
}
