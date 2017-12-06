using CZManageSystem.Data.Domain.HumanResources.Attendance;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Attendance
{
	public class HRheckFingerprintMap : EntityTypeConfiguration<HRheckFingerprint>
	{
		public HRheckFingerprintMap()
		{
			// Primary Key
			 this.HasKey(t => t.FingerprintId);
		/// <summary>
		/// �û���
		/// <summary>
			  this.Property(t => t.UserName)
			 .IsRequired()
			.HasMaxLength(100);
			// Table & Column Mappings
 			 this.ToTable("HRheckFingerprint"); 
			// ����
			this.Property(t => t.FingerprintId).HasColumnName("FingerprintId"); 
			// �û�ID
			this.Property(t => t.UserId).HasColumnName("UserId"); 
			// �û���
			this.Property(t => t.UserName).HasColumnName("UserName"); 
			// ��ʶ
			this.Property(t => t.Flag).HasColumnName("Flag"); 
		 }
	 }
}
