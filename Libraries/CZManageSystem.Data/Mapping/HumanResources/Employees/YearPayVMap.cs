using CZManageSystem.Data.Domain.HumanResources.Employees;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Employees
{
	public class YearPayVMap : EntityTypeConfiguration<YearPayV>
	{
		public YearPayVMap()
		{
            // Primary Key
            this.HasKey(t=>new { t.billcyc ,t.bz ,t.deptname ,t.employerid ,t.pos ,t.postr ,t.sjhm ,t.���� ,t.ʵ������ϼ� ,t.����˫н��ʵ�� ,t.����˫н��Ӧ�� ,t.���տ��˽�ʵ�� ,t.���տ��˽�Ӧ�� ,t.Ӧ���ϼ� ,t.Ӧ�۸��� });
            this.HasKey(T=>T.employerid);
                
			  this.Property(t => t.deptname)
			 .IsRequired()
			.HasMaxLength(50);
			  this.Property(t => t.����)
			 .IsRequired()
			.HasMaxLength(20);
			  this.Property(t => t.bz)

			.HasMaxLength(255);
			// Table & Column Mappings
 			 this.ToTable("yearpay_v"); 
			this.Property(t => t.deptname).HasColumnName("deptname"); 
			this.Property(t => t.����).HasColumnName("����"); 
			this.Property(t => t.sjhm).HasColumnName("sjhm"); 
			this.Property(t => t.pos).HasColumnName("pos"); 
			this.Property(t => t.postr).HasColumnName("postr"); 
			this.Property(t => t.employerid).HasColumnName("employerid"); 
			this.Property(t => t.billcyc).HasColumnName("billcyc"); 
			this.Property(t => t.���տ��˽�Ӧ��).HasColumnName("���տ��˽�Ӧ��"); 
			this.Property(t => t.����˫н��Ӧ��).HasColumnName("����˫н��Ӧ��"); 
			this.Property(t => t.���տ��˽�ʵ��).HasColumnName("���տ��˽�ʵ��"); 
			this.Property(t => t.����˫н��ʵ��).HasColumnName("����˫н��ʵ��"); 
			this.Property(t => t.Ӧ���ϼ�).HasColumnName("Ӧ���ϼ�"); 
			this.Property(t => t.Ӧ�۸���).HasColumnName("Ӧ�۸���"); 
			this.Property(t => t.ʵ������ϼ�).HasColumnName("ʵ������ϼ�"); 
			this.Property(t => t.bz).HasColumnName("bz"); 
		 }
	 }
}
