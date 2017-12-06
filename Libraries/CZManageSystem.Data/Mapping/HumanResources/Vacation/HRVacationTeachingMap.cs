using CZManageSystem.Data.Domain.HumanResources.Vacation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Vacation
{
	public class HRVacationTeachingMap : EntityTypeConfiguration<HRVacationTeaching>
	{
		public HRVacationTeachingMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 授课课程名称
		/// <summary>
			  this.Property(t => t.TeachingPlan)

			.HasMaxLength(500);
		/// <summary>
		/// 讲师级别
		/// <summary>
			  this.Property(t => t.TeacherType)

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
 			 this.ToTable("HRVacationTeaching"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 授课课程名称
			this.Property(t => t.TeachingPlan).HasColumnName("TeachingPlan"); 
			// 讲师级别
			this.Property(t => t.TeacherType).HasColumnName("TeacherType"); 
			// 天数
			this.Property(t => t.PeriodTime).HasColumnName("PeriodTime"); 
			// 开始时间
			this.Property(t => t.StartTime).HasColumnName("StartTime"); 
			// 结束时间
			this.Property(t => t.EndTime).HasColumnName("EndTime"); 
			// 备注
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			// 假期表的ID
			this.Property(t => t.VacationID).HasColumnName("VacationID"); 
			// 积分
			this.Property(t => t.Integral).HasColumnName("Integral"); 
			// 同意的标识
			this.Property(t => t.AgreeFlag).HasColumnName("AgreeFlag"); 
			// 销假ID
			this.Property(t => t.ReVacationID).HasColumnName("ReVacationID"); 
			this.Property(t => t.UserId).HasColumnName("UserId"); 
			this.Property(t => t.UserName).HasColumnName("UserName"); 
			this.Property(t => t.Ftst).HasColumnName("Ftst"); 
			this.Property(t => t.Ftet).HasColumnName("Ftet"); 
			this.Property(t => t.Hispt).HasColumnName("Hispt"); 
		 }
	 }
}
