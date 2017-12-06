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
            this.HasKey(t => new { t.billcyc, t.收入编号, t.员工编号 });
            this.Property(t => t.部门)

          .HasMaxLength(50);
            this.Property(t => t.姓名)

          .HasMaxLength(20);
            this.Property(t => t.账务周期)

          .HasMaxLength(16);
            this.Property(t => t.收入)

          .HasMaxLength(500);
            this.Property(t => t.固定收入项目)
           .IsRequired()
          .HasMaxLength(50);
            this.Property(t => t.所属类型)
           .IsRequired()
          .HasMaxLength(50);
            this.Property(t => t.value_str)

          .HasMaxLength(500);
            this.Property(t => t.DataType)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("gdpay_v");
            this.Property(t => t.部门).HasColumnName("部门");
            this.Property(t => t.deptid).HasColumnName("deptid");
            this.Property(t => t.姓名).HasColumnName("姓名");
            this.Property(t => t.员工编号).HasColumnName("员工编号");
            this.Property(t => t.账务周期).HasColumnName("账务周期");
            this.Property(t => t.收入编号).HasColumnName("收入编号");
            this.Property(t => t.收入).HasColumnName("收入");
            this.Property(t => t.固定收入项目).HasColumnName("固定收入项目");
            this.Property(t => t.更新时间).HasColumnName("更新时间");
            this.Property(t => t.billcyc).HasColumnName("billcyc");
            this.Property(t => t.pid).HasColumnName("pid");
            this.Property(t => t.所属类型).HasColumnName("所属类型");
            this.Property(t => t.value_str).HasColumnName("value_str");
            this.Property(t => t.DataType).HasColumnName("DataType");
        }
    }
}
