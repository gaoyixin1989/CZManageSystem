using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Models.Mapping
{
    public class WorkflowsMap : EntityTypeConfiguration<Workflows>
    {
        public WorkflowsMap()
        {
            // Primary Key
            this.HasKey(t => t.WorkflowId);

            // Properties
            this.Property(t => t.WorkflowName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Owner)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Creator)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Remark)
                .HasMaxLength(3000);

            this.Property(t => t.LastModifier)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("bwwf_Workflows");
            this.Property(t => t.WorkflowId).HasColumnName("WorkflowId");
            this.Property(t => t.WorkflowName).HasColumnName("WorkflowName");
            this.Property(t => t.Owner).HasColumnName("Owner");
            this.Property(t => t.Enabled).HasColumnName("Enabled");
            this.Property(t => t.IsCurrent).HasColumnName("IsCurrent");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.LastModifier).HasColumnName("LastModifier");
            this.Property(t => t.CreatedTime).HasColumnName("CreatedTime");
            this.Property(t => t.LastModTime).HasColumnName("LastModTime");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
        }
    }
}
