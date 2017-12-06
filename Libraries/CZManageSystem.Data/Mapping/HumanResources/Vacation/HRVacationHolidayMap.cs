using CZManageSystem.Data.Domain.HumanResources.Vacation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Vacation
{
	public class HRVacationHolidayMap : EntityTypeConfiguration<HRVacationHoliday>
	{
		public HRVacationHolidayMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 休假类型
		/// <summary>
			  this.Property(t => t.VacationType)

			.HasMaxLength(50);
		/// <summary>
		/// 公假类型
		/// <summary>
			  this.Property(t => t.VacationClass)

			.HasMaxLength(50);
		/// <summary>
		/// 原因
		/// <summary>
			  this.Property(t => t.Reason)

			.HasMaxLength(1000);
			// Table & Column Mappings
 			 this.ToTable("HRVacationHoliday"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 用户ID
			this.Property(t => t.UserId).HasColumnName("UserId"); 
			// 年度
			this.Property(t => t.YearDate).HasColumnName("YearDate"); 
			// 休假类型
			this.Property(t => t.VacationType).HasColumnName("VacationType"); 
			// 公假类型
			this.Property(t => t.VacationClass).HasColumnName("VacationClass"); 
			// 天数
			this.Property(t => t.PeriodTime).HasColumnName("PeriodTime"); 
			// 开始时间
			this.Property(t => t.StartTime).HasColumnName("StartTime"); 
			// 结束时间
			this.Property(t => t.EndTime).HasColumnName("EndTime"); 
			// 原因
			this.Property(t => t.Reason).HasColumnName("Reason");

            this.HasOptional(t => t.UserObj).WithMany().HasForeignKey(d => d.UserId);
        }
	 }
}
