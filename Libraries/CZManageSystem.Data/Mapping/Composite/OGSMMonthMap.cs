using CZManageSystem.Data.Domain.Composite;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.Composite
{
    public class OGSMMonthMap : EntityTypeConfiguration<OGSMMonth>
    {
        public OGSMMonthMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties

            this.Property(t => t.USR_NBR)
                .HasMaxLength(50);

            this.Property(t => t.IsPayment)
                .HasMaxLength(50);

            this.Property(t => t.AccountNo)
                .HasMaxLength(50);

            this.Property(t => t.Remark)
                .HasMaxLength(200);

            this.Property(t => t.Creator)
                .HasMaxLength(50);

            this.Property(t => t.LastModifier)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("OGSMMonth");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PAY_MON).HasColumnName("PAY_MON");
            this.Property(t => t.USR_NBR).HasColumnName("USR_NBR");
            this.Property(t => t.IsPayment).HasColumnName("IsPayment");
            this.Property(t => t.PaymentTime).HasColumnName("PaymentTime");
            this.Property(t => t.AccountTime).HasColumnName("AccountTime");
            this.Property(t => t.AccountMoney).HasColumnName("AccountMoney");
            this.Property(t => t.AccountNo).HasColumnName("AccountNo");
            this.Property(t => t.CMPower2G).HasColumnName("CMPower2G");
            this.Property(t => t.CMPower3G).HasColumnName("CMPower3G");
            this.Property(t => t.CMPower4G).HasColumnName("CMPower4G");
            this.Property(t => t.CUPower2G).HasColumnName("CUPower2G");
            this.Property(t => t.CUPower3G).HasColumnName("CUPower3G");
            this.Property(t => t.CUPower4G).HasColumnName("CUPower4G");
            this.Property(t => t.CTPower2G).HasColumnName("CTPower2G");
            this.Property(t => t.CTPower3G).HasColumnName("CTPower3G");
            this.Property(t => t.CTPower4G).HasColumnName("CTPower4G");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.CreatedTime).HasColumnName("CreatedTime");
            this.Property(t => t.LastModifier).HasColumnName("LastModifier");
            this.Property(t => t.LastModTime).HasColumnName("LastModTime");

           
        }
    }
}
