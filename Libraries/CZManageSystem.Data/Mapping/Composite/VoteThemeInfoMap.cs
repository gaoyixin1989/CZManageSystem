using CZManageSystem.Data.Domain.Composite;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.Composite
{
    public class VoteThemeInfoMap : EntityTypeConfiguration<VoteThemeInfo>
    {
        public VoteThemeInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ThemeID);

            // Properties
            this.Property(t => t.ThemeName)
                .HasMaxLength(200);

            this.Property(t => t.ThemetypeID)
                .HasMaxLength(50);

            this.Property(t => t.Creator)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VoteThemeInfo");
            this.Property(t => t.ThemeID).HasColumnName("ThemeID");
            this.Property(t => t.ThemeName).HasColumnName("ThemeName");
            this.Property(t => t.ThemetypeID).HasColumnName("ThemetypeID");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.CreatorID).HasColumnName("CreatorID");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.IsDel).HasColumnName("IsDel");
        }
    }
}
