using CZManageSystem.Data.Domain.Composite;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.Composite
{
    public class VoteApplyMap : EntityTypeConfiguration<VoteApply>
    {
        public VoteApplyMap()
        {
            // Primary Key
            this.HasKey(t => t.ApplyID);

            // Properties
            this.Property(t => t.ApplyTitle)
                .HasMaxLength(200);

            this.Property(t => t.ApplySn)
                .HasMaxLength(50);
            this.Property(t => t.MobilePhone)
               .HasMaxLength(50);
            this.Property(t => t.Creator)
                .HasMaxLength(50);

            this.Property(t => t.ThemeType)
                .HasMaxLength(100);

            this.Property(t => t.IsNiming)
                .HasMaxLength(50);

            this.Property(t => t.Attids)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("VoteApply");
            this.Property(t => t.ApplyID).HasColumnName("ApplyID");
            this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId");
            this.Property(t => t.ApplyTitle).HasColumnName("ApplyTitle");
            this.Property(t => t.ApplySn).HasColumnName("ApplySn");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.CreatorID).HasColumnName("CreatorID");
            this.Property(t => t.MobilePhone).HasColumnName("MobilePhone");
            this.Property(t => t.ThemeType).HasColumnName("ThemeType");
            this.Property(t => t.ThemeID).HasColumnName("ThemeID");
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            this.Property(t => t.IsNiming).HasColumnName("IsNiming");
            this.Property(t => t.Attids).HasColumnName("Attids");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.MemberName).HasColumnName("MemberName");
            this.Property(t => t.MemberIDs).HasColumnName("MemberIDs");
            this.Property(t => t.MemberType ).HasColumnName("MemberType");
            this.Property(t => t.TempdeptID).HasColumnName("TempdeptID");
            this.Property(t => t.TempdeptName).HasColumnName("TempdeptName");
            this.Property(t => t.TempuserID).HasColumnName("TempuserID");
            this.Property(t => t.TempuserName).HasColumnName("TempuserName");
            this.Property(t => t.IsProc).HasColumnName("IsProc");

            // Relationships
            this.HasOptional(t => t.VoteThemeInfo)
                .WithMany(t => t.VoteApplies)
                .HasForeignKey(d => d.ThemeID);

            this.HasOptional(t => t.TrackingWorkflow)
            .WithMany(t=>t.VoteApplys)
            .HasForeignKey(d => d.WorkflowInstanceId); 
        }
    }
}
