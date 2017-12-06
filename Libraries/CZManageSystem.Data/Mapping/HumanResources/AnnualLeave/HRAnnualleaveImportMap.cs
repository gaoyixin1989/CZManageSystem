using CZManageSystem.Data.Domain.HumanResources.AnnualLeave;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Mapping.HumanResources.AnnualLeave
{
    public class HRAnnualleaveImportMap : EntityTypeConfiguration<HRAnnualleaveImport>
    {
        public HRAnnualleaveImportMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            this.Property(t => t.Importor)

          .HasMaxLength(50);
            this.Property(t => t.ImportSn)

          .HasMaxLength(50);
            this.Property(t => t.ImportTitle)

          .HasMaxLength(50);
            this.Property(t => t.ImportMsg)
;
            this.Property(t => t.ImportInformation)
;
            this.Property(t => t.Remark)

          .HasMaxLength(200);
            this.Property(t => t.ImportType)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("HRAnnualleaveImport");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Importor).HasColumnName("Importor");
            this.Property(t => t.ImportTime).HasColumnName("ImportTime");
            this.Property(t => t.ImportSn).HasColumnName("ImportSn");
            this.Property(t => t.ImportTitle).HasColumnName("ImportTitle");
            this.Property(t => t.ImportMsg).HasColumnName("ImportMsg");
            this.Property(t => t.ImportInformation).HasColumnName("ImportInformation");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.ImportType).HasColumnName("ImportType");
        }
    }

}
