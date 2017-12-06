using CZManageSystem.Data.Domain.HumanResources.Attendance;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Attendance
{
	public class HRCheckAttendanceMap : EntityTypeConfiguration<HRCheckAttendance>
	{
		public HRCheckAttendanceMap()
		{
			// Primary Key
			 this.HasKey(t => t.AttendanceId);
		/// <summary>
		/// 上班登记IP
		/// <summary>
			  this.Property(t => t.IpOn)

			.HasMaxLength(50);
		/// <summary>
		/// 下班登记IP
		/// <summary>
			  this.Property(t => t.IpOff)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("HRCheckAttendance"); 
			this.Property(t => t.AttendanceId).HasColumnName("AttendanceId"); 
			// 用户ID
			this.Property(t => t.UserId).HasColumnName("UserId"); 
			// 日期
			this.Property(t => t.AtDate).HasColumnName("AtDate"); 
                this.Property(t => t.OffDate).HasColumnName("OffDate");
            // 上班时间
            this.Property(t => t.DoTime).HasColumnName("DoTime"); 
			this.Property(t => t.OffTime).HasColumnName("OffTime"); 
			this.Property(t => t.DoReallyTime).HasColumnName("DoReallyTime"); 
			this.Property(t => t.OffReallyTime).HasColumnName("OffReallyTime");
            this.Property(t => t.DoReallyDate).HasColumnName("DoReallyDate");
            this.Property(t => t.OffReallyDate).HasColumnName("OffReallyDate");
            // 上班登记IP
            this.Property(t => t.IpOn).HasColumnName("IpOn"); 
			// 下班登记IP
			this.Property(t => t.IpOff).HasColumnName("IpOff"); 
			// 分钟
			this.Property(t => t.Minute).HasColumnName("Minute"); 
			// 1、已申报；2、休假；3、外出
			this.Property(t => t.DoFlag).HasColumnName("DoFlag"); 
			// 轮休标志，1是为轮休状态
			this.Property(t => t.RotateDaysOffFlag).HasColumnName("RotateDaysOffFlag"); 
			// 上班：1、指纹登记；2、手机登记；3、通宝卡登记；4、门禁卡登记
			this.Property(t => t.FlagOn).HasColumnName("FlagOn"); 
			// 下班：1、指纹登记；2、手机登记；3、通宝卡登记；4、门禁卡登记
			this.Property(t => t.FlagOff).HasColumnName("FlagOff");

            this.HasOptional(t => t.Users )
              .WithMany()
              .HasForeignKey(d => d.UserId);
        }
	 }
}
