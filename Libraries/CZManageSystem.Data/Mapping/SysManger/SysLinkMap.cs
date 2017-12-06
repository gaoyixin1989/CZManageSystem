using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class SysLinkMap : EntityTypeConfiguration<SysLink>
    {
        public SysLinkMap()
        {
            // Primary Key
            this.HasKey(t => t.LinkId);

            // Properties
            this.Property(t => t.LinkName)
                .HasMaxLength(50);

            this.Property(t => t.LinkUrl);

            this.Property(t => t.Remark)
                .HasMaxLength(300);
            

            // Table & Column Mappings
            this.ToTable("SysLink");
            this.Property(t => t.LinkId).HasColumnName("LinkId");
            this.Property(t => t.LinkName).HasColumnName("LinkName");
            this.Property(t => t.LinkUrl).HasColumnName("LinkUrl");
            this.Property(t => t.OrderNo).HasColumnName("OrderNo");
            this.Property(t => t.EnableFlag).HasColumnName("EnableFlag");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.ValidTime).HasColumnName("ValidTime");
        }
    }
}
