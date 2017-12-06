using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class Tracking_WorkflowMap : EntityTypeConfiguration<Tracking_Workflow>
    {
        public Tracking_WorkflowMap()
        {
            // Primary Key
            this.HasKey(t => t.WorkflowInstanceId);

            // Propertiess
            this.Property(t => t.WorkflowId).IsRequired();
            this.Property(t => t.SheetId).IsRequired().HasMaxLength(100);
            this.Property(t => t.State).IsRequired();
            this.Property(t => t.Creator).IsRequired().HasMaxLength(50);
            this.Property(t => t.StartedTime).IsRequired();
            this.Property(t => t.Title).IsRequired().HasMaxLength(200);
            this.Property(t => t.Secrecy).IsRequired();
            this.Property(t => t.Requirement).IsRequired().HasMaxLength(1000);

            this.ToTable("bwwf_Tracking_Workflows");
            this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId");
            this.Property(t => t.WorkflowId).HasColumnName("WorkflowId");
            this.Property(t => t.SheetId).HasColumnName("SheetId");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.StartedTime).HasColumnName("StartedTime");
            this.Property(t => t.FinishedTime).HasColumnName("FinishedTime");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Secrecy).HasColumnName("Secrecy");
            this.Property(t => t.Urgency).HasColumnName("Urgency");
            this.Property(t => t.Importance).HasColumnName("Importance");
            this.Property(t => t.ExpectFinishedTime).HasColumnName("ExpectFinishedTime");
            this.Property(t => t.Requirement).HasColumnName("Requirement");
            this.Property(t => t.CommentCount).HasColumnName("CommentCount");
            this.Property(t => t.PrintCount).HasColumnName("PrintCount");
        }
    }
}
