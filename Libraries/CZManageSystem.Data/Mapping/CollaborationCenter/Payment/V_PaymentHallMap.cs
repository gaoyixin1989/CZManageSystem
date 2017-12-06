using CZManageSystem.Data.Domain.CollaborationCenter.Payment;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Payment
{
    public class V_PaymentHallMap : EntityTypeConfiguration<V_PaymentHall>
    {
        public V_PaymentHallMap()
        {
            // Primary Key
            this.HasKey(t => t.HallID);
            this.Property(t => t.HallName)

          .HasMaxLength(50);
            this.Property(t => t.DpName)
           .IsRequired()
          .HasMaxLength(50);
            this.Property(t => t.DpFullName)
           .IsRequired()
          .HasMaxLength(256);
            // Table & Column Mappings
            this.ToTable("V_PaymentHall");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.HallName).HasColumnName("HallName");
            this.Property(t => t.DpName).HasColumnName("DpName");
            this.Property(t => t.DpFullName).HasColumnName("DpFullName");
        }
    }
}
