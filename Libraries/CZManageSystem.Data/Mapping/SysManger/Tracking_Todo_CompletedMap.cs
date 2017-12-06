using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class Tracking_Todo_CompletedMap : EntityTypeConfiguration<Tracking_Todo_Completed>
    {
        public Tracking_Todo_CompletedMap()
        {
            // Primary Key
            //this.HasKey(t => new { t.WORKFLOWID, t.WorkflowName, t.ActivityName });
            this.HasKey(t => new { t.WorkflowInstanceId });

            // Properties
            this.Property(t => t.Actor)
                .HasMaxLength(50);

            this.Property(t => t.SheetId)
                .HasMaxLength(100);

            this.Property(t => t.Title)
                .HasMaxLength(200);

            this.Property(t => t.WorkflowName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.WorkflowAlias)
                .HasMaxLength(50);

            this.Property(t => t.AliasImage)
                .HasMaxLength(255);

            this.Property(t => t.Creator)
                .HasMaxLength(50);
            this.Property(t => t.ExternalEntityType)
              .HasMaxLength(50);
            this.Property(t => t.ExternalEntityId)
              .HasMaxLength(50);

            this.Property(t => t.CreatorName)
                .HasMaxLength(50);

            this.Property(t => t.ActivityName)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.CurrentActivityNames)
                .HasMaxLength(1000);

            this.Property(t => t.CurrentActors)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("vw_bwwf_Tracking_Todo_Completed");
            this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId");
            this.Property(t => t.startedtime).HasColumnName("startedtime");
            this.Property(t => t.finishedtime).HasColumnName("finishedtime");
            this.Property(t => t.Actor).HasColumnName("Actor");
            this.Property(t => t.SheetId).HasColumnName("SheetId");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Urgency).HasColumnName("Urgency");
            this.Property(t => t.WORKFLOWID).HasColumnName("WORKFLOWID");
            this.Property(t => t.WorkflowName).HasColumnName("WorkflowName");
            this.Property(t => t.WorkflowAlias).HasColumnName("WorkflowAlias");
            this.Property(t => t.AliasImage).HasColumnName("AliasImage");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.state).HasColumnName("state");
            this.Property(t => t.CreatorName).HasColumnName("CreatorName");
            this.Property(t => t.ActivityName).HasColumnName("ActivityName");
            this.Property(t => t.CurrentActivityNames).HasColumnName("CurrentActivityNames");
            this.Property(t => t.CurrentActors).HasColumnName("CurrentActors");
            this.Property(t => t.TrackingType).HasColumnName("TrackingType");
            this.Property(t => t.ExternalEntityId).HasColumnName("ExternalEntityId");
            this.Property(t => t.ExternalEntityType).HasColumnName("ExternalEntityType");
        }
    }
}
