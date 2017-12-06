using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class UsersInRolesMap : EntityTypeConfiguration<UsersInRoles>
    {
        public UsersInRolesMap()
        {
            // Primary Key
            this.HasKey(t => new { t.UserId, t.RoleId });

            // Properties
            // Table & Column Mappings
            this.ToTable("bw_UsersInRoles");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            // Relationships
            this.HasRequired(t => t.Roles )
                .WithMany(t => t.UsersInRoles )
                .HasForeignKey(d => d.RoleId );
            this.HasRequired(t => t.Users )
                .WithMany(t => t.UsersInRoles )
                .HasForeignKey(d => d.UserId );
        }
    }
}
