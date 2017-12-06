using CZManageSystem.Data.Domain.HumanResources.Employees;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Employees
{
	public class VWHRSumMap : EntityTypeConfiguration<VWHRSum>
	{
		public VWHRSumMap()
		{
            // Primary Key
            this.HasKey(t=>new {t.acctno ,t.billcyc ,t.employerid ,t.name ,t.sjhm ,t.updatetime ,t.ʵ�� ,t.Ӧ�۳� ,t.������ });
			  this.Property(t => t.name)
			 .IsRequired()
			.HasMaxLength(20);
			  this.Property(t => t.acctno)

			.HasMaxLength(20);
			// Table & Column Mappings
 			 this.ToTable("vw_hr_sum"); 
			this.Property(t => t.billcyc).HasColumnName("billcyc"); 
			this.Property(t => t.������).HasColumnName("������"); 
			this.Property(t => t.ʵ��).HasColumnName("ʵ��"); 
			this.Property(t => t.Ӧ�۳�).HasColumnName("Ӧ�۳�"); 
			this.Property(t => t.employerid).HasColumnName("employerid"); 
			this.Property(t => t.name).HasColumnName("name"); 
			this.Property(t => t.sjhm).HasColumnName("sjhm"); 
			this.Property(t => t.acctno).HasColumnName("acctno"); 
			this.Property(t => t.updatetime).HasColumnName("updatetime"); 
		 }
	 }
}
