using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class RolesInResourcesMap : EntityTypeConfiguration<RolesInResources>
    {
        public RolesInResourcesMap()
        {
            // Primary Key
            this.HasKey(t => new { t.RoleId, t.ResourceId });

            // Properties
            this.Property(t => t.ResourceId)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("bw_RolesInResources");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.ResourceId).HasColumnName("ResourceId");
            // Relationships
            //this.HasRequired(t => t.Roles )
            //    .WithMany(t => t.RolesInResources )
            //    .HasForeignKey(d => d.RoleId );
            //this.HasRequired(t => t.Resources )
            //    .WithMany(t => t.RolesInResources )
            //    .HasForeignKey(d => d.ResourceId );
        }
    }
}
