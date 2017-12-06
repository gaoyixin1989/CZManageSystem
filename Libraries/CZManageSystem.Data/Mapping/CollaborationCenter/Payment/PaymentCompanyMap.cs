using CZManageSystem.Data.Domain.CollaborationCenter.Payment;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Payment
{
    public class PaymentCompanyMap : EntityTypeConfiguration<PaymentCompany>
    {
        public PaymentCompanyMap()
        {
            // Primary Key
            this.HasKey(t => t.DpId);
            this.Property(t => t.DpCode)

          .HasMaxLength(640);
            this.Property(t => t.DpName)

          .HasMaxLength(100);
            this.Property(t => t.DpFullName)

          .HasMaxLength(1000);
            this.Property(t => t.DpDesc)

          .HasMaxLength(500);
            this.Property(t => t.OrderNo)

          .HasMaxLength(640);
            // Table & Column Mappings
            this.ToTable("PaymentCompany");
            this.Property(t => t.DpId).HasColumnName("DpId");
            this.Property(t => t.ParentDpId).HasColumnName("ParentDpId");
            this.Property(t => t.DpCode).HasColumnName("DpCode");
            this.Property(t => t.DpLv).HasColumnName("DpLv");
            this.Property(t => t.DpName).HasColumnName("DpName");
            this.Property(t => t.DpFullName).HasColumnName("DpFullName");
            this.Property(t => t.DpDesc).HasColumnName("DpDesc");
            this.Property(t => t.IsShow).HasColumnName("IsShow");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.OrderNo).HasColumnName("OrderNo");
            this.Property(t => t.IsTmpDP).HasColumnName("IsTmpDP");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
            this.Property(t => t.AllowDist).HasColumnName("AllowDist");
            this.HasMany(t => t.PaymentPayers).WithOptional().HasForeignKey(t => t.CompanyID);
            this.HasMany(t => t.PaymentCompanyHalls).WithOptional().HasForeignKey(t => t.CompanyId);
        }
    }
}
