using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class SysDeptmentMap:EntityTypeConfiguration<Domain.SysManger.SysDeptment>
    {
        public SysDeptmentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DpName)
                .HasMaxLength(50);

            this.Property(t => t.ParentDpId)
                .HasMaxLength(50);

            this.Property(t => t.DpFullName)
                .HasMaxLength(256);

            this.Property(t => t.Remark)
                .HasMaxLength(300);

            this.ToTable("SysDeptment");
            //this.HasKey(t => t.Id);
            this.Property(t => t.Id).HasColumnName("DpId");
            this.Property(t => t.DpName).HasColumnName("DpName");
            this.Property(t => t.ParentDpId).HasColumnName("ParentDpId");
            this.Property(t => t.DpFullName).HasColumnName("DpFullName");
            this.Property(t => t.DpLevel).HasColumnName("DpLevel");
            this.Property(t => t.OrderNo).HasColumnName("OrderNo");
            this.Property(t => t.IsOuter).HasColumnName("IsOuter");
            this.Property(t => t.Remark).HasColumnName("Remark");
        }
    }
}
