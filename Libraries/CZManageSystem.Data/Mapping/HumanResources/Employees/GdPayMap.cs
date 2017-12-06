using CZManageSystem.Data.Domain.HumanResources.Employees;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Employees
{
    public class GdPayMap : EntityTypeConfiguration<GdPay>
    {
        public GdPayMap()
        {
            // Primary Key
            this.HasKey(t => new { t.employerid, t.billcyc, t.payid });
            this.Property(t => t.je)

            .HasMaxLength(500);
            this.Property(t => t.value_str)

          .HasMaxLength(500);
            // Table & Column Mappings
            this.ToTable("gdpay");
            this.Property(t => t.employerid).HasColumnName("employerid");
            this.Property(t => t.billcyc).HasColumnName("billcyc");
            this.Property(t => t.payid).HasColumnName("payid");
            this.Property(t => t.je).HasColumnName("je");
            this.Property(t => t.updatetime).HasColumnName("updatetime");
            this.Property(t => t.value_str).HasColumnName("value_str");
        }
    }
}
