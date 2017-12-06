using CZManageSystem.Data.Domain.HumanResources.Employees;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Employees
{
	public class GdPayIdMap : EntityTypeConfiguration<GdPayId>
	{
		public GdPayIdMap()
		{
			// Primary Key
			 this.HasKey(t => t.payid);
			  this.Property(t => t.payname)
			 .IsRequired()
			.HasMaxLength(50);
			  this.Property(t => t.bz)

			.HasMaxLength(50);
			  this.Property(t => t.DataType)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("gdpayid"); 
			this.Property(t => t.payid).HasColumnName("payid"); 
			this.Property(t => t.payname).HasColumnName("payname"); 
			this.Property(t => t.pid).HasColumnName("pid"); 
			this.Property(t => t.bz).HasColumnName("bz"); 
			this.Property(t => t.sort).HasColumnName("sort"); 
			// 是否独占一行
			this.Property(t => t.RowExclusive).HasColumnName("RowExclusive"); 
			// 是否沿用上一个周期的数据
			this.Property(t => t.Inherit).HasColumnName("Inherit"); 
			this.Property(t => t.DataType).HasColumnName("DataType"); 
		 }
	 }
}
