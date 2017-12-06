using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class SysServiceStrategyMap : EntityTypeConfiguration<SysServiceStrategy>
    {
        public SysServiceStrategyMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties

            this.Property(t => t.PeriodType)
                .HasMaxLength(50);

            this.Property(t => t.Remark)
                .HasMaxLength(300);

            this.Property(t => t.Creator)
                .HasMaxLength(50);

            this.Property(t => t.LastModifier)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("SysServiceStrategy");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ServiceId).HasColumnName("ServiceId");
            this.Property(t => t.ValidTime).HasColumnName("ValidTime");
            this.Property(t => t.NextRunTime).HasColumnName("NextRunTime");
            this.Property(t => t.PeriodNum).HasColumnName("PeriodNum");
            this.Property(t => t.PeriodType).HasColumnName("PeriodType");
            this.Property(t => t.EnableFlag).HasColumnName("EnableFlag");
            this.Property(t => t.LogFlag).HasColumnName("LogFlag");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.Createdtime).HasColumnName("Createdtime");
            this.Property(t => t.LastModifier).HasColumnName("LastModifier");
            this.Property(t => t.LastModTime).HasColumnName("LastModTime");

            //Íâ¼ü
            this.HasRequired(t => t.SysServices)
                .WithMany(t => t.SysServiceStrategy)
                .HasForeignKey(d => d.ServiceId);
        }
    }
}
