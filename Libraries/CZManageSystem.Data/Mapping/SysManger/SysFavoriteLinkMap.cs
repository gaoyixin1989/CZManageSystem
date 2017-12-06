using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class SysFavoriteLinkMap : EntityTypeConfiguration<SysFavoriteLink>
    {
        public SysFavoriteLinkMap()
        {
            // Primary Key
            this.HasKey(t => t.FavoriteLinkId);

            // Properties
            this.Property(t => t.FavoriteLinkName)
                .HasMaxLength(50);

            this.Property(t => t.FavoriteLinkUrl)
                .HasMaxLength(50);

            this.Property(t => t.Remark)
                .HasMaxLength(300);

            // Table & Column Mappings
            this.ToTable("SysFavoriteLink");
            this.Property(t => t.FavoriteLinkId).HasColumnName("FavoriteLinkId");
            this.Property(t => t.FavoriteLinkName).HasColumnName("FavoriteLinkName");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.FavoriteLinkUrl).HasColumnName("FavoriteLinkUrl");
            this.Property(t => t.OrderNo).HasColumnName("OrderNo");
            this.Property(t => t.EnableFlag).HasColumnName("EnableFlag");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
