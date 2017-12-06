using CZManageSystem.Data.Domain.Composite;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.Composite
{
    public class OGSMElectricityMap : EntityTypeConfiguration<OGSMElectricity>
    {
        public OGSMElectricityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            this.Property(t => t.USR_NBR)

          .HasMaxLength(50);
            
            this.Property(t => t.ElectricityMeter)

          .HasMaxLength(50);
            this.Property(t => t.Electricity)

          .HasMaxLength(50);
            this.Property(t => t.Remark)

          .HasMaxLength(250);
            this.Property(t => t.Creator)

          .HasMaxLength(50);
            this.Property(t => t.LastModifier)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("OGSMElectricity");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.USR_NBR).HasColumnName("USR_NBR");
            this.Property(t => t.PAY_MON).HasColumnName("PAY_MON");
            this.Property(t => t.ElectricityMeter).HasColumnName("ElectricityMeter");
            this.Property(t => t.Electricity).HasColumnName("Electricity");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.CreatedTime).HasColumnName("CreatedTime");
            this.Property(t => t.LastModifier).HasColumnName("LastModifier");
            this.Property(t => t.LastModTime).HasColumnName("LastModTime");
        }
    }

}
