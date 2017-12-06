using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
	public class Tracking_Activities_CompletedMap : EntityTypeConfiguration<Tracking_Activities_Completed>
	{
		public Tracking_Activities_CompletedMap()
		{
			// Primary Key
			 this.HasKey(t => t.ActivityInstanceId);
			  this.Property(t => t.Actor)

			.HasMaxLength(50);
			  this.Property(t => t.Command)

			.HasMaxLength(50);
			  this.Property(t => t.Reason)

			.HasMaxLength(1024);
			  this.Property(t => t.ExternalEntityType)

			.HasMaxLength(50);
			  this.Property(t => t.ExternalEntityId)

			.HasMaxLength(50);
			  this.Property(t => t.ActorDescription)

			.HasMaxLength(255);
			// Table & Column Mappings
 			 this.ToTable("bwwf_Tracking_Activities_Completed"); 
			this.Property(t => t.ActivityInstanceId).HasColumnName("ActivityInstanceId"); 
			this.Property(t => t.PrevSetId).HasColumnName("PrevSetId"); 
			this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId"); 
			this.Property(t => t.ActivityId).HasColumnName("ActivityId"); 
			this.Property(t => t.IsCompleted).HasColumnName("IsCompleted"); 
			this.Property(t => t.OperateType).HasColumnName("OperateType"); 
			this.Property(t => t.Actor).HasColumnName("Actor"); 
			this.Property(t => t.CreatedTime).HasColumnName("CreatedTime"); 
			this.Property(t => t.FinishedTime).HasColumnName("FinishedTime"); 
			this.Property(t => t.Command).HasColumnName("Command"); 
			this.Property(t => t.Reason).HasColumnName("Reason"); 
			this.Property(t => t.ExternalEntityType).HasColumnName("ExternalEntityType"); 
			this.Property(t => t.ExternalEntityId).HasColumnName("ExternalEntityId"); 
			this.Property(t => t.ActorDescription).HasColumnName("ActorDescription"); 
			this.Property(t => t.PrintCount).HasColumnName("PrintCount");
            
            // Relationships
            this.HasOptional(t => t.Activities).WithMany()
                .HasForeignKey(d => d.ActivityId);

        }
	 }
}
