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
            this.HasKey(t=>new { t.billcyc ,t.bz ,t.deptname ,t.employerid ,t.pos ,t.postr ,t.sjhm ,t.姓名 ,t.实发奖金合计 ,t.年终双薪奖实发 ,t.年终双薪奖应发 ,t.年终考核奖实发 ,t.年终考核奖应发 ,t.应发合计 ,t.应扣个所 });
            this.HasKey(T=>T.employerid);
                
			  this.Property(t => t.deptname)
			 .IsRequired()
			.HasMaxLength(50);
			  this.Property(t => t.姓名)
			 .IsRequired()
			.HasMaxLength(20);
			  this.Property(t => t.bz)

			.HasMaxLength(255);
			// Table & Column Mappings
 			 this.ToTable("yearpay_v"); 
			this.Property(t => t.deptname).HasColumnName("deptname"); 
			this.Property(t => t.姓名).HasColumnName("姓名"); 
			this.Property(t => t.sjhm).HasColumnName("sjhm"); 
			this.Property(t => t.pos).HasColumnName("pos"); 
			this.Property(t => t.postr).HasColumnName("postr"); 
			this.Property(t => t.employerid).HasColumnName("employerid"); 
			this.Property(t => t.billcyc).HasColumnName("billcyc"); 
			this.Property(t => t.年终考核奖应发).HasColumnName("年终考核奖应发"); 
			this.Property(t => t.年终双薪奖应发).HasColumnName("年终双薪奖应发"); 
			this.Property(t => t.年终考核奖实发).HasColumnName("年终考核奖实发"); 
			this.Property(t => t.年终双薪奖实发).HasColumnName("年终双薪奖实发"); 
			this.Property(t => t.应发合计).HasColumnName("应发合计"); 
			this.Property(t => t.应扣个所).HasColumnName("应扣个所"); 
			this.Property(t => t.实发奖金合计).HasColumnName("实发奖金合计"); 
			this.Property(t => t.bz).HasColumnName("bz"); 
		 }
	 }
}
