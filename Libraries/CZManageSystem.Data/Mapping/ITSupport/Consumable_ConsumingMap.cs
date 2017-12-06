using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class Consumable_ConsumingMap : EntityTypeConfiguration<Consumable_Consuming>
	{
		public Consumable_ConsumingMap()
		{
			// Primary Key
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
 			 this.ToTable("Consumable_Consuming"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId"); 
			this.Property(t => t.Series).HasColumnName("Series"); 
			this.Property(t => t.ApplyTime).HasColumnName("ApplyTime"); 
			this.Property(t => t.ApplyDpCode).HasColumnName("ApplyDpCode"); 
			this.Property(t => t.Applicant).HasColumnName("Applicant"); 
			this.Property(t => t.Mobile).HasColumnName("Mobile"); 
			this.Property(t => t.Title).HasColumnName("Title"); 
			this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.State).HasColumnName("State");
        }
	 }
}
