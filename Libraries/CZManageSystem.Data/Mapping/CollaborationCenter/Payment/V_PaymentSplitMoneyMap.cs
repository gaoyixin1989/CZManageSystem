using CZManageSystem.Data.Domain.CollaborationCenter.Payment;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Payment
{
    public class V_PaymentSplitMoneyMap : EntityTypeConfiguration<V_PaymentSplitMoney>
    {
        public V_PaymentSplitMoneyMap()
        {
            // Primary Key

            this.HasKey(t => t.SplitID);
            this.Property(t => t.Purpose)

            .HasMaxLength(50);
            this.Property(t => t.PayerAccount)

          .HasMaxLength(50);
            this.Property(t => t.PayerName)

          .HasMaxLength(50);
            this.Property(t => t.PayerBranchCode)

          .HasMaxLength(50);
            this.Property(t => t.PayeeAccount)

          .HasMaxLength(50);
            this.Property(t => t.PayeeName)

          .HasMaxLength(50);
            this.Property(t => t.PayeeBranch)

          .HasMaxLength(50);
            this.Property(t => t.PayeeBranchCode)

          .HasMaxLength(50);
            this.Property(t => t.PayeeOpenBank)

          .HasMaxLength(50);
            this.Property(t => t.PayeeBank)

          .HasMaxLength(50);
            this.Property(t => t.PayeeBankCode)

          .HasMaxLength(50);
            this.Property(t => t.PayeeAddressCode)

          .HasMaxLength(50);
            this.Property(t => t.MoneyType)

          .HasMaxLength(50);
            this.Property(t => t.PayeeAreaCode)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("V_PaymentSplitMoney");
            this.Property(t => t.SplitID).HasColumnName("SplitID");
            this.Property(t => t.PayMoneyID).HasColumnName("PayMoneyID");
            this.Property(t => t.SplitMoney).HasColumnName("SplitMoney");
            this.Property(t => t.Purpose).HasColumnName("Purpose");
            this.Property(t => t.ApplyID).HasColumnName("ApplyID");
            this.Property(t => t.PayerAccount).HasColumnName("PayerAccount");
            this.Property(t => t.PayerName).HasColumnName("PayerName");
            this.Property(t => t.PayerBranchCode).HasColumnName("PayerBranchCode");
            this.Property(t => t.PayeeAccount).HasColumnName("PayeeAccount");
            this.Property(t => t.PayeeName).HasColumnName("PayeeName");
            this.Property(t => t.PayeeBranch).HasColumnName("PayeeBranch");
            this.Property(t => t.PayeeBranchCode).HasColumnName("PayeeBranchCode");
            this.Property(t => t.PayeeOpenBank).HasColumnName("PayeeOpenBank");
            this.Property(t => t.PayeeBank).HasColumnName("PayeeBank");
            this.Property(t => t.PayeeBankCode).HasColumnName("PayeeBankCode");
            this.Property(t => t.PayeeAddressCode).HasColumnName("PayeeAddressCode");
            this.Property(t => t.MoneyType).HasColumnName("MoneyType");
            this.Property(t => t.PayeeAreaCode).HasColumnName("PayeeAreaCode");
        }
    }
}
