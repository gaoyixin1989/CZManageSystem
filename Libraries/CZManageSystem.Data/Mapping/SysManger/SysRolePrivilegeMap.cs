using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class SysRolePrivilegeMap : EntityTypeConfiguration<SysRolePrivilege>
    {
        public SysRolePrivilegeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("SysRolePrivilege");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.MenuId).HasColumnName("MenuId");
            this.Property(t => t.RoleID).HasColumnName("RoleID");

            // Relationships
            this.HasRequired(t => t.SysMenu)
                .WithMany(t => t.SysRolePrivileges)
                .HasForeignKey(d => d.MenuId);
            this.HasRequired(t => t.SysRole)
                .WithMany(t => t.SysRolePrivileges)
                .HasForeignKey(d => d.RoleID);

        }
    }
}
