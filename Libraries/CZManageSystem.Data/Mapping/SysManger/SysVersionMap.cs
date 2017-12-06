using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Models.Mapping
{
    public class SysVersionMap : EntityTypeConfiguration<SysVersion>
    {
        public SysVersionMap()
        {
            // Primary Key
            this.HasKey(t => t.VerId);

            // Properties
            this.Property(t => t.Version)
                .HasMaxLength(50);

            this.Property(t => t.VerDsc)
                .HasMaxLength(50);

            this.Property(t => t.UpdateTime)
                .HasMaxLength(50);

            this.Property(t => t.Remark)
                .HasMaxLength(300);

            // Table & Column Mappings
            this.ToTable("SysVersion");
            this.Property(t => t.VerId).HasColumnName("VerId");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.VerDsc).HasColumnName("VerDsc");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
            this.Property(t => t.Remark).HasColumnName("Remark");
        }
    }
}
