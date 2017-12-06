using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class Tracking_TodoMap : EntityTypeConfiguration<Tracking_Todo>
    {
        public Tracking_TodoMap()
        {
            // Primary Key
            this.HasKey(t => t.ActivityInstanceId);

            // Properties
            this.Property(t => t.UserName)
                .IsRequired()
                .HasMaxLength(50);
            this.Property(t => t.State)
               .IsRequired();
            this.Property(t => t.OperateType)
               .IsRequired();
            this.Property(t => t.IsCompleted)
                .IsRequired();
            this.Property(t => t.ActivityName)
                .IsRequired();
            this.Property(t => t.Title)
                .IsRequired();
            this.Property(t => t.WorkflowName)
              .IsRequired();
            this.Property(t => t.WorkflowInstanceId)
              .IsRequired();
            this.Property(t => t.StartedTime)
              .IsRequired();
            this.Property(t => t.Creator)
             .IsRequired();
            this.Property(t => t.TodoActors)
             .IsRequired();
            this.Property(t => t.StartedTime)
             .IsRequired();
            this.Property(t => t.StartedTime)
             .IsRequired();
            this.Property(t => t.ExternalEntityType)
            .HasMaxLength(50);
            this.Property(t => t.ExternalEntityId)
            .HasMaxLength(50);

            this.ToTable("vw_bwwf_Tracking_Todo");
        }
    }
}
