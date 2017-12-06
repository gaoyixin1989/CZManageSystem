using CZManageSystem.Data.Domain.HumanResources.Vacation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Vacation
{
	public class HRVacationConfigMap : EntityTypeConfiguration<HRVacationConfig>
	{
		public HRVacationConfigMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 假期名称
		/// <summary>
			  this.Property(t => t.VacationName)

			.HasMaxLength(100);
		/// <summary>
		/// 假期限定天数
		/// <summary>
			  this.Property(t => t.Limit)

			.HasMaxLength(500);
		/// <summary>
		/// 范围
		/// <summary>
			  this.Property(t => t.Scope)

			.HasMaxLength(100);
			  this.Property(t => t.Daycalmethod)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("HRVacationConfig"); 
			// 主键
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 假期名称
			this.Property(t => t.VacationName).HasColumnName("VacationName"); 
			// 假期限定天数
			this.Property(t => t.Limit).HasColumnName("Limit"); 
			// 假期天数
			this.Property(t => t.SpanTime).HasColumnName("SpanTime"); 
			// 范围
			this.Property(t => t.Scope).HasColumnName("Scope"); 
			this.Property(t => t.Daycalmethod).HasColumnName("Daycalmethod"); 
		 }
	 }
}
