using CZManageSystem.Data.Domain.Administrative.BirthControl;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
    public class BirthControlConfigMap : EntityTypeConfiguration<BirthControlConfig>
    {
        public BirthControlConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.id);
            this.Property(t => t.ManAge)

          .HasMaxLength(50);
            this.Property(t => t.WomenAge)

          .HasMaxLength(50);
            this.Property(t => t.IsPush)

          .HasMaxLength(1);
            // Table & Column Mappings
            this.ToTable("BirthControlConfig");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.ConfirmStartdate).HasColumnName("ConfirmStartdate");
            this.Property(t => t.ConfirmEnddate).HasColumnName("ConfirmEnddate");
            this.Property(t => t.ManAge).HasColumnName("ManAge");
            this.Property(t => t.WomenAge).HasColumnName("WomenAge");
            this.Property(t => t.IsPush).HasColumnName("IsPush");

        }
    }
}
