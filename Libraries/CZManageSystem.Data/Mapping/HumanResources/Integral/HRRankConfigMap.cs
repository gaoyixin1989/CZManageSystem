using CZManageSystem.Data.Domain.HumanResources.Integral;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Mapping.HumanResources.Integral
{
    public class HRRankConfigMap : EntityTypeConfiguration<HRRankConfig>
    {
        public HRRankConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Table & Column Mappings
            this.ToTable("HRRankConfig");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SGrade).HasColumnName("SGrade");
            this.Property(t => t.EGrade).HasColumnName("EGrade");
            this.Property(t => t.Integral).HasColumnName("Integral");
        }
    }

}
