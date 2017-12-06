using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class SysServicesMap : EntityTypeConfiguration<SysServices>
    {
        public SysServicesMap()
        {
            // Primary Key
            this.HasKey(t => t.ServiceId);

            // Properties
            this.Property(t => t.ServiceName)
                .HasMaxLength(50);

            this.Property(t => t.AssemblyName)
                .HasMaxLength(100);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.ServiceDesc)
                .HasMaxLength(300);

            this.Property(t => t.Remark)
                .HasMaxLength(300);

            this.Property(t => t.Creator)
                .HasMaxLength(50);

            this.Property(t => t.LastModifier)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("SysServices");
            this.Property(t => t.ServiceId).HasColumnName("ServiceId");
            this.Property(t => t.ServiceName).HasColumnName("ServiceName");
            this.Property(t => t.AssemblyName).HasColumnName("AssemblyName");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.ServiceDesc).HasColumnName("ServiceDesc");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.Createdtime).HasColumnName("Createdtime");
            this.Property(t => t.LastModifier).HasColumnName("LastModifier");
            this.Property(t => t.LastModTime).HasColumnName("LastModTime");
        }
    }
}
