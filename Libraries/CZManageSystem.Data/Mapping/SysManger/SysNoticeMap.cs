using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class SysNoticeMap : EntityTypeConfiguration<SysNotice>
    {
        public SysNoticeMap()
        {
            // Primary Key
            this.HasKey(t => t.NoticeId);

            // Properties
            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.Content)
                .IsRequired();

            this.Property(t => t.ValidTime).IsRequired();

            this.Property(t => t.EnableFlag).IsRequired();

            this.Property(t => t.OrderNo).IsRequired();

            this.Property(t => t.Creator)
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("SysNotice");
            this.Property(t => t.NoticeId).HasColumnName("NoticeId");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.ValidTime).HasColumnName("ValidTime");
            this.Property(t => t.EnableFlag).HasColumnName("EnableFlag");
            this.Property(t => t.OrderNo).HasColumnName("OrderNo");
            this.Property(t => t.Createdtime).HasColumnName("Createdtime");
            this.Property(t => t.Creator).HasColumnName("Creator");
        }
    }
}
