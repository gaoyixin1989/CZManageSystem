using CZManageSystem.Data.Domain.HumanResources.Attendance;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Attendance
{
	public class HRHolidayWorkMap : EntityTypeConfiguration<HRHolidayWork>
	{
		public HRHolidayWorkMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 编辑者
		/// <summary>
			  this.Property(t => t.Editor)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("HRHolidayWork"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 编辑者ID
			this.Property(t => t.EditorId).HasColumnName("EditorId"); 
			// 编辑者
			this.Property(t => t.Editor).HasColumnName("Editor"); 
			// 编辑时间
			this.Property(t => t.EditTime).HasColumnName("EditTime"); 
			// 开始时间
			this.Property(t => t.StartTime).HasColumnName("StartTime"); 
			// 结束时间
			this.Property(t => t.EndTime).HasColumnName("EndTime"); 
		 }
	 }
}
