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
		/// �༭��
		/// <summary>
			  this.Property(t => t.Editor)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("HRHolidayWork"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// �༭��ID
			this.Property(t => t.EditorId).HasColumnName("EditorId"); 
			// �༭��
			this.Property(t => t.Editor).HasColumnName("Editor"); 
			// �༭ʱ��
			this.Property(t => t.EditTime).HasColumnName("EditTime"); 
			// ��ʼʱ��
			this.Property(t => t.StartTime).HasColumnName("StartTime"); 
			// ����ʱ��
			this.Property(t => t.EndTime).HasColumnName("EndTime"); 
		 }
	 }
}
