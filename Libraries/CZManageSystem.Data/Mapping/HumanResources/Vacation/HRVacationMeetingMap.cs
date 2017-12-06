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
		/// ��������
		/// <summary>
			  this.Property(t => t.MeetingName)

			.HasMaxLength(500);
		/// <summary>
		/// ��ע
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(1073741823);
			// Table & Column Mappings
 			 this.ToTable("HRVacationMeeting"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ��������
			this.Property(t => t.MeetingName).HasColumnName("MeetingName"); 
			// ����
			this.Property(t => t.PeriodTime).HasColumnName("PeriodTime"); 
			// ��ʼʱ��
			this.Property(t => t.StartTime).HasColumnName("StartTime"); 
			// ����ʱ��
			this.Property(t => t.EndTime).HasColumnName("EndTime"); 
			// ��ע
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			// ���ڱ��ID
			this.Property(t => t.VacationID).HasColumnName("VacationID"); 
			// ����
			this.Property(t => t.Integral).HasColumnName("Integral"); 
			// ͬ��ı�ʶ
			this.Property(t => t.AgreeFlag).HasColumnName("AgreeFlag"); 
			// ����ID
			this.Property(t => t.ReVacationID).HasColumnName("ReVacationID"); 
		 }
	 }
}
