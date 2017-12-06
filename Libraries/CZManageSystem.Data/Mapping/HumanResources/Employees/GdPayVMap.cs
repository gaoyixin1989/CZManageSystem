using CZManageSystem.Data.Domain.HumanResources.Employees;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Employees
{
    public class GdPayVMap : EntityTypeConfiguration<GdPayV>
    {
        public GdPayVMap()
        {
            // Primary Key
            this.HasKey(t => new { t.billcyc, t.������, t.Ա����� });
            this.Property(t => t.����)

          .HasMaxLength(50);
            this.Property(t => t.����)

          .HasMaxLength(20);
            this.Property(t => t.��������)

          .HasMaxLength(16);
            this.Property(t => t.����)

          .HasMaxLength(500);
            this.Property(t => t.�̶�������Ŀ)
           .IsRequired()
          .HasMaxLength(50);
            this.Property(t => t.��������)
           .IsRequired()
          .HasMaxLength(50);
            this.Property(t => t.value_str)

          .HasMaxLength(500);
            this.Property(t => t.DataType)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("gdpay_v");
            this.Property(t => t.����).HasColumnName("����");
            this.Property(t => t.deptid).HasColumnName("deptid");
            this.Property(t => t.����).HasColumnName("����");
            this.Property(t => t.Ա�����).HasColumnName("Ա�����");
            this.Property(t => t.��������).HasColumnName("��������");
            this.Property(t => t.������).HasColumnName("������");
            this.Property(t => t.����).HasColumnName("����");
            this.Property(t => t.�̶�������Ŀ).HasColumnName("�̶�������Ŀ");
            this.Property(t => t.����ʱ��).HasColumnName("����ʱ��");
            this.Property(t => t.billcyc).HasColumnName("billcyc");
            this.Property(t => t.pid).HasColumnName("pid");
            this.Property(t => t.��������).HasColumnName("��������");
            this.Property(t => t.value_str).HasColumnName("value_str");
            this.Property(t => t.DataType).HasColumnName("DataType");
        }
    }
}
