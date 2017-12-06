using CZManageSystem.Data.Domain.HumanResources.Attendance;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Attendance
{
	public class HRHolidaysMap : EntityTypeConfiguration<HRHolidays>
	{
		public HRHolidaysMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 编辑者
		/// <summary>
			  this.Property(t => t.Editor)

			.HasMaxLength(50);
		/// <summary>
		/// 假日名称
		/// <summary>
			  this.Property(t => t.HolidayName)

			.HasMaxLength(50);
		/// <summary>
		/// 备注
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(200);
		/// <summary>
		/// 假期类别。1、公休假日；2、法定假日
		/// <summary>
			  this.Property(t => t.HolidayClass)

			.HasMaxLength(100);
			// Table & Column Mappings
 			 this.ToTable("HRHolidays"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 编辑者ID
			this.Property(t => t.EditorId).HasColumnName("EditorId"); 
			// 编辑者
			this.Property(t => t.Editor).HasColumnName("Editor"); 
			// 编辑时间
			this.Property(t => t.EditTime).HasColumnName("EditTime"); 
			// 假日名称
			this.Property(t => t.HolidayName).HasColumnName("HolidayName"); 
			// 年度
			this.Property(t => t.HolidayYear).HasColumnName("HolidayYear"); 
			// 开始时间
			this.Property(t => t.StartTime).HasColumnName("StartTime"); 
			// 结束时间
			this.Property(t => t.EndTime).HasColumnName("EndTime"); 
			// 备注
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			// 假期类别。1、公休假日；2、法定假日
			this.Property(t => t.HolidayClass).HasColumnName("HolidayClass"); 
		 }
	 }
}
