using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class RolesMap : EntityTypeConfiguration<Roles>
    {
        public RolesMap()
        {
            // Primary Key
            this.HasKey(t => t.RoleId );

            // Properties
            this.Property(t => t.RoleName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Comment)
                .HasMaxLength(256);

            this.Property(t => t.Creator)
                .HasMaxLength(50);

            this.Property(t => t.LastModifier)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("bw_Roles");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.IsInheritable).HasColumnName("IsInheritable");
            this.Property(t => t.RoleName).HasColumnName("RoleName");
            this.Property(t => t.Comment).HasColumnName("Comment");
            this.Property(t => t.BeginTime).HasColumnName("BeginTime");
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            this.Property(t => t.CreatedTime).HasColumnName("CreatedTime");
            this.Property(t => t.LastModTime).HasColumnName("LastModTime");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.LastModifier).HasColumnName("LastModifier");
            this.Property(t => t.SortOrder).HasColumnName("SortOrder");
        }
    }
}
