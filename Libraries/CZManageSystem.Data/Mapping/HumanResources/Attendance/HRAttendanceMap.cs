using CZManageSystem.Data.Domain.HumanResources.Attendance;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Attendance
{
	public class HRAttendanceMap : EntityTypeConfiguration<HRAttendance>
	{
		public HRAttendanceMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 员工姓名
		/// <summary>
			  this.Property(t => t.Editor)

			.HasMaxLength(50);
 
		/// <summary>
		/// 上午上班IP
		/// <summary>
			  this.Property(t => t.AmOnIP)

			.HasMaxLength(50);
		/// <summary>
		/// 上午下班IP
		/// <summary>
			  this.Property(t => t.AmOffIP)

			.HasMaxLength(50);
		/// <summary>
		/// 下午上班IP
		/// <summary>
			  this.Property(t => t.PmOnIP)

			.HasMaxLength(50);
		/// <summary>
		/// 下午下班IP
		/// <summary>
			  this.Property(t => t.PmOffIP)

			.HasMaxLength(50);
		/// <summary>
		/// 备注
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(200);
			// Table & Column Mappings
 			 this.ToTable("HRAttendance"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 员工ID
			this.Property(t => t.EditorId).HasColumnName("EditorId"); 
			// 员工姓名
			this.Property(t => t.Editor).HasColumnName("Editor"); 
			// 时间
			this.Property(t => t.EditTime).HasColumnName("EditTime"); 
			// 日期
			this.Property(t => t.AtDate).HasColumnName("AtDate"); 
			// 上午上班时间
			this.Property(t => t.AmOnTime).HasColumnName("AmOnTime"); 
			// 上午上班IP
			this.Property(t => t.AmOnIP).HasColumnName("AmOnIP"); 
			// 上午下班时间
			this.Property(t => t.AmOffTime).HasColumnName("AmOffTime"); 
			// 上午下班IP
			this.Property(t => t.AmOffIP).HasColumnName("AmOffIP"); 
			// 下午上班时间
			this.Property(t => t.PmOnTime).HasColumnName("PmOnTime"); 
			// 下午上班IP
			this.Property(t => t.PmOnIP).HasColumnName("PmOnIP"); 
			// 下午下班时间
			this.Property(t => t.PmOffTime).HasColumnName("PmOffTime"); 
			// 下午下班IP
			this.Property(t => t.PmOffIP).HasColumnName("PmOffIP"); 
			// 备注
			this.Property(t => t.Remark).HasColumnName("Remark"); 
		 }
	 }
}
