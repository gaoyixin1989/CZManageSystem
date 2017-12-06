using CZManageSystem.Data.Domain.HumanResources.Integral;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Mapping.HumanResources.Integral
{
    public class HRCintegralConfigMap : EntityTypeConfiguration<HRCintegralConfig>
    {
        public HRCintegralConfigMap()
        {
            // Primary Key
            this.Property(t => t.Id);
            // Table & Column Mappings
            this.ToTable("HRCIntegralConfig");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Mindays).HasColumnName("Mindays");
            this.Property(t => t.Maxdays).HasColumnName("Maxdays");
            this.Property(t => t.Integral).HasColumnName("Integral");
            this.Property(t => t.Times).HasColumnName("Times");
            this.Property(t => t.BuseFormula).HasColumnName("BuseFormula");
        }

    }
}
