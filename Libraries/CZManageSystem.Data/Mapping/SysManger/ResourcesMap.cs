using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class ResourcesMap : EntityTypeConfiguration<Resources>
    {
        public ResourcesMap()
        {
            // Primary Key
            this.HasKey(t => t.ResourceId );

            // Properties
            this.Property(t => t.ResourceId )
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ParentId)
                .HasMaxLength(50);

            this.Property(t => t.Type)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Name)
                //.IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.Alias)
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("bw_Resources");
            this.Property(t => t.ResourceId).HasColumnName("ResourceId");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Alias).HasColumnName("Alias");
            this.Property(t => t.Enabled).HasColumnName("Enabled");
            this.Property(t => t.CreatedTime).HasColumnName("CreatedTime");
            this.Property(t => t.Visible).HasColumnName("Visible");
            this.Property(t => t.SortIndex).HasColumnName("SortIndex");
        }
    }
}
