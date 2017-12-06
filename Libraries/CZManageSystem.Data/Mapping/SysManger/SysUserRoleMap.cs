using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class SysUserRoleMap : EntityTypeConfiguration<SysUserRole>
    {
        public SysUserRoleMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("SysUserRole");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.RoleID).HasColumnName("RoleID");

            // Relationships
            this.HasRequired(t => t.SysRole)
                .WithMany(t => t.SysUserRoles)
                .HasForeignKey(d => d.RoleID);
            this.HasRequired(t => t.SysUser)
                .WithMany(t => t.SysUserRoles)
                .HasForeignKey(d => d.UserID); 
        }
    }
}
