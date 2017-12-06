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
            this.Property(t => t.备注)

          .HasMaxLength(255);
            this.Property(t => t.部门)
           .IsRequired()
          .HasMaxLength(50);
            this.Property(t => t.姓名)
           .IsRequired()
          .HasMaxLength(20);
            this.Property(t => t.acctno)

          .HasMaxLength(20);
            // Table & Column Mappings
            this.ToTable("pay_v");
            this.Property(t => t.employerid).HasColumnName("employerid");
            this.Property(t => t.billcyc).HasColumnName("billcyc");
            this.Property(t => t.固定收入).HasColumnName("固定收入");
            this.Property(t => t.工龄工资).HasColumnName("工龄工资");
            this.Property(t => t.月度考核奖).HasColumnName("月度考核奖");
            this.Property(t => t.话费补助).HasColumnName("话费补助");
            this.Property(t => t.交通补贴).HasColumnName("交通补贴");
            this.Property(t => t.值夜夜班津贴).HasColumnName("值夜夜班津贴");
            this.Property(t => t.节假日加班工资).HasColumnName("节假日加班工资");
            this.Property(t => t.其它).HasColumnName("其它");
            this.Property(t => t.机动奖合计).HasColumnName("机动奖合计");
            this.Property(t => t.总收入).HasColumnName("总收入");
            this.Property(t => t.社保扣款).HasColumnName("社保扣款");
            this.Property(t => t.医保扣款).HasColumnName("医保扣款");
            this.Property(t => t.住房公积金).HasColumnName("住房公积金");
            this.Property(t => t.宿舍房租及水电费).HasColumnName("宿舍房租及水电费");
            this.Property(t => t.其它扣款).HasColumnName("其它扣款");
            this.Property(t => t.社保企).HasColumnName("社保企");
            this.Property(t => t.医保企).HasColumnName("医保企");
            this.Property(t => t.住房公积金企).HasColumnName("住房公积金企");
            this.Property(t => t.应纳税所得额).HasColumnName("应纳税所得额");
            this.Property(t => t.个人所得税).HasColumnName("个人所得税");
            this.Property(t => t.实发).HasColumnName("实发");
            this.Property(t => t.备注).HasColumnName("备注");
            this.Property(t => t.更新时间).HasColumnName("更新时间");
            this.Property(t => t.部门).HasColumnName("部门");
            this.Property(t => t.deptid).HasColumnName("deptid");
            this.Property(t => t.姓名).HasColumnName("姓名");
            this.Property(t => t.手机号码).HasColumnName("手机号码");
            this.Property(t => t.postr).HasColumnName("postr");
            this.Property(t => t.acctno).HasColumnName("acctno");
            this.Property(t => t.pos).HasColumnName("pos");
        }
    }
}
