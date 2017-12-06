using CZManageSystem.Data.Domain.HumanResources.Vacation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Vacation
{
	public class HRVacationCoursesMap : EntityTypeConfiguration<HRVacationCourses>
	{
		public HRVacationCoursesMap()
		{
			// Primary Key
			 this.HasKey(t => t.CoursesId);
		/// <summary>
		/// 课程名称
		/// <summary>
			  this.Property(t => t.CoursesName)

			.HasMaxLength(500);
		/// <summary>
		/// 课程类别
		/// <summary>
			  this.Property(t => t.CoursesType)

			.HasMaxLength(50);
		/// <summary>
		/// 主办单位
		/// <summary>
			  this.Property(t => t.ProvinceCity)

			.HasMaxLength(50);
		/// <summary>
		/// 备注
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(1073741823);
			  this.Property(t => t.UserName)

			.HasMaxLength(100);
			  this.Property(t => t.Ftst)

			.HasMaxLength(100);
			  this.Property(t => t.Ftet)

			.HasMaxLength(100);
			  this.Property(t => t.Hispt)

			.HasMaxLength(100);
			// Table & Column Mappings
 			 this.ToTable("HRVacationCourses"); 
			this.Property(t => t.CoursesId).HasColumnName("CoursesId"); 
			// 课程名称
			this.Property(t => t.CoursesName).HasColumnName("CoursesName"); 
			// 课程类别
			this.Property(t => t.CoursesType).HasColumnName("CoursesType"); 
			// 主办单位
			this.Property(t => t.ProvinceCity).HasColumnName("ProvinceCity"); 
			// 开始时间
			this.Property(t => t.StartTime).HasColumnName("StartTime"); 
			// 结束时间
			this.Property(t => t.EndTime).HasColumnName("EndTime"); 
			// 培训天数
			this.Property(t => t.PeriodTime).HasColumnName("PeriodTime"); 
			// 备注
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			// 假期表ID
			this.Property(t => t.VacationID).HasColumnName("VacationID"); 
			// 积分
			this.Property(t => t.Integral).HasColumnName("Integral"); 
			// 同意的标识
			this.Property(t => t.AgreeFlag).HasColumnName("AgreeFlag"); 
			this.Property(t => t.UserId).HasColumnName("UserId"); 
			this.Property(t => t.DaoId).HasColumnName("DaoId"); 
			this.Property(t => t.UserName).HasColumnName("UserName"); 
			this.Property(t => t.Ftst).HasColumnName("Ftst"); 
			this.Property(t => t.Ftet).HasColumnName("Ftet"); 
			this.Property(t => t.Hispt).HasColumnName("Hispt"); 
			// 销假ID
			this.Property(t => t.ReVacationID).HasColumnName("ReVacationID"); 
		 }
	 }
}
