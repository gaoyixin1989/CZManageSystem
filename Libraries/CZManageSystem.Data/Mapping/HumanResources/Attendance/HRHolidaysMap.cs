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
		/// �༭��
		/// <summary>
			  this.Property(t => t.Editor)

			.HasMaxLength(50);
		/// <summary>
		/// ��������
		/// <summary>
			  this.Property(t => t.HolidayName)

			.HasMaxLength(50);
		/// <summary>
		/// ��ע
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(200);
		/// <summary>
		/// �������1�����ݼ��գ�2����������
		/// <summary>
			  this.Property(t => t.HolidayClass)

			.HasMaxLength(100);
			// Table & Column Mappings
 			 this.ToTable("HRHolidays"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// �༭��ID
			this.Property(t => t.EditorId).HasColumnName("EditorId"); 
			// �༭��
			this.Property(t => t.Editor).HasColumnName("Editor"); 
			// �༭ʱ��
			this.Property(t => t.EditTime).HasColumnName("EditTime"); 
			// ��������
			this.Property(t => t.HolidayName).HasColumnName("HolidayName"); 
			// ���
			this.Property(t => t.HolidayYear).HasColumnName("HolidayYear"); 
			// ��ʼʱ��
			this.Property(t => t.StartTime).HasColumnName("StartTime"); 
			// ����ʱ��
			this.Property(t => t.EndTime).HasColumnName("EndTime"); 
			// ��ע
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			// �������1�����ݼ��գ�2����������
			this.Property(t => t.HolidayClass).HasColumnName("HolidayClass"); 
		 }
	 }
}
