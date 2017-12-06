using CZManageSystem.Data.Domain.HumanResources.Vacation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Vacation
{
	public class HRVacationMeetingMap : EntityTypeConfiguration<HRVacationMeeting>
	{
		public HRVacationMeetingMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 会议名称
		/// <summary>
			  this.Property(t => t.MeetingName)

			.HasMaxLength(500);
		/// <summary>
		/// 备注
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(1073741823);
			// Table & Column Mappings
 			 this.ToTable("HRVacationMeeting"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 会议名称
			this.Property(t => t.MeetingName).HasColumnName("MeetingName"); 
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
		 }
	 }
}
