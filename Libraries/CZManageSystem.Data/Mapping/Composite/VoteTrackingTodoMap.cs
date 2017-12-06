using CZManageSystem.Data.Domain.Composite;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Composite
{
    public class VoteTrackingTodoMap : EntityTypeConfiguration<VoteTrackingTodo>
    {
        public VoteTrackingTodoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ActivityInstanceId, t.UserName });
            this.Property(t => t.ProxyName)

          .HasMaxLength(50);
            this.Property(t => t.ActorName)

          .HasMaxLength(50);
            this.Property(t => t.Title)

          .HasMaxLength(200);
            this.Property(t => t.CreatorName)

          .HasMaxLength(50);
            this.Property(t => t.SheetId)

          .HasMaxLength(20);
            this.Property(t => t.Creator)

          .HasMaxLength(50);
            this.Property(t => t.ExternalEntityType).HasMaxLength(50);
            this.Property(t => t.ExternalEntityId)

       .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("VoteTrackingTodo");
            this.Property(t => t.ActivityInstanceId).HasColumnName("ActivityInstanceId");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.ProxyName).HasColumnName("ProxyName");
            this.Property(t => t.OperateType).HasColumnName("OperateType");
            this.Property(t => t.ActorName).HasColumnName("ActorName");
            this.Property(t => t.IsCompleted).HasColumnName("IsCompleted");
            this.Property(t => t.CreatedTime).HasColumnName("CreatedTime");
            this.Property(t => t.StartedTime).HasColumnName("StartedTime");
            this.Property(t => t.FinishedTime).HasColumnName("FinishedTime"); 
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.CreatorName).HasColumnName("CreatorName");
            this.Property(t => t.SheetId).HasColumnName("SheetId");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.ExternalEntityId).HasColumnName("ExternalEntityId");
            this.Property(t => t.ExternalEntityType).HasColumnName("ExternalEntityType");
        }
    }
}
