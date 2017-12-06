using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class DeptsMap : EntityTypeConfiguration<Depts>
    {
        public DeptsMap()
        {
            // Primary Key
            this.HasKey(t => t.DpId );

            // Properties
            this.Property(t => t.DpId)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.DpName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ParentDpId)
                .HasMaxLength(50);

            this.Property(t => t.DpFullName)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.Creator)
                .HasMaxLength(50);

            this.Property(t => t.LastModifier)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("bw_Depts");
            this.Property(t => t.DpId).HasColumnName("DpId");
            this.Property(t => t.DpName).HasColumnName("DpName");
            this.Property(t => t.ParentDpId).HasColumnName("ParentDpId");
            this.Property(t => t.DpFullName).HasColumnName("DpFullName");
            this.Property(t => t.DpLevel).HasColumnName("DpLevel");
            this.Property(t => t.DeptOrderNo).HasColumnName("DeptOrderNo");
            this.Property(t => t.IsTmpDp).HasColumnName("IsTmpDp");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.CreatedTime).HasColumnName("CreatedTime");
            this.Property(t => t.LastModTime).HasColumnName("LastModTime");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.LastModifier).HasColumnName("LastModifier");
        }
    }
}
