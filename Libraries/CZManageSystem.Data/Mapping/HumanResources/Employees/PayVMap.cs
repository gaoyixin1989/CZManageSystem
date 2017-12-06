using CZManageSystem.Data.Domain.HumanResources.Employees;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Employees
{
    public class PayVMap : EntityTypeConfiguration<PayV>
    {
        public PayVMap()
        {
            // Primary Key
            this.HasKey(k => new { k.employerid, k.billcyc });
            this.Property(t => t.��ע)

          .HasMaxLength(255);
            this.Property(t => t.����)
           .IsRequired()
          .HasMaxLength(50);
            this.Property(t => t.����)
           .IsRequired()
          .HasMaxLength(20);
            this.Property(t => t.acctno)

          .HasMaxLength(20);
            // Table & Column Mappings
            this.ToTable("pay_v");
            this.Property(t => t.employerid).HasColumnName("employerid");
            this.Property(t => t.billcyc).HasColumnName("billcyc");
            this.Property(t => t.�̶�����).HasColumnName("�̶�����");
            this.Property(t => t.���乤��).HasColumnName("���乤��");
            this.Property(t => t.�¶ȿ��˽�).HasColumnName("�¶ȿ��˽�");
            this.Property(t => t.���Ѳ���).HasColumnName("���Ѳ���");
            this.Property(t => t.��ͨ����).HasColumnName("��ͨ����");
            this.Property(t => t.ֵҹҹ�����).HasColumnName("ֵҹҹ�����");
            this.Property(t => t.�ڼ��ռӰ๤��).HasColumnName("�ڼ��ռӰ๤��");
            this.Property(t => t.����).HasColumnName("����");
            this.Property(t => t.�������ϼ�).HasColumnName("�������ϼ�");
            this.Property(t => t.������).HasColumnName("������");
            this.Property(t => t.�籣�ۿ�).HasColumnName("�籣�ۿ�");
            this.Property(t => t.ҽ���ۿ�).HasColumnName("ҽ���ۿ�");
            this.Property(t => t.ס��������).HasColumnName("ס��������");
            this.Property(t => t.���᷿�⼰ˮ���).HasColumnName("���᷿�⼰ˮ���");
            this.Property(t => t.�����ۿ�).HasColumnName("�����ۿ�");
            this.Property(t => t.�籣��).HasColumnName("�籣��");
            this.Property(t => t.ҽ����).HasColumnName("ҽ����");
            this.Property(t => t.ס����������).HasColumnName("ס����������");
            this.Property(t => t.Ӧ��˰���ö�).HasColumnName("Ӧ��˰���ö�");
            this.Property(t => t.��������˰).HasColumnName("��������˰");
            this.Property(t => t.ʵ��).HasColumnName("ʵ��");
            this.Property(t => t.��ע).HasColumnName("��ע");
            this.Property(t => t.����ʱ��).HasColumnName("����ʱ��");
            this.Property(t => t.����).HasColumnName("����");
            this.Property(t => t.deptid).HasColumnName("deptid");
            this.Property(t => t.����).HasColumnName("����");
            this.Property(t => t.�ֻ�����).HasColumnName("�ֻ�����");
            this.Property(t => t.postr).HasColumnName("postr");
            this.Property(t => t.acctno).HasColumnName("acctno");
            this.Property(t => t.pos).HasColumnName("pos");
        }
    }
}
