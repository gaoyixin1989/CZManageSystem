using CZManageSystem.Data.Domain.HumanResources.Attendance;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Attendance
{
	public class HRUnattendLinkMap : EntityTypeConfiguration<HRUnattendLink>
	{
		public HRUnattendLinkMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
			// Table & Column Mappings
 			 this.ToTable("HRUnattendLink"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			this.Property(t => t.ApplyRecordId).HasColumnName("ApplyRecordId"); 
			this.Property(t => t.AttendanceId).HasColumnName("AttendanceId"); 
		 }
	 }
}
