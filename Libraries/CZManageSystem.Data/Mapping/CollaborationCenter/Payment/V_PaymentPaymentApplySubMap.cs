using CZManageSystem.Data.Domain.CollaborationCenter.Payment;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Payment
{
    public class V_PaymentPaymentApplySubMap : EntityTypeConfiguration<V_PaymentPaymentApplySub>
    {
        public V_PaymentPaymentApplySubMap()
        {
            // Primary Key
            this.HasKey(t => t.ApplyID);
            this.Property(t => t.ApplyTitle)

          .HasMaxLength(200);
            this.Property(t => t.ApplySn)

          .HasMaxLength(50);
            this.Property(t => t.Mobile)

          .HasMaxLength(20);
            this.Property(t => t.Series)

          .HasMaxLength(50);
            this.Property(t => t.Status)

          .HasMaxLength(50);
            this.Property(t => t.HallID)

          .HasMaxLength(50);
            this.Property(t => t.Remark)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("V_PaymentPaymentApplySub");
            this.Property(t => t.ApplyID).HasColumnName("ApplyID");
            this.Property(t => t.SubStatus).HasColumnName("SubStatus");
            this.Property(t => t.ApplyTitle).HasColumnName("ApplyTitle");
            this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId");
            this.Property(t => t.ApplySn).HasColumnName("ApplySn");
            this.Property(t => t.Mobile).HasColumnName("Mobile");
            this.Property(t => t.MainApplyID).HasColumnName("MainApplyID");
            this.Property(t => t.PayDay).HasColumnName("PayDay");
            this.Property(t => t.Series).HasColumnName("Series");
            this.Property(t => t.AppliCant).HasColumnName("AppliCant");
            this.Property(t => t.ApplyTime).HasColumnName("ApplyTime");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.CompanyID).HasColumnName("CompanyID");
            this.Property(t => t.Remark).HasColumnName("Remark");
        }
    }
}
