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
		/// 用户名
		/// <summary>
			  this.Property(t => t.UserName)
			 .IsRequired()
			.HasMaxLength(100);
			// Table & Column Mappings
 			 this.ToTable("HRheckFingerprint"); 
			// 主键
			this.Property(t => t.FingerprintId).HasColumnName("FingerprintId"); 
			// 用户ID
			this.Property(t => t.UserId).HasColumnName("UserId"); 
			// 用户名
			this.Property(t => t.UserName).HasColumnName("UserName"); 
			// 标识
			this.Property(t => t.Flag).HasColumnName("Flag"); 
		 }
	 }
}
