using CZManageSystem.Data.Domain.CollaborationCenter.Payment;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Payment
{
    public class V_PaymentUserMap : EntityTypeConfiguration<V_PaymentUser>
    {
        public V_PaymentUserMap()
        {
            // Primary Key
            this.HasKey(t => t.UserID);
            this.Property(t => t.LoginID)

            .HasMaxLength(50);
            this.Property(t => t.PassWord)

          .HasMaxLength(50);
            this.Property(t => t.UserName)

          .HasMaxLength(50);
            this.Property(t => t.Mobile)

          .HasMaxLength(50);
            this.Property(t => t.Phone)

          .HasMaxLength(50);
            this.Property(t => t.Status)

          .HasMaxLength(50);
            this.Property(t => t.DpCode)

          .HasMaxLength(640);
            this.Property(t => t.DpName)

          .HasMaxLength(100);
            this.Property(t => t.DpFullName)

          .HasMaxLength(1000);
            // Table & Column Mappings
            this.ToTable("V_PaymentUser");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.LoginID).HasColumnName("LoginID");
            this.Property(t => t.PassWord).HasColumnName("PassWord");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.Mobile).HasColumnName("Mobile");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.CompanyID).HasColumnName("CompanyID");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.DpId).HasColumnName("DpId");
            this.Property(t => t.DpCode).HasColumnName("DpCode");
            this.Property(t => t.DpLv).HasColumnName("DpLv");
            this.Property(t => t.DpName).HasColumnName("DpName");
            this.Property(t => t.DpFullName).HasColumnName("DpFullName");
        }
    }
}
