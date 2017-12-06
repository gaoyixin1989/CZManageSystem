using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Models.Mapping
{
    public class IAMS_PendingJobMap : EntityTypeConfiguration<IAMS_PendingJob>
    {
        public IAMS_PendingJobMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.systemid)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.sysAccount)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.sysPassword)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.JobID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Owner)
                .HasMaxLength(50);

            this.Property(t => t.ParentOwner)
                .HasMaxLength(50);

            this.Property(t => t.JobName)
                .HasMaxLength(500);

            this.Property(t => t.URL)
                .HasMaxLength(500);

            this.Property(t => t.IssuedTime)
                .IsFixedLength()
                .HasMaxLength(19);

            this.Property(t => t.JobType)
                .HasMaxLength(50);

            this.Property(t => t.ActionType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.EntityType)
                .HasMaxLength(50);

            this.Property(t => t.EntityId)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("IAMS_PendingJob");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.systemid).HasColumnName("systemid");
            this.Property(t => t.sysAccount).HasColumnName("sysAccount");
            this.Property(t => t.sysPassword).HasColumnName("sysPassword");
            this.Property(t => t.JobID).HasColumnName("JobID");
            this.Property(t => t.Owner).HasColumnName("Owner");
            this.Property(t => t.ParentOwner).HasColumnName("ParentOwner");
            this.Property(t => t.JobName).HasColumnName("JobName");
            this.Property(t => t.URL).HasColumnName("URL");
            this.Property(t => t.IssuedTime).HasColumnName("IssuedTime");
            this.Property(t => t.JobType).HasColumnName("JobType");
            this.Property(t => t.ActionType).HasColumnName("ActionType");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.ProcessedDT).HasColumnName("ProcessedDT");
            this.Property(t => t.RetriedTimes).HasColumnName("RetriedTimes");
            this.Property(t => t.EntityType).HasColumnName("EntityType");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
        }
    }
}
