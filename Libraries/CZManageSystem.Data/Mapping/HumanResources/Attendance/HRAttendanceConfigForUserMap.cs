using CZManageSystem.Data.Domain.HumanResources.Attendance;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Attendance
{
	public class HRAttendanceConfigForUserMap : EntityTypeConfiguration<HRAttendanceConfigForUser>
	{
		public HRAttendanceConfigForUserMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 编辑者
		/// <summary>
			  this.Property(t => t.Editor)

			.HasMaxLength(50);
	 
		/// <summary>
		/// 备注
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(200);
	 
			// Table & Column Mappings
 			 this.ToTable("HRAttendanceConfigForUser"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 编辑者ID
			this.Property(t => t.EditorId).HasColumnName("EditorId"); 
			// 编辑者
			this.Property(t => t.Editor).HasColumnName("Editor"); 
			// 编辑时间
			this.Property(t => t.EditTime).HasColumnName("EditTime"); 
			// 上午上班时间
			this.Property(t => t.AMOnDuty).HasColumnName("AMOnDuty"); 
			// 上午下班时间
			this.Property(t => t.AMOffDuty).HasColumnName("AMOffDuty"); 
			// 下午上班时间
			this.Property(t => t.PMOnDuty).HasColumnName("PMOnDuty"); 
			// 下午下班时间
			this.Property(t => t.PMOffDuty).HasColumnName("PMOffDuty"); 
			// 时间跨度
			this.Property(t => t.SpanTime).HasColumnName("SpanTime"); 
			// 备注
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			// 上班可提前打卡时间
			this.Property(t => t.LimitTime).HasColumnName("LimitTime"); 
			// 用户id
			this.Property(t => t.UserId).HasColumnName("UserId"); 
		 }
	 }
}
