using CZManageSystem.Data.Domain.HumanResources.Integral;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Mapping.HumanResources.Integral
{
    public class HRNeedIntegralMap : EntityTypeConfiguration<HRNeedIntegral>
    {
        public HRNeedIntegralMap()
        {
            // Primary Key
            this.Property(t => t.UserName)

          .HasMaxLength(100);
            this.Property(t => t.YearDate)

          .HasMaxLength(100);
            this.Property(t => t.NeedIntegral)

          .HasMaxLength(100);
            // Table & Column Mappings
            this.ToTable("HRNeedIntegral");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.YearDate).HasColumnName("YearDate");
            this.Property(t => t.NeedIntegral).HasColumnName("NeedIntegral");
            this.Property(t => t.DoFlag).HasColumnName("DoFlag");
        }

    }
}
