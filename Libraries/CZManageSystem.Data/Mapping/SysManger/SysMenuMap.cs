using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class SysMenuMap : EntityTypeConfiguration<SysMenu>
    {
        public SysMenuMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.MenuName)
                .HasMaxLength(50);

            this.Property(t => t.MenuFullName)
                .HasMaxLength(50);

            this.Property(t => t.PageUrl)
                .HasMaxLength(250);

            this.Property(t => t.MenuType)
                .HasMaxLength(50);

            this.Property(t => t.Remark)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("SysMenu");
            this.Property(t => t.Id).HasColumnName("MenuId");
            this.Property(t => t.MenuName).HasColumnName("MenuName");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.MenuFullName).HasColumnName("MenuFullName");
            this.Property(t => t.MenuLevel).HasColumnName("MenuLevel");
            this.Property(t => t.OrderNo).HasColumnName("OrderNo");
            this.Property(t => t.PageUrl).HasColumnName("PageUrl");
            this.Property(t => t.MenuType).HasColumnName("MenuType");
            this.Property(t => t.EnableFlag).HasColumnName("EnableFlag");
            this.Property(t => t.Remark).HasColumnName("Remark");
        }
    }
}
