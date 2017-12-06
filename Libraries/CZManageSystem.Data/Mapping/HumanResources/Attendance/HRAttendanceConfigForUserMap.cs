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
		/// �༭��
		/// <summary>
			  this.Property(t => t.Editor)

			.HasMaxLength(50);
	 
		/// <summary>
		/// ��ע
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(200);
	 
			// Table & Column Mappings
 			 this.ToTable("HRAttendanceConfigForUser"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// �༭��ID
			this.Property(t => t.EditorId).HasColumnName("EditorId"); 
			// �༭��
			this.Property(t => t.Editor).HasColumnName("Editor"); 
			// �༭ʱ��
			this.Property(t => t.EditTime).HasColumnName("EditTime"); 
			// �����ϰ�ʱ��
			this.Property(t => t.AMOnDuty).HasColumnName("AMOnDuty"); 
			// �����°�ʱ��
			this.Property(t => t.AMOffDuty).HasColumnName("AMOffDuty"); 
			// �����ϰ�ʱ��
			this.Property(t => t.PMOnDuty).HasColumnName("PMOnDuty"); 
			// �����°�ʱ��
			this.Property(t => t.PMOffDuty).HasColumnName("PMOffDuty"); 
			// ʱ����
			this.Property(t => t.SpanTime).HasColumnName("SpanTime"); 
			// ��ע
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			// �ϰ����ǰ��ʱ��
			this.Property(t => t.LimitTime).HasColumnName("LimitTime"); 
			// �û�id
			this.Property(t => t.UserId).HasColumnName("UserId"); 
		 }
	 }
}
