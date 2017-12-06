using CZManageSystem.Data.Domain.CollaborationCenter.Payment;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Payment
{
    public class V_PaymentPayerMap : EntityTypeConfiguration<V_PaymentPayer>
    {
        public V_PaymentPayerMap()
        {
            // Primary Key
            this.HasKey(t => t.PayerID);
            this.Property(t => t.Account)

          .HasMaxLength(50);
            this.Property(t => t.Name)

          .HasMaxLength(50);
            this.Property(t => t.BranchCode)

          .HasMaxLength(50);
            this.Property(t => t.CompanyName)

          .HasMaxLength(100);
            // Table & Column Mappings
            this.ToTable("V_PaymentPayer");
            this.Property(t => t.PayerID).HasColumnName("PayerID");
            this.Property(t => t.Account).HasColumnName("Account");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.BranchCode).HasColumnName("BranchCode");
            this.Property(t => t.CompanyID).HasColumnName("CompanyID");
            this.Property(t => t.CompanyName).HasColumnName("CompanyName");
        }
    }
}
