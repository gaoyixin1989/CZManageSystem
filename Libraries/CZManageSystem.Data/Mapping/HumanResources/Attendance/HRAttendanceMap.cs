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
		/// Ա������
		/// <summary>
			  this.Property(t => t.Editor)

			.HasMaxLength(50);
 
		/// <summary>
		/// �����ϰ�IP
		/// <summary>
			  this.Property(t => t.AmOnIP)

			.HasMaxLength(50);
		/// <summary>
		/// �����°�IP
		/// <summary>
			  this.Property(t => t.AmOffIP)

			.HasMaxLength(50);
		/// <summary>
		/// �����ϰ�IP
		/// <summary>
			  this.Property(t => t.PmOnIP)

			.HasMaxLength(50);
		/// <summary>
		/// �����°�IP
		/// <summary>
			  this.Property(t => t.PmOffIP)

			.HasMaxLength(50);
		/// <summary>
		/// ��ע
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(200);
			// Table & Column Mappings
 			 this.ToTable("HRAttendance"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// Ա��ID
			this.Property(t => t.EditorId).HasColumnName("EditorId"); 
			// Ա������
			this.Property(t => t.Editor).HasColumnName("Editor"); 
			// ʱ��
			this.Property(t => t.EditTime).HasColumnName("EditTime"); 
			// ����
			this.Property(t => t.AtDate).HasColumnName("AtDate"); 
			// �����ϰ�ʱ��
			this.Property(t => t.AmOnTime).HasColumnName("AmOnTime"); 
			// �����ϰ�IP
			this.Property(t => t.AmOnIP).HasColumnName("AmOnIP"); 
			// �����°�ʱ��
			this.Property(t => t.AmOffTime).HasColumnName("AmOffTime"); 
			// �����°�IP
			this.Property(t => t.AmOffIP).HasColumnName("AmOffIP"); 
			// �����ϰ�ʱ��
			this.Property(t => t.PmOnTime).HasColumnName("PmOnTime"); 
			// �����ϰ�IP
			this.Property(t => t.PmOnIP).HasColumnName("PmOnIP"); 
			// �����°�ʱ��
			this.Property(t => t.PmOffTime).HasColumnName("PmOffTime"); 
			// �����°�IP
			this.Property(t => t.PmOffIP).HasColumnName("PmOffIP"); 
			// ��ע
			this.Property(t => t.Remark).HasColumnName("Remark"); 
		 }
	 }
}
